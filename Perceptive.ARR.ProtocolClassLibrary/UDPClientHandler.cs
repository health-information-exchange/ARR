using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Perceptive.ARR.HelperLibrary;

namespace Perceptive.ARR.ProtocolClassLibrary
{
    public class UDPClientHandler : IDisposable
    {
        private int ARRPort_UDP;
        private UdpClient client;

        public UDPClientHandler(int port)
        {
            ARRPort_UDP = port;
            client = new UdpClient(ARRPort_UDP);
            client.BeginReceive(new AsyncCallback(UdpMessageReceiver), null);
            Helper.LogMessage("UDP Listener started...", Constants.LogCategoryName_Service);
        }

        private void UdpMessageReceiver(IAsyncResult res)
        {
            try
            {
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, ARRPort_UDP);
                byte[] received = client.EndReceive(res, ref RemoteIpEndPoint);
                RepositoryRequest request = new RepositoryRequest()
                {
                    Data = received,
                    Protocol = MessageProtocol.UDP,
                    IP = RemoteIpEndPoint.Address.ToString()
                };

                new Task(() => new MessageLogger().LogMessage(request)).Start();
            }
            catch (Exception ex)
            {
                Helper.LogMessage(ex.ToString(), Constants.LogCategoryName_Service);
            }
            finally
            {
                if(client != null)
                    client.BeginReceive(new AsyncCallback(UdpMessageReceiver), null);
            }
        }

        public void Dispose()
        {
            if (client != null)
            {
                try 
                { 
                    client.Close(); 
                }
                catch (Exception ex) 
                {
                    Helper.LogMessage(ex.ToString(), Constants.LogCategoryName_Service);
                }
                finally 
                { 
                    client = null;
                    Helper.LogMessage("UDP Listener stopped...", Constants.LogCategoryName_Service);
                }
            }            
        }

        public int Port
        {
            get { return ARRPort_UDP; }
        }
    }
}
