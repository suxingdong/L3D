/***********************************************
	FileName: ChangeCanonView.cs	    
	Creation: 2017-08-29
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using GF;
using UnityEngine;
using GF.UI;
using Lobby;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BuYu
{
    public enum CanonState
    {
        Equiped = 0,       //已装备
        NoEquiped,         //已拥有
        WithOutGet,        //未获得
    }


    public class ChangeCanonView : AppView, IPointerClickHandler
    {
        [SerializeField]
        private GridLayoutGroup grid;
        [SerializeField]
        private GameObject canonTemplet;
        private SceneModel sceneModel;
        public void OnPointerClick(PointerEventData eventData)
        {
            UIManager.Instance.HideView<ChangeCanonView>();
        }

        protected override void OnDestroy()
        {
            EventManager.Instance.RemoveEventListener(EventMsg.UPDATE_CANON_SKILL, OnUpdateCanonSkill);
        }

        private void RegisterEvent()
        {
            EventManager.Instance.AddEventListener(EventMsg.UPDATE_CANON_SKILL, OnUpdateCanonSkill);
        }

        protected override void OnStart()
        {
            RegisterEvent();
            sceneModel = ModelManager.Instance.Get<SceneModel>();
            InitLauncherList();
        }


        void InitLauncherList()
        {
            
            RectTransform rectTransform = grid.GetComponent<RectTransform>();
            for (int i = 0; i<grid.transform.childCount;i++ )
            {
                GameObject go = grid.transform.GetChild(i).gameObject;
                Destroy(go);
            }

            for (byte i = 0; i < (byte)LauncherType.LAUNCHER_MAX; ++i)
            {
                uint ItemID = LauncherSetting.LauncherDataList[i].nItemid;

                if (ItemID != 0 && FishConfig.Instance.m_ItemInfo.m_ItemMap.ContainsKey(ItemID) == false)
                    continue;
                tagItemConfig pItem = FishConfig.Instance.m_ItemInfo.m_ItemMap[ItemID];

               // var item = GameObject.Instantiate(canonTemplet);

                CanonItem item = new CanonItem();
                item.Init(canonTemplet);
                byte tType = (byte)i;
                if (PlayerRole.Instance.RoleLauncher.IsCanUseLauncher(i))
                {
                    if (i == SceneRuntime.PlayerMgr.MySelf.Launcher.LauncherType)
                        item.ShowLaunchInfo(CanonState.Equiped, pItem, tType);
                    else
                        item.ShowLaunchInfo(CanonState.NoEquiped, pItem, tType);
                }
                else
                    item.ShowLaunchInfo(CanonState.WithOutGet, pItem,tType);
                item.m_BaseTrans.parent = grid.transform;

                //////////////////////

                //uint validTime = 0;
                //永久有效的
                //if (PlayerRole.Instance.RoleLauncher.IsCanUseLauncherByAllTime(i))
                //    validTime = 0;
                ////限时有效
                //else
                //{
                //    if (!PlayerRole.Instance.RoleLauncher.GetLauncherEndTime(i, out validTime))
                //        continue;
                //}
                

            }
            canonTemplet.SetActive(false);
            var count = grid.transform.childCount - 1;
            rectTransform.sizeDelta = new Vector2(count * 360, 400);
        }

        public void OnUpdateCanonSkill(IEvent iEvent)
        {
            Debug.Log("OnUpdateCanonSkill");
            InitLauncherList();
        }


    }


    public class CanonItem
    {
        public GameObject m_BaseWndObject;
        public Transform m_BaseTrans;
        Image icon;
        Text title;
        Text state;
        GameObject m_Locked;
        Button m_UIButton;
        Image m_ButtonBg;
        CanonState m_State;
        byte m_LaunchType;
        private SceneModel sceneModel;

        public void Init(GameObject go)
        {
            sceneModel = ModelManager.Instance.Get<SceneModel>();
            m_BaseWndObject = GameObject.Instantiate(go) as GameObject;
            m_BaseTrans = m_BaseWndObject.transform;
            m_BaseWndObject.SetActive(true);
            icon = m_BaseTrans.FindChild("Icon").GetComponent<Image>();
            title = m_BaseTrans.FindChild("Title").GetComponent<Text>();
            state = m_BaseTrans.FindChild("State").GetComponent<Text>();
            m_UIButton = m_BaseTrans.GetComponent<Button>();
            m_UIButton.onClick.AddListener(delegate ()
            {
                if (m_State == CanonState.WithOutGet)
                {
                     //TODO 显示VIP充值
                }
                else if (m_State == CanonState.NoEquiped)
                {
                    SceneRuntime.SceneLogic.ChangeDestLauncher(m_LaunchType);
                }
                sceneModel.ChangeDestLauncher(m_LaunchType);
            });
            //icon.sprite = ResManager.Instance.LoadSprite("BuYu/Texture/Gun/Icon/" + pItem.ItemIcon);

        }
        public void ResetScale()
        {
            m_BaseTrans.localScale = Vector3.one;
        }

        public void ShowLaunchInfo(CanonState tState, tagItemConfig itemConfig, byte tType)
        {
            m_State = tState;
            m_LaunchType = tType;
            icon.sprite = ResManager.Instance.LoadSprite("BuYu/Texture/Gun/Icon/" + itemConfig.ItemIcon);
            title.text = itemConfig.ItemName;
            if (m_State == CanonState.Equiped)
            {
                m_UIButton.enabled = false;
                state.text = "已装备";
                //m_Locked.SetActive(false);
            }
            else if (m_State == CanonState.NoEquiped)
            {
                m_UIButton.enabled = true;
                state.text = "装备";
                //m_Locked.SetActive(false);
            }
            else
            {
                m_UIButton.enabled = true;
                state.text = "装备";
                //m_Locked.SetActive(false);
            }
        }
      
    }

}

