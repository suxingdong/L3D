/***********************************************
	FileName: RegisterView.cs	    
	Creation: 2017-09-06
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using GF;
using GF.UI;
using UnityEngine.EventSystems;

namespace Lobby
{

    public class RegisterView : AppView, IPointerClickHandler
    {

        private Button btnRegister;
        private InputField labelUID;
        private InputField labelPWD;
        private Animation animation;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            animation.Play("ViewOut");
            StartCoroutine(OnClose());
        }

        IEnumerator OnClose()
        {
            yield return new WaitForSeconds(0.2f);
            UIManager.Instance.HideView<RegisterView>();
        }

        private void RegisterEvent()
        {
            _RegisterEvent(EventMsg.REGISTER_SUCCESS, OnRegisterSuccess);
        }

        protected override void OnStart()
        {
            btnRegister = transform.FindChild("Background/BtnLogin").GetComponent<Button>();
            btnRegister.onClick.AddListener(delegate ()
            {
                onEnterMainGame();
            });
            animation = transform.FindChild("Background").GetComponent<Animation>();
            animation.Play("ViewIn");

            labelUID = transform.FindChild("Background/UID/InputField").GetComponent<InputField>();
            labelPWD = transform.FindChild("Background/PWD/InputField").GetComponent<InputField>();
            RegisterEvent();
        }

        public void OnRegisterSuccess(IEvent iEvent)
        {
            UserDefault.Instance.SetStringForKey("Account", labelUID.text);
            UserDefault.Instance.SetStringForKey("Password", labelPWD.text);
            UIManager.Instance.HideView<LoginView>();
            UIManager.Instance.HideView<AccountLoginView>();
            UIManager.Instance.HideView<RegisterView>();
            UIManager.Instance.ShowView<MainMenuView>();
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
                isConnect = NetManager.Instance.Connect(true, "192.168.0.110", 40056);
            }

            if (isConnect)
            {
                ModelManager.Instance.Get<LoginModel>().RegisterLogon(info);
                //ModelManager.Instance.Get<LoginModel>().Logon(info);
                //StartCoroutine(LoginSuscess());
                //UIManager.Instance.ShowView<MainMenuView>();
            }
            else
            {
                UIManager.Instance.ShowMessage(ErrorCode.NET_CONNECT_FAIL.Description(), MessageBoxEnum.Style.Ok, null);
            }
        }

        public ErrorCode CheckUserInfo(AccountInfo info)
        {
            if (string.IsNullOrEmpty(info.PWD) || string.IsNullOrEmpty(info.UID))
            {
                return ErrorCode.UID_PWD_IS_NULL;
            }

            if (info.PWD.Length < 3 || info.PWD.Length > 16 || info.UID.Length < 3 || info.UID.Length > 16)
            {
                return ErrorCode.UID_PWD_LONG_WRONG;
            }
            return ErrorCode.OK;
        }


    }
}

