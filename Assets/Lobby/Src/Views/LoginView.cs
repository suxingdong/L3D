/***********************************************
	FileName: LoginView.cs	    
	Creation: 2017-07-12
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GF;
using GF.UI;
namespace Lobby
{
    public class LoginView : AppView
    {
        private Button btnAccountLogin;
        private Button btnPhoneLogin;
        private bool isLogining = false;
        private const float timeout = 5;

        private void RegisterEvent()
        {
            _RegisterEvent(EventMsg.REGISTER_SUCCESS, OnRegisterSuccess);
        }

        protected override void OnStart()
        {
            btnAccountLogin = GameObject.Find("BtnAccountLogin").GetComponent<Button>();
            btnAccountLogin.onClick.AddListener(OnBtnAccountLogin);

            string account = UserDefault.Instance.GetStringForKey("Account");
            string pwd = UserDefault.Instance.GetStringForKey("Password");

            if (!string.IsNullOrEmpty(account) && !string.IsNullOrEmpty(pwd))
            {
                AccountInfo info = new AccountInfo();
                info.UID = account;
                info.PWD = pwd;
                RegisterEvent();
                btnAccountLogin.enabled = false;
                StartCoroutine(Onlogin(info));
            }
        }

        IEnumerator Onlogin(AccountInfo info)
        {
            if (!NetManager.Instance.IsConnected)
            {
                NetManager.Instance.Connect(true, "127.0.0.1", 40056);
            }
            yield return new WaitForSeconds(0.1f);
            float timeCount = 0;
            while (!NetManager.Instance.IsConnected && timeCount> timeout)
            {
                yield return new WaitForSeconds(0.1f);
                timeCount += 0.2f;
            }
            
            if (NetManager.Instance.IsConnected)
            {
                ModelManager.Instance.Get<LoginModel>().Logon(info);
            }
            else
            {
                btnAccountLogin.enabled = true;
                OnDestroy();
                UIManager.Instance.ShowMessage(ErrorCode.NET_CONNECT_FAIL.Description(), MessageBoxEnum.Style.Ok, null);
            }
        }

        public void OnBtnAccountLogin()
        {
            UIManager.Instance.ShowView<AccountLoginView>();
            //UIManager.Instance;
        }

        public void OnRegisterSuccess(IEvent iEvent)
        {
            UIManager.Instance.HideView<LoginView>();
            UIManager.Instance.HideView<AccountLoginView>();
            UIManager.Instance.HideView<RegisterView>();
            UIManager.Instance.ShowView<MainMenuView>();
        }

    }
}

