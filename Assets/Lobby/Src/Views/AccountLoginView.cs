/***********************************************
	FileName: AccountLoginView.cs	    
	Creation: 2017-07-12
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GF;
using GF.UI;
using System;
using GF.NET;

namespace Lobby
{
    public class AccountLoginView : AppView, IPointerClickHandler
    {
        private Button btnAccountLogin;
        private Button btnRegister;
        private InputField labelUID;
        private InputField labelPWD;
        private Animation animation;
        public void OnPointerClick(PointerEventData eventData)
        {
            animation.Play("ViewOut");
            /*AnimationEvent evt = new AnimationEvent();
            evt.time = animation.GetClip("ViewOut").length;
            evt.functionName = "OnClose";
            animation.GetClip("ViewOut").AddEvent(evt);*/
            StartCoroutine(OnClose());
        }

        IEnumerator OnClose()
        {
            yield return new WaitForSeconds(0.2f);
            UIManager.Instance.HideView<AccountLoginView>();
        }
        
        protected override void OnStart()
        {
            
            btnAccountLogin = transform.FindChild("Background/BtnLogin").GetComponent<Button>();
            btnAccountLogin.onClick.AddListener(delegate () 
            {
                //UIManager.Instance.HideView<LoginView>();
                //UIManager.Instance.HideView<AccountLoginView>();
                onEnterMainGame();
            });

            btnRegister = transform.FindChild("Background/BtnRegister").GetComponent<Button>();
            btnRegister.onClick.AddListener(delegate ()
            {
                UIManager.Instance.ShowView<RegisterView>();
            });

            animation = transform.FindChild("Background").GetComponent<Animation>();
            animation.Play("ViewIn");

            labelUID = transform.FindChild("Background/UID/InputField").GetComponent<InputField>();
            labelPWD = transform.FindChild("Background/PWD/InputField").GetComponent<InputField>();
            RegisterEvent();
        }

        /*protected override void OnDestroy()
        {
            EventManager.Instance.RemoveEventListener(EventMsg.LOGON_SUCCESS, OnRegisterSuccess);
        }*/

        private void RegisterEvent()
        {
            _RegisterEvent(EventMsg.LOGON_SUCCESS, OnRegisterSuccess);
        }

        public void onEnterMainGame()
        {
            AccountInfo info = new AccountInfo();
            info.UID = labelUID.text;
            info.PWD = labelPWD.text;
            ErrorCode code = CheckUserInfo(info);
            if (code != ErrorCode.OK)
            {                
                UIManager.Instance.ShowMessage(code.Description(), MessageBoxEnum.Style.Ok, null);
                return;
            }
            bool isConnect = NetManager.Instance.IsConnected;
            if (!isConnect)
            {
                 //isConnect = NetManager.Instance.Connect(true, "127.0.0.1", 40056);
                 isConnect = NetManager.Instance.Connect(true, "192.168.77.226", 40056);
            }
                        
            if(isConnect)
            {
                //ModelManager.Instance.Get<LoginModel>().RegisterLogon(info);
                ModelManager.Instance.Get<LoginModel>().Logon(info);
                //StartCoroutine(LoginSuscess());
                //UIManager.Instance.ShowView<MainMenuView>();
            }
            else
            {
                UIManager.Instance.ShowMessage(ErrorCode.NET_CONNECT_FAIL.Description(), MessageBoxEnum.Style.Ok,null);                
            }            
        }

        public ErrorCode CheckUserInfo(AccountInfo info)
        {
            if(string.IsNullOrEmpty(info.PWD) || string.IsNullOrEmpty(info.UID))
            {
                return ErrorCode.UID_PWD_IS_NULL;
            }
            
            if(info.PWD.Length<3 ||info.PWD.Length>16 || info.UID.Length < 3 || info.UID.Length>16)
            {
                return ErrorCode.UID_PWD_LONG_WRONG;
            }
            return ErrorCode.OK;
        }

        public void OnRegisterSuccess(IEvent iEvent)
        {
            UIManager.Instance.HideView<LoginView>();
            UIManager.Instance.HideView<AccountLoginView>();
            UIManager.Instance.ShowView<MainMenuView>();
        }

        IEnumerator LoginSuscess()
        {
            while (NetManager.Instance.IsConnected)
            {
                Debug.Log("connect...");
                yield return new WaitForSeconds(0.1f);
            }
            UIManager.Instance.HideView<LoginView>();
            UIManager.Instance.HideView<AccountLoginView>();
            UIManager.Instance.ShowView<MainMenuView>();
        }
    }
}
