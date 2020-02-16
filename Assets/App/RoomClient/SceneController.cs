using App.Common;
using App.Net;
using App.RoomServer;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Operators;
using UnityEngine;
using UnityEngine.UI;

namespace App.RoomClient
{
    public class SceneController : MonoBehaviour
    {
        [Header("UI parameters connect")]
        [SerializeField] Text ipConnection;
        [SerializeField] Text port;
        [SerializeField] Image panelParametersConnect;
        [SerializeField] Color normColor;
        [SerializeField] Color badColor;

        [Header("UI Controll Panel")]
        [SerializeField] Text inputField;
        [SerializeField] Image panelControll;

        [Header("Loading window")]
        [SerializeField] GameObject loadingWindow;

        Client client;

        private void Start()
        {

        }

        public void OnClickConnect()
        {
            client = new Client(ipConnection.text, Convert.ToInt32(port.text));
            loadingWindow.SetActive(true);
            Observable.Start(() => client.CheckConnect()).ObserveOnMainThread()
                .Subscribe(HandlerConnect);
        }

        public void OnClickSendMessage()
        {
            string message = inputField.text;
            if (message.Length > 0)
            {
                loadingWindow.SetActive(true);
                Observable.Start(() => client?.SendMessage(new MessageData(message))).ObserveOnMainThread()
                    .Subscribe(x => loadingWindow.SetActive(false));
            }
        }

        #region Animation
        Sequence SetStatePanel(Image panel, bool show)
        {
            Sequence anim = DOTween.Sequence();
            if (show) { anim.AppendCallback(() => panel.gameObject.SetActive(true)); }
            anim.Append(panel.GetComponent<CanvasGroup>()?.DOFade(show ? 1f : 0f, 0.5f));
            if (!show) { anim.AppendCallback(() => panel.gameObject.SetActive(false)); }
            return anim;
        }

        Sequence AnimBadConnection()
        {
            Sequence anim = DOTween.Sequence().Append(panelParametersConnect.DOColor(badColor, 0.5f)).
                        Append(panelParametersConnect.DOColor(normColor, 0.5f));
            return anim;
        }
        #endregion

        void HandlerConnect(bool result)
        {
            loadingWindow.SetActive(false);
            if (result)
            {
                SetStatePanel(panelParametersConnect, false).
                    Append(SetStatePanel(panelControll, true));
            }
            else
            {
                ipConnection.text = ""; port.text = "";
                AnimBadConnection();
            }
        }
    }
}
