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

        public bool SendMessage<T>(T sentData)
        {
            CreateSocket();
            Debug.Log(JsonUtility.ToJson(sentData));
            var data = Encoding.UTF8.GetBytes(JsonUtility.ToJson(sentData));

            try
            {
                tcpSocket.Connect(tcpEndPoint);
                tcpSocket.Send(data);
                tcpSocket.Shutdown(SocketShutdown.Both);
                return true;
            }
            catch (SocketException exception)
            {
                Debug.Log(exception.Message);
                return false;
            }
            finally
            {
                tcpSocket.Close();
            }
        }

        public bool CheckConnect()
        {
            try
            {
                CreateSocket();
                var data = Encoding.UTF8.GetBytes(StandardMessages.TestQuery);
                tcpSocket.Connect(tcpEndPoint);
                tcpSocket.Send(data);

                string answer = GetAnswer();
                tcpSocket.Shutdown(SocketShutdown.Both);

                return answer.ToString() == StandardMessages.Сonfirmation;
            }
            catch (SocketException exception)
            {
                Debug.Log(exception.Message);
                return false;
            }
            finally
            {
                tcpSocket.Close();
            }
        }

        string GetAnswer()
        {
            byte[] buffer = new byte[256];
            int size = 0;
            StringBuilder answer = new StringBuilder();
            do
            {
                size = tcpSocket.Receive(buffer);
                answer.Append(Encoding.UTF8.GetString(buffer, 0, size));

            } while (tcpSocket.Available > 0);
            return answer.ToString();
        }

        void CreateSocket()
        {
            tcpEndPoint = new IPEndPoint(IPAddress.Parse(connectIp), port);
            tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
    }
}
