using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using UniRx;
using App.RoomServer;
using System.Threading.Tasks;
using System.Threading;

namespace App.Net
{
    public class Server<T>
    {
        Socket tcpSocket;

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
                ThreadPool.QueueUserWorkItem((obj)=> { HandleListener(listener); });
                }
        }

        void HandleListener(Socket listener) {
            byte[] buffer = new byte[256];
            int size = 0;
            StringBuilder data = new StringBuilder();
            do
            {
                size = listener.Receive(buffer);
                data.Append(Encoding.UTF8.GetString(buffer, 0, size));

            } while (listener.Available > 0);

            HandleRequest(data.ToString(), listener);

            listener.Shutdown(SocketShutdown.Both);
            listener.Close();
        }

        void HandleRequest(string message, Socket listener) {            
            if (message.ToString() != StandardMessages.TestQuery)
            {
                var data = JsonUtility.FromJson<T>(message.ToString());            
                MessageBroker.Default.Publish(data);
            }
            else
            {
                listener.Send(Encoding.UTF8.GetBytes(StandardMessages.Сonfirmation));
            }
        }
    }

    public class StandardMessages {
        public const string TestQuery = "test";
        public const string Сonfirmation = "processed";
    }    
}
