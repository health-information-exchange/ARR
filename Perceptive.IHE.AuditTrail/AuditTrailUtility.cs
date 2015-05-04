using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;
using System.Net.Sockets;
using System.Configuration;
using System.Reflection;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Perceptive.IHE.AuditTrail
{
    public static partial class AuditTrail
    {
        private const string CompanyName = "PerceptiveSoftware";
        public const string DATETIME_FORMAT = "yyyy-MM-ddTHH:mm:ss.fffffffzzzzzz";
        //private static string hostName;

        //private static string PrepareSysLogFormatMessage(string auditMessage, string appName)
        //{
        //    //SYSLOG-MSG      = HEADER SP STRUCTURED-DATA [SP MSG]
        //    //HEADER          = PRI VERSION SP TIMESTAMP SP HOSTNAME SP APP-NAME SP PROCID SP MSGID
        //    //http://tools.ietf.org/html/rfc5424

        //    string auditMessageSyslogFormat;
        //    string emptyHyphens = string.Format(CultureInfo.InvariantCulture, "{1} {0} - ", IdGenerator.GenerateRandomId(), System.Diagnostics.Process.GetCurrentProcess().Id);
        //    string pri = "85";//CP417 for ATNA 5424
        //    string timeStamp = GetTimeStamp();
        //    auditMessageSyslogFormat = String.Format(@"<{0}>{5} {1} {2} {3} {6}{4}", pri, timeStamp, HostName, appName, auditMessage, "1", emptyHyphens);
        //    return auditMessageSyslogFormat;
        //}

        //private static string GetTimeStamp()
        //{
        //    //CP417 for ATNA 5424
        //    string timestamp = string.Empty;
        //    timestamp = DateTime.UtcNow.ToString("s") + "Z";

        //    return timestamp;
        //}

        //private static string HostName
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(hostName))
        //            hostName = Dns.GetHostName();

        //        return hostName;
        //    }
        //}

        public static string LocalIPAddress
        {
            get
            {
                return getIpAddress();
            }
        }

        private static string getIpAddress()
        {
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                }
            }

            return localIP;
        }

        //private static void SendDataForAuditTrail(AuditMessage message, string appName)
        //{
        //    StringBuilder builder = new StringBuilder();
        //    builder.Append(@"<?xml version=""1.0"" encoding=""utf-8"" ?>");
        //    builder.Append(GetXmlElementFromEventData(message));

        //    string auditMessageSyslogFormat = PrepareSysLogFormatMessage(builder.ToString(), appName);

        //    int useLog = UseLog;

        //    if(useLog == 0)
        //        SendViaUDP(SyslogCollectorAddress, SyslogCollectorPortUdp, auditMessageSyslogFormat);
        //    else
        //        SendViaTCP(SyslogCollectorAddress, (useLog ==2 ) ? SyslogCollectorPortTls : SyslogCollectorPortTcp, auditMessageSyslogFormat, useLog == 2);
        //}

        //private static void SendViaTCP(string address, int portTls, string message, bool tls)
        //{
        //    try
        //    {
        //        // Convert to TLS specific message format
        //        //SYSLOG-FRAME = MSG-LEN SP SYSLOG-MSG
        //        //The message length (MSG-LEN) is the octet count of the SYSLOG-MSG in the SYSLOG-FRAME

        //        message = string.Format(CultureInfo.InvariantCulture, "{0} {1}", message.Length, message);
        //        byte[] auditMessageStream = Encoding.UTF8.GetBytes(message);
        //        TcpClient tcpClient = new TcpClient(address, portTls);

        //        if (tls)
        //        {
        //            //Get the installed personal certificate
        //            X509Store store = new X509Store("My");
        //            store.Open(OpenFlags.ReadOnly);
        //            X509Certificate2Collection collection = store.Certificates.Find(X509FindType.FindByThumbprint, CurrentConfigValue.CertificateThumbPrint, true);
        //            store.Close();

        //            using (SslStream sslStream = new SslStream(tcpClient.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), new LocalCertificateSelectionCallback(SelectCertificate)))
        //            {
        //                // Add a client certificate to the ssl connection
        //                sslStream.AuthenticateAsClient(address, collection, System.Security.Authentication.SslProtocols.Default, false);
        //                sslStream.Write(auditMessageStream);
        //                Logging.WriteToLog(string.Format(CultureInfo.InvariantCulture, "TLS Audit Trail at {0} to {1}:{2} -- {3}",
        //                    DateTime.Now.ToString(), address, portTls, message), TraceEventType.Information, LogCategory.General);
        //            }
        //        }
        //        else
        //        {
        //            var stream = tcpClient.GetStream();
        //            stream.Write(auditMessageStream, 0, auditMessageStream.Length);
        //            Logging.WriteToLog(string.Format(CultureInfo.InvariantCulture, "TCP Audit Trail at {0} to {1}:{2} -- {3}",
        //                    DateTime.Now.ToString(), address, portTls, message), TraceEventType.Information, LogCategory.General);
        //        }

        //        tcpClient.Close();
        //    }
        //    catch (Exception e)
        //    {
        //        Logging.WriteToLog("Unable to transfer Audit Trail Data via TLS :" + e.ToString(), TraceEventType.Warning, LogCategory.General);
        //    }
        //}

        //private static X509Certificate SelectCertificate(object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers)
        //{
        //    return localCertificates[0];
        //}

        //private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        //{
        //    //Accept all certificates for the time being
        //    //return true;

        //    // Replace this line with code to validate server certificate.
        //    if (sslPolicyErrors == SslPolicyErrors.None || sslPolicyErrors == SslPolicyErrors.RemoteCertificateNameMismatch)
        //        return true;

        //    //// If we are here it means we are failing the server node authentication.
        //    ////ProcessNodeAuthenticationFailure(EventOutcomeIndicator.SeriousFailure, MessageType.TLSAuditTrail);

        //    ////Above call won't work because TLS has already failed, so it is better to do a local log in such cases.
        //    //Logging.WriteToLog("Authentication failed for TLS Audit Trail Logging", TraceEventType.Error, LogCategory.General); 

        //    return false;
        //}

        //private static void SendViaUDP(string address, int portUdp, string message)
        //{
        //    UdpClient udpClient = new UdpClient(address, portUdp);
        //    byte[] auditMessageStream = Encoding.UTF8.GetBytes(message);
        //    int noOfBytesSent = udpClient.Client.Send(auditMessageStream);
        //    Debug.Assert(noOfBytesSent > 0);

        //    if (noOfBytesSent <= 0)
        //    {
        //        Logging.WriteToLog("UDP posting of audit message failed", System.Diagnostics.TraceEventType.Error, LogCategory.General);
        //    }
        //    else
        //        Logging.WriteToLog(string.Format(CultureInfo.InvariantCulture, "UDP Audit Trail at {0} to {1}:{2} -- {3}", 
        //            DateTime.Now.ToString(), address, portUdp, message), TraceEventType.Information, LogCategory.General);
        //}

        private static string ToStringHL7V2Format(this DateTime dateTime)
        {
            string formatString = string.Empty;

            if (dateTime.Year > 0)
            {
                formatString += "yyyy";
            }
            if (dateTime.Month > 0)
            {
                formatString += "MM";
            }
            if (dateTime.Day > 0)
            {
                formatString += "dd";
            }
            if (dateTime.Hour >= 0)
            {
                formatString += "HH";
            }
            if (dateTime.Minute >= 0)
            {
                formatString += "mm";
            }
            if (dateTime.Second >= 0)
            {
                formatString += "ss";
            }
            return dateTime.ToString(formatString);
        }

        private static string ToBase64String(this string inputString)
        {
            byte[] msgBytes = System.Text.Encoding.UTF8.GetBytes(inputString);
            return Convert.ToBase64String(msgBytes);
        }

        private static string GetXmlElementFromEventData<T>(T eventData)
            where T : XmlSectionBase
        {
            XmlDocument doc = new XmlDocument();
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stream, eventData);
                stream.Position = 0;
                doc.Load(stream);
                RemoveUnwantedAttribute(doc.DocumentElement);
                return doc.DocumentElement.OuterXml;
            }
        }

        public static T Deserialize<T>(string data)
            where T:XmlSectionBase
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            T deserializedObject ;
            TextReader reader = new StringReader(data);
           
            try
            {
                deserializedObject = (T)xs.Deserialize(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return deserializedObject;
        }

        private static void RemoveUnwantedAttribute(XmlElement element)
        {
            element.RemoveAttribute("xmlns:xsi");
            element.RemoveAttribute("xmlns:xsd");
        }

        //private static string SyslogCollectorAddress
        //{
        //    get
        //    {
        //        var config = CurrentConfigValue.GetDeserializedObject<SyslogConfig>();
        //        return config.SyslogCollectorAddress;
        //    }
        //}

        //private static int SyslogCollectorPortUdp
        //{
        //    get
        //    {
        //        var config = CurrentConfigValue.GetDeserializedObject<SyslogConfig>();
        //        return config.SyslogUDPPort;
        //    }
        //}

        //private static int SyslogCollectorPortTls
        //{
        //    get
        //    {
        //        var config = CurrentConfigValue.GetDeserializedObject<SyslogConfig>();
        //        return config.SyslogTLSPort;
        //    }
        //}

        //private static int SyslogCollectorPortTcp
        //{
        //    get
        //    {
        //        var config = CurrentConfigValue.GetDeserializedObject<SyslogConfig>();
        //        return config.SyslogTCPPort;
        //    }
        //}

        //private static int UseLog
        //{
        //    get
        //    {
        //        var config = CurrentConfigValue.GetDeserializedObject<SyslogConfig>();
        //        return config.UseLog;
        //    }
        //}

        //private static AuditMessage GetAuditMessage(MessageType messageType, int patientCount = 0, int documentCount = 0)
        //{
        //    AuditMessage message = new AuditMessage() { Actor = messageType };
        //    message.PopulateDefaultValues(patientCount, documentCount);
        //    return message;
        //}

        //private static string GetAppName(MessageType messageType)
        //{
        //    string actor = string.Empty;
        //    // Added to ensure the validation for Finite sized app name is not broken for Document Consumer
        //    switch(messageType)
        //    {
        //        case MessageType.DocConsumerRegistryStoredQuery:
        //        case MessageType.DocConsumerRetrieveDocumentSetImport:
        //            actor = "DocumentConsumer";
        //            break;
        //        default:
        //            actor = messageType.ToString();
        //            break;
        //    }

        //    return string.Format(CultureInfo.InvariantCulture, "{0}@{1}", actor, CompanyName);
        //}
    }
}
