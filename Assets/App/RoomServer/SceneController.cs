using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Net;
using UnityEngine.UI;
using System.Threading;
using System.Threading.Tasks;

namespace App.RoomServer
{
    public class SceneController : MonoBehaviour
    {
        public Text text;
        public string bufferText;

        private void Start()
        {
            Server server = new Server(8080);
            server.ReceivedData = listenServer;
            Task task = new Task(() =>
            {
                server.Start();
            });
            task.Start();            
        }

        void listenServer(string message) {
            bufferText = message;            
        }

        private void Update()
        {
            text.text = bufferText;
        }
    }
}
