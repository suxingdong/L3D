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
        void Start()
        {
            btnAccountLogin = GameObject.Find("BtnAccountLogin").GetComponent<Button>();
            btnAccountLogin.onClick.AddListener(OnBtnAccountLogin);
        }


        public void OnBtnAccountLogin()
        {
            UIManager.Instance.ShowView<RegisterView>();
            //UIManager.Instance;
        }
    }
}

