using App.Net;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using App.Common;

namespace App.RoomServer
{
    public class SceneController : MonoBehaviour
    {
        public Text text;

        private void Start()
        {
            var server = new Server<MessageData>(8080);
            Task task = new Task(() =>
            {
                server.Start();
            });
            task.Start();

            MessageBroker.Default.Receive<MessageData>().ObserveOnMainThread()
                .Subscribe(data => HandleReceivedMessage(data));
        }

        void HandleReceivedMessage(MessageData data)
        {
            text.text = data.message;
        }       
    }    
}
