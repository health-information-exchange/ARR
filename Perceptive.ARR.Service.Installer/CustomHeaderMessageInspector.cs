using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Xml;

namespace Perceptive.ARR.Service.Installer
{
    class CorsState
    {
        public Message Message;
    }

    /// <summary>
    /// This behavior should be added to both endpoint and operations to make it work.
    /// This will detect cors requests and reply on preflight request, plus supply any needed headers to make rest wcf calls work seamlessly.
    /// </summary>
    /// <remarks>
    /// 2011-06-23 dan: Created
    /// </remarks>
    //[AttributeUsage(AttributeTargets.Method | AttributeTargets.Interface)]
    public class CorsBehaviorAttribute : BehaviorExtensionElement, IEndpointBehavior, IOperationBehavior
    //public class CorsBehaviorAttribute : Attribute, IEndpointBehavior, IOperationBehavior
    {
        public override Type BehaviorType { get { return typeof(CorsBehaviorAttribute); } }

        protected override object CreateBehavior() { return new CorsBehaviorAttribute(); }


        /* *******************************************************************
         *  Properties 
         * *******************************************************************/
        #region public string AllowOrigin
        /// <summary>
        /// Get/Sets the AllowOrigin of the CrossOriginResourceSharingBehaviorAttribute
        /// </summary>
        /// <value></value>
        public string AllowOrigin
        {
            get { return _allowOrigin; }
            set { _allowOrigin = value; }
        }
        private string _allowOrigin = "*";
        #endregion
        #region public string AllowMethods
        /// <summary>
        /// Get/Sets the AllowMethods of the CrossOriginResourceSharingBehaviorAttribute
        /// </summary>
        /// <value></value>
        public string AllowMethods
        {
            get { return _allowMethods; }
            set { _allowMethods = value; }
        }
        private string _allowMethods = "POST, OPTIONS, GET";
        #endregion
        #region public string AllowHeaders
        /// <summary>
        /// Get/Sets the AllowHeaders of the CrossOriginResourceSharingBehaviorAttribute
        /// </summary>
        /// <value></value>
        public string AllowHeaders
        {
            get { return _allowHeaders; }
            set { _allowHeaders = value; }
        }
        private string _allowHeaders = "Content-Type, Accept, Authorization, x-requested-with";
        #endregion
        /* *******************************************************************
         *  Methods 
         * *******************************************************************/
        #region public void Validate(ServiceEndpoint endpoint)
        /// <summary>
        /// Implement to confirm that the endpoint meets some intended criteria.
        /// </summary>
        /// <param name="endpoint">The endpoint to validate.</param>
        public void Validate(ServiceEndpoint endpoint) { }
        #endregion
        #region public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        /// <summary>
        /// Implement to pass data at runtime to bindings to support custom behavior.
        /// </summary>
        /// <param name="endpoint">The endpoint to modify.</param>
        /// <param name="bindingParameters">The objects that binding elements require to support the behavior.</param>
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters) { }
        #endregion
        #region public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        /// <summary>
        /// Implements a modification or extension of the service across an endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint that exposes the contract.</param>
        /// <param name="endpointDispatcher">The endpoint dispatcher to be modified or extended.</param>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            //adds an inspector that detect cors requests and marks them so the operation invoker/formatter can detect it
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new CorsDispatchMessageInspector(endpoint, this));
        }
        #endregion
        #region public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        /// <summary>
        /// Implements a modification or extension of the client across an endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint that is to be customized.</param>
        /// <param name="clientRuntime">The client runtime to be customized.</param>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime) { }
        #endregion
        #region public void Validate(OperationDescription operationDescription)
        /// <summary>
        /// Implement to confirm that the operation meets some intended criteria.
        /// </summary>
        /// <param name="operationDescription">The operation being examined. Use for examination only. If the operation description is modified, the results are undefined.</param>
        public void Validate(OperationDescription operationDescription) { }
        #endregion
        #region public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        /// <summary>
        /// Implements a modification or extension of the service across an operation.
        /// </summary>
        /// <param name="operationDescription">The operation being examined. Use for examination only. If the operation description is modified, the results are undefined.</param>
        /// <param name="dispatchOperation">The run-time object that exposes customization properties for the operation described by <paramref name="operationDescription"/>.</param>
        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            //For every opertation we inject a formatter and an invoker to detect Cors calls
            dispatchOperation.Formatter = new CorsFormatter(dispatchOperation.Formatter);
            dispatchOperation.Invoker = new CorsInvoker(dispatchOperation.Invoker);
        }
        #endregion
        #region public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        /// <summary>
        /// Implements a modification or extension of the client across an operation.
        /// </summary>
        /// <param name="operationDescription">The operation being examined. Use for examination only. If the operation description is modified, the results are undefined.</param>
        /// <param name="clientOperation">The run-time object that exposes customization properties for the operation described by <paramref name="operationDescription"/>.</param>
        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation) { }
        #endregion
        #region public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        /// <summary>
        /// Implement to pass data at runtime to bindings to support custom behavior.
        /// </summary>
        /// <param name="operationDescription">The operation being examined. Use for examination only. If the operation description is modified, the results are undefined.</param>
        /// <param name="bindingParameters">The collection of objects that binding elements require to support the behavior.</param>
        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters) { }
        #endregion


        /// <summary>
        /// Inspects the incoming message and looks for cors requests
        /// </summary>
        /// <remarks>
        /// 2011-06-23 dan: Created
        /// </remarks>
        public class CorsDispatchMessageInspector : IDispatchMessageInspector
        {
            /* *******************************************************************
             *  Properties 
             * *******************************************************************/
            private readonly ServiceEndpoint _serviceEndpoint;
            private readonly CorsBehaviorAttribute _behavior;
            internal const string CrossOriginResourceSharingPropertyName = "CrossOriginResourcSharingState";
            /* *******************************************************************
             *  Constructors 
             * *******************************************************************/
            #region public CorsDispatchMessageInspector()
            /// <summary>
            /// Initializes a new instance of the <b>CorsDispatchMessageInspector</b> class.
            /// </summary>
            public CorsDispatchMessageInspector(ServiceEndpoint serviceEndpoint, CorsBehaviorAttribute behavior)
            {
                _serviceEndpoint = serviceEndpoint;
                _behavior = behavior;
            }
            #endregion
            /* *******************************************************************
         *  Methods 
         * *******************************************************************/
            #region public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
            /// <summary>
            /// Called after an inbound message has been received but before the message is dispatched to the intended operation.
            /// </summary>
            /// <param name="request">The request message.</param>
            /// <param name="channel">The incoming channel.</param>
            /// <param name="instanceContext">The current service instance.</param>
            /// <returns>
            /// The object used to correlate state. This object is passed back in the <see cref="M:System.ServiceModel.Dispatcher.IDispatchMessageInspector.BeforeSendReply(System.ServiceModel.Channels.Message@,System.Object)"/> method.
            /// </returns>
            public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
            {
                CorsState state = null;
                HttpRequestMessageProperty responseProperty = null;
                if (request.Properties.ContainsKey(HttpRequestMessageProperty.Name))
                {
                    responseProperty = request.Properties[HttpRequestMessageProperty.Name] as HttpRequestMessageProperty;
                }

                if (responseProperty != null)
                {

                    //Handle cors requests
                    var origin = responseProperty.Headers["Origin"];
                    if (!string.IsNullOrEmpty(origin))
                    {
                        state = new CorsState();
                        //if a cors options request (preflight) is detected, we create our own reply message and don't invoke any operation at all.
                        if (responseProperty.Method == "OPTIONS")
                        {
                            state.Message = Message.CreateMessage(request.Version, FindReplyAction(request.Headers.Action), new EmptyBodyWriter());
                        }
                        request.Properties.Add(CrossOriginResourceSharingPropertyName, state);
                    }
                }
                return state;
            }
            #endregion
            #region private string FindReplyAction(string requestAction)
            /// <summary>
            /// Finds the reply action based on the supplied request action
            /// </summary>
            /// <param name="requestAction">The request action for witch the reply action should be found.</param>
            /// <returns></returns>
            private string FindReplyAction(string requestAction)
            {
                foreach (var operation in _serviceEndpoint.Contract.Operations)
                {
                    if (operation.Messages[0].Action == requestAction)
                    {
                        return operation.Messages[1].Action;
                    }
                }
                return null;
            }
            #endregion

            /// <summary>
            /// A simple body writer that writes nothing
            /// </summary>
            class EmptyBodyWriter : BodyWriter
            {
                #region public EmptyBodyWriter()
                /// <summary>
                /// Initializes a new instance of the <b>EmptyBodyWriter</b> class.
                /// </summary>
                public EmptyBodyWriter() : base(true) { }
                #endregion
                #region protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
                /// <summary>
                /// When implemented, provides an extensibility point when the body contents are written.
                /// </summary>
                /// <param name="writer">The <see cref="T:System.Xml.XmlDictionaryWriter"/> used to write out the message body.</param>
                protected override void OnWriteBodyContents(XmlDictionaryWriter writer) { }
                #endregion
            }

            #region public void BeforeSendReply(ref Message reply, object correlationState)
            /// <summary>
            /// Called after the operation has returned but before the reply message is sent.
            /// </summary>
            /// <param name="reply">The reply message. This value is null if the operation is one way.</param>
            /// <param name="correlationState">The correlation object returned from the <see cref="M:System.ServiceModel.Dispatcher.IDispatchMessageInspector.AfterReceiveRequest(System.ServiceModel.Channels.Message@,System.ServiceModel.IClientChannel,System.ServiceModel.InstanceContext)"/> method.</param>
            public void BeforeSendReply(ref Message reply, object correlationState)
            {
                var state = correlationState as CorsState;
                if (state != null)
                {
                    if (state.Message != null)
                    {
                        reply = state.Message;
                    }
                    HttpResponseMessageProperty responseProperty = null;
                    if (reply.Properties.ContainsKey(HttpResponseMessageProperty.Name))
                    {
                        responseProperty = reply.Properties[HttpResponseMessageProperty.Name] as HttpResponseMessageProperty;
                    }
                    if (responseProperty == null)
                    {
                        responseProperty = new HttpResponseMessageProperty();
                        reply.Properties.Add(HttpResponseMessageProperty.Name, responseProperty);
                    }
                    //Acao should be added for all cors responses
                    responseProperty.Headers.Set("Access-Control-Allow-Origin", _behavior.AllowOrigin);
                    if (state.Message != null)
                    {
                        //the following headers should only be added for OPTIONS requests
                        responseProperty.Headers.Set("Access-Control-Allow-Methods", _behavior.AllowMethods);
                        responseProperty.Headers.Set("Access-Control-Allow-Headers", _behavior.AllowHeaders);
                    }
                }
            }
            #endregion

        }
    }

    /// <summary>
    /// Handles invocations for the Cors behaviour
    /// </summary>
    /// <remarks>
    /// 2011-06-23 dan: Created
    /// </remarks>
    public class CorsInvoker : IOperationInvoker
    {
        /* *******************************************************************
         *  Properties 
         * *******************************************************************/
        #region public bool IsSynchronous
        /// <summary>
        /// Gets a value that specifies whether the <see cref="M:System.ServiceModel.Dispatcher.IOperationInvoker.Invoke(System.Object,System.Object[],System.Object[]@)"/> or <see cref="M:System.ServiceModel.Dispatcher.IOperationInvoker.InvokeBegin(System.Object,System.Object[],System.AsyncCallback,System.Object)"/> method is called by the dispatcher.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// true if the dispatcher invokes the synchronous operation; otherwise, false.
        /// </returns>
        public bool IsSynchronous
        {
            get { return true; }
        }
        #endregion
        private readonly IOperationInvoker _originalInvoker;
        /* *******************************************************************
         *  Constructors 
         * *******************************************************************/
        #region public CorsInvoker()
        /// <summary>
        /// Initializes a new instance of the <b>CorsInvoker</b> class.
        /// </summary>
        /// <param name="invoker"></param>
        public CorsInvoker(IOperationInvoker invoker)
        {
            if (!invoker.IsSynchronous)
            {
                throw new NotSupportedException("This implementation only supports syncronous invokers.");
            }
            _originalInvoker = invoker;
        }
        #endregion
        /* *******************************************************************
         *  Methods 
         * *******************************************************************/
        #region public object[] AllocateInputs()
        /// <summary>
        /// Returns an <see cref="T:System.Array"/> of parameter objects.
        /// </summary>
        /// <returns>
        /// The parameters that are to be used as arguments to the operation.
        /// </returns>
        public object[] AllocateInputs()
        {
            return _originalInvoker.AllocateInputs();
        }
        #endregion
        #region public object Invoke(object instance, object[] inputs, out object[] outputs)
        /// <summary>
        /// Returns an object and a set of output objects from an instance and set of input objects.  
        /// </summary>
        /// <param name="instance">The object to be invoked.</param>
        /// <param name="inputs">The inputs to the method.</param>
        /// <param name="outputs">The outputs from the method.</param>
        /// <returns>
        /// The return value.
        /// </returns>
        public object Invoke(object instance, object[] inputs, out object[] outputs)
        {
            if (OperationContext.Current.IncomingMessageProperties.ContainsKey(Perceptive.ARR.Service.Installer.CorsBehaviorAttribute.CorsDispatchMessageInspector.CrossOriginResourceSharingPropertyName))
            {
                var state = OperationContext.Current.IncomingMessageProperties[Perceptive.ARR.Service.Installer.CorsBehaviorAttribute.CorsDispatchMessageInspector.CrossOriginResourceSharingPropertyName] as CorsState;
                if (state != null && state.Message != null)
                {
                    outputs = null;
                    return null;
                }
            }
            return _originalInvoker.Invoke(instance, inputs, out outputs);
        }
        #endregion
        #region public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
        /// <summary>
        /// An asynchronous implementation of the <see cref="M:System.ServiceModel.Dispatcher.IOperationInvoker.Invoke(System.Object,System.Object[],System.Object[]@)"/> method.
        /// </summary>
        /// <param name="instance">The object to be invoked.</param>
        /// <param name="inputs">The inputs to the method.</param>
        /// <param name="callback">The asynchronous callback object.</param>
        /// <param name="state">Associated state data.</param>
        /// <returns>
        /// A <see cref="T:System.IAsyncResult"/> used to complete the asynchronous call.
        /// </returns>
        /// <exception cref="NotSupportedException"></exception>
        public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
        }
        #endregion
        #region public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
        /// <summary>
        /// The asynchronous end method.
        /// </summary>
        /// <param name="instance">The object invoked.</param>
        /// <param name="outputs">The outputs from the method.</param>
        /// <param name="result">The <see cref="T:System.IAsyncResult"/> object.</param>
        /// <returns>
        /// The return value.
        /// </returns>
        /// <exception cref="NotSupportedException"></exception>
        public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
        {
            throw new NotSupportedException();
        }
        #endregion
    }

    /// <summary>
    /// Handles request/response formatting for the Cors behavior
    /// </summary>
    /// <remarks>
    /// 2011-06-23 dan: Created
    /// </remarks>
    public class CorsFormatter : IDispatchMessageFormatter
    {
        /* *******************************************************************
         *  Properties 
         * *******************************************************************/
        private readonly IDispatchMessageFormatter _originalFormatter;
        /* *******************************************************************
         *  Constructors 
         * *******************************************************************/
        #region public CorsFormatter(IDispatchMessageFormatter formatter)
        /// <summary>
        /// Initializes a new instance of the <b>CorsFormatter</b> class.
        /// </summary>
        /// <param name="formatter"></param>
        public CorsFormatter(IDispatchMessageFormatter formatter)
        {
            _originalFormatter = formatter;
        }
        #endregion
        /* *******************************************************************
         *  Methods 
         * *******************************************************************/
        /// <summary>
        /// Deserializes a message into an array of parameters.
        /// </summary>
        /// <param name="message">The incoming message.</param><param name="parameters">The objects that are passed to the operation as parameters.</param>
        public void DeserializeRequest(Message message, object[] parameters)
        {
            if (message.Properties.ContainsKey(Perceptive.ARR.Service.Installer.CorsBehaviorAttribute.CorsDispatchMessageInspector.CrossOriginResourceSharingPropertyName))
            {
                var state = message.Properties[Perceptive.ARR.Service.Installer.CorsBehaviorAttribute.CorsDispatchMessageInspector.CrossOriginResourceSharingPropertyName] as CorsState;
                if (state != null)
                {
                    //if we have a message ready, skip normal deserialization
                    if (state.Message != null)
                    {
                        OperationContext.Current.OutgoingMessageProperties.Add(Perceptive.ARR.Service.Installer.CorsBehaviorAttribute.CorsDispatchMessageInspector.CrossOriginResourceSharingPropertyName, state);
                        return;
                    }
                }
            }
            _originalFormatter.DeserializeRequest(message, parameters);
        }
        /// <summary>
        /// Serializes a reply message from a specified message version, array of parameters, and a return value.
        /// </summary>
        /// <returns>
        /// The serialized reply message.
        /// </returns>
        /// <param name="messageVersion">The SOAP message version.</param><param name="parameters">The out parameters.</param><param name="result">The return value.</param>
        public Message SerializeReply(MessageVersion messageVersion, object[] parameters, object result)
        {

            //see if we have a cors state with a predefined message.
            //in that case where we can ignore the whole serialization process
            if (OperationContext.Current.OutgoingMessageProperties.ContainsKey(Perceptive.ARR.Service.Installer.CorsBehaviorAttribute.CorsDispatchMessageInspector.CrossOriginResourceSharingPropertyName))
            {
                var state = OperationContext.Current.OutgoingMessageProperties[Perceptive.ARR.Service.Installer.CorsBehaviorAttribute.CorsDispatchMessageInspector.CrossOriginResourceSharingPropertyName] as CorsState;
                if (state != null && state.Message != null)
                {
                    return state.Message;
                }
            }
            return _originalFormatter.SerializeReply(messageVersion, parameters, result);
        }
    }
}