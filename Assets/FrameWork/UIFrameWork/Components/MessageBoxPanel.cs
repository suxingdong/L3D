/***********************************************
	FileName: MessageBoxPanel.cs	    
	Creation: 2017-07-12
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GF
{
    public class MessageBoxPanel : AppView
    {
        public Text lblNote;
        public Button btnSubmit;
        public Button btnYes;
        public Button btnNo;

        private MessageBoxEnum.OnReceiveMessageBoxResult callback;

        protected delegate void ObjCallBack(GameObject obj);
        protected delegate void BtnCallBack(Button obj);
        protected void SetBtnClick(GameObject btnObj, ObjCallBack onClick)
        {
            Button btn = btnObj.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(delegate ()
                {
                    //SoundEvent sound = BootStrap.GetInstance<BootStrap>().menuClickSound;
                    //if (sound != null)
                    //{
                    //    sound.play();
                    //}
                    onClick(btnObj);
                });
            }
        }
        void Awake()
        {
            SetBtnClick(btnSubmit.gameObject, OnSubmitHandler);
            SetBtnClick(btnYes.gameObject, OnSubmitHandler);
            SetBtnClick(btnNo.gameObject, OnCancelHandler);
            this.ResetButtons();
        }


        private void ResetButtons()
        {
            btnSubmit.gameObject.SetActive(false);
            btnYes.gameObject.SetActive(false);
            btnNo.gameObject.SetActive(false);
        }

        public void ShowMessageBox(string context, MessageBoxEnum.Style style, MessageBoxEnum.OnReceiveMessageBoxResult callback)
        {
            this.lblNote.text = context;
            this.callback = callback;
            gameObject.SetActive(true);
            this.ResetButtons();

            if (style == MessageBoxEnum.Style.Ok)
            {
                this.btnSubmit.gameObject.SetActive(true);
            }
            else if (style == MessageBoxEnum.Style.OkAndCancel)
            {
                btnYes.gameObject.SetActive(true);
                btnNo.gameObject.SetActive(true);
            }
        }

        private void OnSubmitHandler(GameObject o)
        {
            gameObject.SetActive(false);
            if (callback != null)
            {
                callback.Invoke(MessageBoxEnum.Result.Ok);
            }
        }

        private void OnCancelHandler(GameObject o)
        {
            gameObject.SetActive(false);
            if (callback != null)
            {
                callback.Invoke(MessageBoxEnum.Result.Cancel);
            }
        }

        public void onEscCancle()
        {
            OnCancelHandler(btnNo.gameObject);
        }
    }

}
