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
using Lobby;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BuYu
{
    public class GameView : AppView, IPointerDownHandler,IPointerUpHandler
    {

        private SceneModel scemModel;
        private SkillModel skillModel;
        private Button btnShowMenu;
        private Button btnSetting;
        private Button btnBack;
        private Button btnShop;
        private bool isShowMenu = false;
        private Animation animation;

        private Button btnVip;
        //private Button btnShop;
        private Button btnLockFish;
        private Button btnSkillEp;

  
        private void RegisterEvent()
        {
            _RegisterEvent(EventMsg.UPDATE_CANON_SKILL, OnUpdateSkillBtn);
            _RegisterEvent(EventMsg.HIDE_CANON_SKILL, OnDisableSkillBtn);
        }

        protected override void OnStart()
        {
            scemModel = ModelManager.Instance.Get<SceneModel>();
            skillModel = ModelManager.Instance.Get<SkillModel>();

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
            RegisterEvent();

            var tSkillNode = transform.FindChild("Skill");
            btnVip = tSkillNode.FindChild("BtnVip").GetComponent<Button>();
            btnLockFish = tSkillNode.FindChild("BtnLockFish").GetComponent<Button>();
            btnSkillEp = tSkillNode.FindChild("BtnSkillEp").GetComponent<Button>();

            btnSkillEp.onClick.AddListener(delegate ()
            {
                Debug.Log("使用技能");
                OnClickSkill();
            });
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

        public void OnOpenVip()
        {
            Debug.Log("OnOpenVip");
        }


        public void OnUpdateSkillBtn(IEvent iEvent )
        {
            byte idx = (byte)iEvent.parameter;
            btnSkillEp.gameObject.SetActive(true);
            var skilIcon = string.Format("Skill_Btn{0}", idx);
            btnSkillEp.gameObject.GetComponent<Image>().sprite = ResManager.Instance.LoadSprite("BuYu/Texture/Skill/" + skilIcon);
        }

        public void OnDisableSkillBtn(IEvent iEvent)
        {
            btnSkillEp.gameObject.SetActive(false);
        }


       

        void OnClickSkill()
        {
            skillModel.OnProcessSkill();
        }
        

    }
}

