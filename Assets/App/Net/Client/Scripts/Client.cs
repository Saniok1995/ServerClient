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

        public void SendMessage(string message)
        {
            ConnectServer();

            var data = Encoding.UTF8.GetBytes(message);
            tcpSocket.Connect(tcpEndPoint);
            tcpSocket.Send(data);

            tcpSocket.Shutdown(SocketShutdown.Both);
            tcpSocket.Close();
        }

        void ConnectServer()
        {
            tcpEndPoint = new IPEndPoint(IPAddress.Parse(connectIp), port);
            tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
    }
}
