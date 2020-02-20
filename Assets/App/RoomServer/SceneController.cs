using App.Common;
using App.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace App.RoomServer
{
    public class SceneController : MonoBehaviour
    {
        public Text text;

        private void Awake()
        {
            StartReceiveMessage();
        }

        private void Start()
        {
            CreateServer(8080);
        }

        void CreateServer(int port)
        {
            Server<MessageData> server = null;
            try
            {
                server = new Server<MessageData>(port);
            }
            catch (SocketException exception)
            {
                Debug.Log(exception.Message);
            }
            if (server != null)
            {
                Task task = new Task(() =>
                {
                    server.Start();
                });
                task.Start();
            }
        }

        void StartReceiveMessage()
        {
            MessageBroker.Default.Receive<MessageData>().ObserveOnMainThread()
                      .Subscribe(data => HandleReceivedMessage(data));
        }

        void HandleReceivedMessage(MessageData data)
        {
            text.text = data.GetCommand().ToString();
        }
    }
}
