using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace App.Net
{
    public class Client
    {
        Socket tcpSocket;
        IPEndPoint tcpEndPoint;

        string connectIp;
        int port;

        public Client(string connectIp, int port)
        {
            this.connectIp = connectIp;
            this.port = port;
        }        

        public void SendMessage<T>(T sentData)
        {
            ConnectServer();
            
            var data = Encoding.UTF8.GetBytes(JsonUtility.ToJson(sentData));
            tcpSocket.Connect(tcpEndPoint);
            tcpSocket.Send(data);

            tcpSocket.Shutdown(SocketShutdown.Both);
            tcpSocket.Close();
        }

        public bool CheckConnect()
        {
            try
            {
                ConnectServer();
                var data = Encoding.UTF8.GetBytes(StandardMessages.TestQuery);
                tcpSocket.Connect(tcpEndPoint);
                tcpSocket.Send(data);

                byte[] buffer = new byte[256];
                int size = 0;
                StringBuilder answer = new StringBuilder();
                do
                {
                    size = tcpSocket.Receive(buffer);
                    answer.Append(Encoding.UTF8.GetString(buffer, 0, size));

                } while (tcpSocket.Available > 0);
                tcpSocket.Shutdown(SocketShutdown.Both);

                return answer.ToString() == StandardMessages.Сonfirmation;
            }
            catch (SocketException exception)
            {
                Debug.Log(exception.Message);
                return false;
            }
            finally {                
                tcpSocket.Close();
            }
        }

        void ConnectServer()
        {
            tcpEndPoint = new IPEndPoint(IPAddress.Parse(connectIp), port);
            tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
    }
}
