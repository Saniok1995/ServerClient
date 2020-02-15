using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Net;
using UnityEngine.UI;

namespace App.RoomClient
{
    public class SceneController : MonoBehaviour
    {
        [Header("Links")]
        [SerializeField] Text inputField;

        Client connectServer;


        private void Start()
        {
            connectServer = new Client("127.0.0.1", 8080);
        }

        public void OnClickSendMessage() {
            string message = inputField.text;
            if (message.Length > 0) {
                connectServer.SendMessage(message);
            }
        }
    }
}
