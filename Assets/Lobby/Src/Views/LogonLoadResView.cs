/***********************************************
	FileName: LogonLoadResView.cs	    
	Creation: 2017-08-21
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GF;
namespace Lobby
{
    public class LogonLoadResView : AppView
    {
        public Image ProgressBar;
        private float speed = 0.2f;
        private LoginModel loginModel;
        void Start()
        {
            ProgressBar.fillAmount = 0;
            loginModel = ModelManager.Instance.Get<LoginModel>();
            StartCoroutine(LoadRes());
        }

        IEnumerator LoadRes()
        {
            yield return new WaitForEndOfFrame();
        }
        void Update()
        {

        }
    }

}

