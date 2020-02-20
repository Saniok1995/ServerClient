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
        [SerializeField] Image panelControll;
        [SerializeField] Button buttonLight;

        [Header("Loading window")]
        [SerializeField] GameObject loadingWindow;

        Client client;

        public void OnClickConnect()
        {
            client = new Client(ipConnection.text, Convert.ToInt32(port.text));
            loadingWindow.SetActive(true);
            Observable.Start(() => client.CheckConnect()).ObserveOnMainThread()
                .Subscribe(HandleConnect);
        }

        public void OnClickToggleLight()
        {
            SendMessage(new MessageData(TypeCommand.ToggleLight));
        }

        public void OnClickBoom()
        {
            SendMessage(new MessageData(TypeCommand.Boom));
        }

        void SendMessage(MessageData message)
        {
            Debug.Log(message.GetCommand());
                loadingWindow.SetActive(true);
                Observable.Start(() => client.SendMessage(message)).ObserveOnMainThread()
                    .Subscribe(HandleSend);            
        }

        void HandleSend(bool result)
        {            
            loadingWindow.SetActive(false);
            if (result == false)
            {                
                SetStatePanel(panelControll, false).
                    Append(SetStatePanel(panelParametersConnect, true));
            }
        }

        void HandleConnect(bool result)
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
    }
}
