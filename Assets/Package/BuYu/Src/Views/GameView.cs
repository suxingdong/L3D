/***********************************************
	FileName: GameView.cs	    
	Creation: 2017-08-08
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GF;
using GF.UI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BuYu
{
    public class GameView : AppView, IPointerDownHandler,IPointerUpHandler
    {

        private SceneModel scemModel;
        private Button btnShowMenu;
        private Button btnSetting;
        private Button btnBack;
        private Button btnShop;
        private bool isShowMenu = false;
        private Animation animation;
        void Start()
        {
            scemModel = ModelManager.Instance.Get<SceneModel>();
            btnShowMenu = transform.FindChild("BtnMenu").GetComponent<Button>();
            btnSetting = btnShowMenu.transform.FindChild("BtnSetting").GetComponent<Button>();
            btnBack = btnShowMenu.transform.FindChild("BtnBack").GetComponent<Button>();
            btnShop = btnShowMenu.transform.FindChild("BtnShop").GetComponent<Button>();
            
            animation = btnShowMenu.GetComponent<Animation>();
            btnShowMenu.onClick.AddListener(OnShowMenu);
            btnSetting.onClick.AddListener(OnOpenSetting);
            btnBack.onClick.AddListener(OnBackRoom);
            btnShop.onClick.AddListener(OnOpenShop);

            btnSetting.gameObject.SetActive(false);
            btnBack.gameObject.SetActive(false);
            btnShop.gameObject.SetActive(false);
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            scemModel.PlayerMgr.GetPlayer(0).Launcher.IsPress = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            scemModel.PlayerMgr.GetPlayer(0).Launcher.IsPress = false;
        }

        public void OnShowMenu()
        {
            if (isShowMenu)
            {
                animation.Play("CloseMenu");
            }
            else
            {
                animation.Play("ShowMenu");
            }
            isShowMenu = !isShowMenu;
        }

        public void OnBackRoom()
        {
            UIManager.Instance.ShowView<RoomView>();
            scemModel.Shutdown();
            UIManager.Instance.HideView<GameView>();
        }

        public void OnOpenShop()
        {

        }

        public void OnOpenSetting()
        {

        }
    }
}

