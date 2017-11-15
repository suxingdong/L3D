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
        private Button btnPay;

        SceneLockedUI m_LockedFishUI = new SceneLockedUI();

        private void RegisterEvent()
        {
            _RegisterEvent(EventMsg.UPDATE_CANON_SKILL, OnUpdateSkillBtn);
            _RegisterEvent(EventMsg.HIDE_CANON_SKILL, OnDisableSkillBtn);
            _RegisterEvent(EventMsg.UPDATE_SKILLCDTIME, OnPlayCD);
            _RegisterEvent(EventMsg.UPDATE_PLAYSCORE, OnUpdataScore);
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
            btnLockFish.onClick.AddListener(OnClickLock);
            
            btnSkillEp = tSkillNode.FindChild("BtnSkillEp").GetComponent<Button>();
            btnSkillEp.onClick.AddListener(OnClickSkill);

            btnPay = tSkillNode.FindChild("BtnPay").GetComponent<Button>();
            btnPay.onClick.AddListener(OnCliclPay);
             
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
            OnSendLevel();
            UIManager.Instance.ShowView<RoomView>();
            scemModel.Shutdown();
            UIManager.Instance.HideView<GameView>();
        }

        public void OnSendLevel()
        {
            //发送玩家离开桌子的命令
            CL_Cmd_LeaveTable ncb = new CL_Cmd_LeaveTable();
            ncb.SetCmdType(NetCmdType.CMD_CL_LeaveTable);
            NetManager.Instance.Send<CL_Cmd_LeaveTable>(ncb);

            PlayerRole.Instance.RoleGameData.OnHandleRoleLeaveTable();

            //让玩家直接离开场景
           // LogicManager.Instance.Back(null);//返回大厅就Ok了
            int bg = 1;
            AudioManager.Instance.PlayerBGMusic(Audio.EffectBGType.EffectBGSound2);
        }
        public void OnOpenShop()
        {

        }

        public void OnOpenSetting()
        {
            UIManager.Instance.ShowView<MusicSettingView>();
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


        public void OnPlayCD(IEvent iEvent)
        {
            object[] obj = iEvent.parameter as object[];
            float time = (float)obj[0];
            SkillType type = (SkillType)obj[1];
            //float time, SkillType type
            if (type < SkillType.SKILL_LOCK)
                skillModel.PlayCD(time, 0);
            else
            {
                skillModel.PlayCD(time, 1);
            }
        }

        public void  OnUpdataScore(IEvent iEvent)
        {
            SceneRuntime.PlayerMgr.UpdatePlayerGold(PlayerRole.Instance.RoleInfo.RoleMe.GetUserID());
            //SceneRuntime.LogicUI.UpdateUnLockDataInfo();
        }
        void OnClickLock()
        {
            skillModel.OnProcessLock();
        }

        void OnClickSkill()
        {
            skillModel.OnProcessSkill();
        }

        void OnCliclPay()
        {
            UIManager.Instance.ShowView<ShopView>();
        }

        protected override void OnUpdate(float time)
        {
            m_LockedFishUI.UpdateLockedUI();
        }


        protected override void OnDestroy()
        {
            base.OnDestroy();
            //NetManager.Instance.Disconnect();
            ModelManager.Instance.RemoveModule<SceneModel>();
            ModelManager.Instance.RemoveModule<SkillModel>();
            
        }

    }
}

