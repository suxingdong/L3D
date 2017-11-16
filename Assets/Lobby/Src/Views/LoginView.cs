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
            _RegisterEvent(EventMsg.LOGON_SUCCESS, OnLogonSuccess);
            
        }

        protected override void OnAnimationComplete(string name)
        {
            if (name.Equals("view_in"))
            {
                string account = UserDefault.Instance.GetStringForKey("Account");
                string pwd = UserDefault.Instance.GetStringForKey("Password");
                if (!string.IsNullOrEmpty(account) && !string.IsNullOrEmpty(pwd))
                {
                    AccountInfo info = new AccountInfo();
                    info.UID = account;
                    info.PWD = pwd;
                    btnAccountLogin.enabled = false;
                    StartCoroutine(Onlogin(info));
                }
            }
            else if (name.Equals("view_out"))
            {
                UIManager.Instance.HideView<LoginView>();
                UIManager.Instance.HideView<AccountLoginView>();
                UIManager.Instance.HideView<RegisterView>();
                UIManager.Instance.ShowView<MainMenuView>();
            }
        }

        protected override void OnStart()
        {
            btnAccountLogin = GameObject.Find("BtnAccountLogin").GetComponent<Button>();
            btnAccountLogin.onClick.AddListener(OnBtnAccountLogin);
            RegisterEvent();

        }

        IEnumerator Onlogin(AccountInfo info)
        {
            yield return new WaitForSeconds(0.5f);
            if (!NetManager.Instance.IsConnected)
            {
                NetManager.Instance.Connect(true, Boot.Instance.Ip, 40056);
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

        public void OnLogonSuccess(IEvent iEvent)
        {
            Animation anim = gameObject.GetComponent<Animation>();
            if (anim)
            {
                anim.Play("LogoOut");
            }
        }

        public void OnRegisterSuccess(IEvent iEvent)
        {
            Animation anim = gameObject.GetComponent<Animation>();
            if (anim)
            {
                anim.Play("view_out");
            }
        }

    }
}

