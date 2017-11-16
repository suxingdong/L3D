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
using DG.Tweening;
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

        public void OnComplete()
        {
            UIManager.Instance.HideView<RegisterView>();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            Transform background = transform.Find("BackGround").transform;
            Tweener tweener = background.DOLocalMoveX(1665, 0.4f);
            tweener.SetUpdate(true);
            tweener.SetEase(Ease.InBack);
            tweener.OnComplete(OnComplete);
        }
        private void RegisterEvent()
        {
            _RegisterEvent(EventMsg.REGISTER_SUCCESS, OnRegisterSuccess);
        }

        protected override void OnStart()
        {
            Transform parent = transform.Find("Background");
            Tweener tweener = parent.DOLocalMoveX(450, 0.3f);
            tweener.SetUpdate(true);
            tweener.SetEase(Ease.OutBack);
            btnRegister = parent.FindChild("BtnLogin").GetComponent<Button>();

            btnRegister.onClick.AddListener(delegate ()
            {
                onEnterMainGame();
            });

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
                isConnect = NetManager.Instance.Connect(true, Boot.Instance.Ip, 40056);
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

