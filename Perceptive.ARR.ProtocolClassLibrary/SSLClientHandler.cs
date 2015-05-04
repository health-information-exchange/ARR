using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Perceptive.ARR.HelperLibrary;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Security.Authentication;
using System.Diagnostics;

namespace Perceptive.ARR.ProtocolClassLibrary
{
    public class SSLClientHandler : IDisposable
    {
        private int ARRPort_TLS;
        private TcpListener tlsListener;
        private const int tlsInitialLength = 32;
        private X509Certificate2 certificate;

        public SSLClientHandler(int port, string thumbPrint)
        {
            try
            {
                ARRPort_TLS = port;                
                X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection collection = store.Certificates.Find(X509FindType.FindByThumbprint, thumbPrint, true);
                store.Close();

                if (collection != null && collection.Count > 0)
                {
                    certificate = collection[0];
                    tlsListener = new TcpListener(IPAddress.Any, ARRPort_TLS);
                    tlsListener.Start();
                    tlsListener.BeginAcceptTcpClient(new AsyncCallback(TlsMessageReceiver), null);
                    Helper.LogMessage("TLS Listener started...", Constants.LogCategoryName_Service);
                }
            }
            catch (Exception ex)
            {
                Helper.LogMessage(ex.ToString(), Constants.LogCategoryName_Service);
            }
        }

        private void TlsMessageReceiver(IAsyncResult res)
        {
            try
            {
                using (var client = tlsListener.EndAcceptTcpClient(res))
                {
                    byte[] buffer = new byte[tlsInitialLength];
                    int read = 0;
                    int chunk;
                    bool dataReceived = false;

                    using (SslStream sslStream = new SslStream(client.GetStream()))
                    {
                        sslStream.AuthenticateAsServer(certificate, true, SslProtocols.Default, false);
                        while ((chunk = sslStream.Read(buffer, read, buffer.Length - read)) > 0)
                        {
                            read += chunk;

                            // If we've reached the end of our buffer, check to see if there's
                            // any more information
                            if (read == buffer.Length)
                            {
                                int nextByte = sslStream.ReadByte();

                                // End of stream? If so, we're done
                                if (nextByte == -1)
                                {
                                    dataReceived = true;
                                    RepositoryRequest request = new RepositoryRequest()
                                    {
                                        Data = buffer,
                                        Protocol = MessageProtocol.TLS,
                                        IP = ((IPEndPoint)(client.Client.RemoteEndPoint)).Address.ToString()
                                    };
                                    new Task(() => new MessageLogger().LogMessage(request)).Start();
                                    break;
                                }

                                // Nope. Resize the buffer, put in the byte we've just
                                // read, and continue
                                byte[] newBuffer = new byte[buffer.Length * 2];
                                Array.Copy(buffer, newBuffer, buffer.Length);
                                newBuffer[read] = (byte)nextByte;
                                buffer = newBuffer;
                                read++;
                            }
                        }

                        if (!dataReceived)
                        {
                            // Buffer is now too big. Shrink it.
                            byte[] ret = new byte[read];
                            Array.Copy(buffer, ret, read);
                            RepositoryRequest request = new RepositoryRequest()
                            {
                                Data = ret,
                                Protocol = MessageProtocol.TLS,
                                IP = ((IPEndPoint)(client.Client.RemoteEndPoint)).Address.ToString()
                            };
                            new Task(() => new MessageLogger().LogMessage(request)).Start();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.LogMessage(ex.ToString(), Constants.LogCategoryName_Service);
            }
            finally
            {
                if(tlsListener != null)
                    tlsListener.BeginAcceptTcpClient(new AsyncCallback(TlsMessageReceiver), null);
            }
        }

        public void Dispose()
        {
            if (tlsListener != null)
            {
                try 
                { 
                    tlsListener.Stop(); 
                }
                catch (Exception ex)
                {
                    Helper.LogMessage(ex.ToString(), Constants.LogCategoryName_Service);                
                }
                finally 
                { 
                    tlsListener = null;
                    Helper.LogMessage("TLS Listener stopped...", Constants.LogCategoryName_Service);
                }
            }
        }

        public int Port
        {
            get { return ARRPort_TLS; }
        }
    }
}
