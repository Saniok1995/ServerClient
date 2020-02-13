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

        public Client(string connectIp, int port)
        {
            tcpEndPoint = new IPEndPoint(IPAddress.Parse(connectIp), port);
            tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tcpSocket.Bind(tcpEndPoint);
            tcpSocket.Listen(10);
        }

        public void SendMessage(string message) {
            var data = Encoding.UTF8.GetBytes(message);
            tcpSocket.Connect(tcpEndPoint);
            tcpSocket.Send(data);
        }
    }
}
