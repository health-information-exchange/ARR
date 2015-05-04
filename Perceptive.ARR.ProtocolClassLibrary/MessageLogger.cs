using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Perceptive.ARR.HelperLibrary;
using System.Threading.Tasks;

namespace Perceptive.ARR.ProtocolClassLibrary
{
    internal class MessageLogger
    {
        internal void LogMessage(RepositoryRequest request)
        {
            string data = Encoding.UTF8.GetString(request.Data, 0, request.Data.Length);
            if (request.Protocol != MessageProtocol.UDP)
            {
                //An extra validation to check if all data is received                
                string firstPart = data.Split(Constants.Separator.ToCharArray())[0];
                int count = 0;
                if (int.TryParse(firstPart, out count)) // && count == request.Data.Length) Removing count validaton as of now
                {
                    //Data correct, Proceed
                    request.ByteCount = count;
                    data = data.Substring(firstPart.Length + 1);
                }
                else
                {
                    RecordAuditTrail(request);
                    return;
                }
            }

            SyslogMessage message = new SyslogMessage(data);
            if (message.IsValid)
                request.ValidMessage = message;

            RecordAuditTrail(request);            
        }

        private void RecordAuditTrail(RepositoryRequest request)
        {
            if (!AppsettingManager.Instance.LogIntoMessageQueue)
            {
                Task<bool> t = Task<bool>.Factory.StartNew(() => new DBLogger().RecordAuditTrail(request));
                if (!t.Result)
                    Helper.LogMessage(Encoding.UTF8.GetString(request.Data), Constants.LogCategoryName_Service);
            }
            else
                AppsettingManager.Instance.AddToQueue(request);
        }
    }
}
