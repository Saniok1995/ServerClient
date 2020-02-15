using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;

namespace App.Net
{

    public class Server
    {
        Socket tcpSocket;
        public Action<string> ReceivedData;

        public Server(int port) {

            IPEndPoint tcpEndPoint = new IPEndPoint(IPAddress.Any, port);
            tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tcpSocket.Bind(tcpEndPoint);
            tcpSocket.Listen(10);
        }

        public void Start() {
            
            while (true)
            {                
                Socket listener = tcpSocket.Accept();

                byte[] buffer = new byte[256];
                int size = 0;
                StringBuilder data = new StringBuilder();
                do
                {
                    size = listener.Receive(buffer);
                    data.Append(Encoding.UTF8.GetString(buffer, 0, size));

                } while (listener.Available > 0);

                ReceivedData?.Invoke(data.ToString());

                listener.Send(Encoding.UTF8.GetBytes("ok"));

                listener.Shutdown(SocketShutdown.Both);
                listener.Close();
            }
        }
    }
}
