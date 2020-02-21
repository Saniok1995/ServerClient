using App.Common;
using App.Net;
using DG.Tweening;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace App.RoomServer
{
    public class SceneController : MonoBehaviour
    {
        [Header("Links UI")]
        [SerializeField] InputField enteredPort;
        [SerializeField] CanvasGroup parametersPanel;

        [Header("Links Game Objects")]
        [SerializeField] ParticleSystem bomb;
        [SerializeField] Light[] lights;

        string wrongPortMessage = "Wrong Port";

        public void OnClickStartServer()
        {
            int port = Convert.ToInt32(enteredPort.text);
            if (port > 1023 || port < 49151)
            {
                try
                {
                    CreateServer(port);
                    StartReceiveMessage();
                    HideControllPanel();
                }
                catch (SocketException exception)
                {
                    Debug.Log(exception.Message);
                    enteredPort.text = String.Empty;
                }
            }
            else
            {
                enteredPort.text = wrongPortMessage;
            }
        }

        void CreateServer(int port)
        {
            Server<MessageData> server = null;
            server = new Server<MessageData>(port);
            Task task = new Task(() =>
            {
                server.Start();
            });
            task.Start();
        }

        void StartReceiveMessage()
        {
            MessageBroker.Default.Receive<MessageData>().ObserveOnMainThread()
                      .Subscribe(data => HandleReceivedMessage(data));
        }

        void HandleReceivedMessage(MessageData data)
        {
            switch (data.GetCommand())
            {
                case TypeCommand.OnLight:
                    foreach (Light light in lights)
                    {
                        light.enabled = true;
                    }
                    break;
                case TypeCommand.OffLight:
                    foreach (Light light in lights)
                    {
                        light.enabled = false;
                    }
                    break;
                case TypeCommand.Boom:
                    bomb.Play();
                    break;
                default: break;
            }
        }

        Tween HideControllPanel()
        {
            return parametersPanel.DOFade(0f, 0.5f);
        }
    }
}
