using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AuditRecordSender
{
    public partial class AuditMessageSender : Form
    {
        public AuditMessageSender()
        {
            InitializeComponent();
        }

        private void AuditMessageSender_Load(object sender, EventArgs e)
        {
            cmbMode.Items.AddRange(new string[]{ "UDP", "TCP", "TLS"});
            cmbMode.SelectedIndex = 0;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (cmbMode.SelectedIndex != 0)
            {
                int count = 1;
                int.TryParse(txtCount.Text.Trim(), out count);
                count = count > 0 ? count : 1;

                for (int i = 0; i < count; i++)
                    SendViaTCP(txtIP.Text, int.Parse(txtPort.Text), txtMessage.Text, cmbMode.SelectedIndex == 2);

                txtMessage.Text += Environment.NewLine + "TCP / TLS posting of audit message completed";
            }
            else
                SendViaUDP(txtIP.Text, int.Parse(txtPort.Text), txtMessage.Text);
        }

        private void SendViaUDP(string address, int portUdp, string message)
        {
            int count = 1;
            int.TryParse(txtCount.Text.Trim(), out count);
            count = count > 0 ? count : 1;
            
            UdpClient udpClient = new UdpClient(address, portUdp);
            byte[] auditMessageStream = Encoding.UTF8.GetBytes(message);
            
            for(int i = 1; i <= count; i++)
                udpClient.Client.Send(auditMessageStream);

            txtMessage.Text += Environment.NewLine + "UDP posting of audit message completed";
        }

        private static X509Certificate SelectCertificate(object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers)
        {
            return localCertificates[0];
        }

        private void SendViaTCP(string address, int portTls, string message, bool tls)
        {
            try
            {
                // Convert to TLS specific message format
                //SYSLOG-FRAME = MSG-LEN SP SYSLOG-MSG
                //The message length (MSG-LEN) is the octet count of the SYSLOG-MSG in the SYSLOG-FRAME

                message = string.Format(CultureInfo.InvariantCulture, "{0} {1}", message.Length, message);
                byte[] auditMessageStream = Encoding.UTF8.GetBytes(message);
                TcpClient tcpClient = new TcpClient(address, portTls);

                if (tls)
                {
                    //Get the installed personal certificate
                    X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                    store.Open(OpenFlags.ReadOnly);
                    X509Certificate2Collection collection = store.Certificates.Find(X509FindType.FindByThumbprint, ConfigurationManager.AppSettings["thumbprint"], true);
                    store.Close();

                    using (SslStream sslStream = new SslStream(tcpClient.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), new LocalCertificateSelectionCallback(SelectCertificate)))
                    {
                        // Add a client certificate to the ssl connection
                        sslStream.AuthenticateAsClient(address, collection, System.Security.Authentication.SslProtocols.Default, false);                        
                        sslStream.Write(auditMessageStream);
                    }
                }
                else
                {
                    var stream = tcpClient.GetStream();
                    stream.Write(auditMessageStream, 0, auditMessageStream.Length);
                }

                tcpClient.Close();
            }
            catch (Exception e)
            {
                txtMessage.Text += Environment.NewLine + e.ToString();
            }
        }

        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            //Accept all certificates for the time being
            //return true;

            // Replace this line with code to validate server certificate.
            if (sslPolicyErrors == SslPolicyErrors.None || sslPolicyErrors == SslPolicyErrors.RemoteCertificateNameMismatch)
                return true; 

            return false;
        }
    }
}
