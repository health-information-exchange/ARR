using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using Perceptive.ARR.HelperLibrary;

namespace Perceptive.ARR.ProtocolClassLibrary
{
    public class TCPClientHandler : IDisposable
    {
        private int ARRPort_TCP;
        private TcpListener tcpListener;
        private const int tcpInitialLength = 32;

        public TCPClientHandler(int port)
        {
            ARRPort_TCP = port;
            tcpListener = new TcpListener(IPAddress.Any, ARRPort_TCP);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TcpMessageReceiver), null);
            Helper.LogMessage("TCP Listener started...", Constants.LogCategoryName_Service);
        }

        private void TcpMessageReceiver(IAsyncResult res)
        {
            try
            {
                using (var client = tcpListener.EndAcceptTcpClient(res))
                {
                    byte[] buffer = new byte[tcpInitialLength];
                    int read = 0;
                    int chunk;
                    bool dataReceived = false;
                    using (var stream = client.GetStream())
                    {
                        while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
                        {
                            read += chunk;

                            // If we've reached the end of our buffer, check to see if there's
                            // any more information
                            if (read == buffer.Length)
                            {
                                int nextByte = stream.ReadByte();

                                // End of stream? If so, we're done
                                if (nextByte == -1)
                                {
                                    dataReceived = true;
                                    RepositoryRequest request = new RepositoryRequest()
                                    {
                                        Data = buffer,
                                        Protocol = MessageProtocol.TCP,
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
                                Protocol = MessageProtocol.TCP,
                                IP = ((IPEndPoint)(client.Client.RemoteEndPoint)).Address.ToString()
                            };
                            new Task(() => new MessageLogger().LogMessage(request)).Start();
                        }
                    }                    
                }
            }
            catch(Exception ex)
            {
                Helper.LogMessage(ex.ToString(), Constants.LogCategoryName_Service);
            }
            finally
            {
                if(tcpListener != null)
                    tcpListener.BeginAcceptTcpClient(new AsyncCallback(TcpMessageReceiver), null);
            }
        }

        public void Dispose()
        {
            if(tcpListener != null)
            {
                try
                {
                    tcpListener.Stop();
                }
                catch(Exception ex)
                {
                    Helper.LogMessage(ex.ToString(), Constants.LogCategoryName_Service);
                }
                finally
                { 
                    tcpListener = null;
                    Helper.LogMessage("TCP Listener stopped...", Constants.LogCategoryName_Service);
                }
            }
        }

        public int Port
        {
            get { return ARRPort_TCP; }
        }
    }
}
