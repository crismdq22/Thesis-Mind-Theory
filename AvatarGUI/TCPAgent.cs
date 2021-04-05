using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AvatarGUI
{
    class TCPAgent
    {
        private Socket sender;
        private bool _isConnected = false;
        public bool isConnected {
            get
            {
                return _isConnected;
            }
        }

        public void SendMessage(byte message)
        {
            byte[] msg = new byte[] {message};
            try
            {
                int bytesSent = sender.Send(msg);
            }
            catch
            {
                CatchNoConnection();
            }
            
            if (message == Constants.STOP && _isConnected)
            {
                _isConnected = false;
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
            }           
        }

        public void SendJson(string message)
        {
            try
            {
                BinaryWriter writer = new BinaryWriter(new NetworkStream(sender));
                writer.Write(message);
                writer.Close();
                sender.Receive(new byte[] { });
            }
            catch
            {
                CatchNoConnection();
            }
        }

        private void CatchNoConnection()
        {
            _isConnected = false;
            try
            {
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
            }
            catch (SocketException)
            {

            }
            MessageBox.Show("Conexion con el reproductor no establecida.");
        }

        public void ConnectServer(string iPRemote)
        {
            byte[] bytes = new byte[1024];

            try
            {
                IPAddress ipAddress = IPAddress.Parse(iPRemote);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000); 
                sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);    
                try
                {
                    sender.Connect(remoteEP);
                    Console.WriteLine("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString());
                    _isConnected = true;

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
