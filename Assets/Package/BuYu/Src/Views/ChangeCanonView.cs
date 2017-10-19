/***********************************************
	FileName: ChangeCanonView.cs	    
	Creation: 2017-08-29
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

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
        private GridLayoutGroup _mGrid;
        [SerializeField]
        private GameObject _mCanonTemplet;
        public void OnPointerClick(PointerEventData eventData)
        {
            UIManager.Instance.HideView<ChangeCanonView>();
        }
        

        private void RegisterEvent()
        {
            _RegisterEvent(EventMsg.UPDATE_CANON_SKILL, OnUpdateCanonSkill);
        }

        protected override void OnStart()
        {
            RegisterEvent();
            InitLauncherList();
        }


        void InitLauncherList()
        {
            RectTransform rectTransform = _mGrid.GetComponent<RectTransform>();
            for (int i = 0; i<_mGrid.transform.childCount;i++ )
            {
                GameObject go = _mGrid.transform.GetChild(i).gameObject;
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
                item.Init(_mCanonTemplet);
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
                item.m_BaseTrans.parent = _mGrid.transform;
                item.ResetScale();
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
            _mCanonTemplet.SetActive(false);
            rectTransform.sizeDelta = new Vector2((int)LauncherType.LAUNCHER_MAX * 360, 400);
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
        Image _mImageIcon;
        Text _mTextTitle;
        Text _mTextState;
        GameObject _mGoLocked;
        Button _mUiButton;
        CanonState _mState;
        byte _mLaunchType;
        private SceneModel sceneModel;

        public void Init(GameObject go)
        {
            sceneModel = ModelManager.Instance.Get<SceneModel>();
            m_BaseWndObject = Object.Instantiate(go);
            m_BaseTrans = m_BaseWndObject.transform;
            m_BaseWndObject.SetActive(true);
            _mImageIcon = m_BaseTrans.FindChild("Icon").GetComponent<Image>();
            _mTextTitle = m_BaseTrans.FindChild("Title").GetComponent<Text>();
            _mTextState = m_BaseTrans.FindChild("State").GetComponent<Text>();
            _mGoLocked = m_BaseTrans.FindChild("LockFlag").gameObject;
            _mUiButton = m_BaseTrans.GetComponent<Button>();
            _mUiButton.onClick.AddListener(delegate ()
            {
                if (_mState == CanonState.WithOutGet)
                {
                     //TODO 显示VIP充值
                }
                else if (_mState == CanonState.NoEquiped)
                {
                    SceneRuntime.SceneModel.ChangeDestLauncher(_mLaunchType);
                }
                sceneModel.ChangeDestLauncher(_mLaunchType);
            });

        }
        public void ResetScale()
        {
            m_BaseTrans.localScale = Vector3.one;
        }

        public void ShowLaunchInfo(CanonState tState, tagItemConfig itemConfig, byte tType)
        {
            _mState = tState;
            _mLaunchType = tType;
            _mImageIcon.sprite = ResManager.Instance.LoadSprite("BuYu/Texture/Gun/Icon/" + itemConfig.ItemIcon);
            _mTextTitle.text = itemConfig.ItemName;
            if (_mState == CanonState.Equiped)
            {
                _mUiButton.enabled = false;
                _mTextState.text = "已装备";
                _mGoLocked.SetActive(false);
            }
            else if (_mState == CanonState.NoEquiped)
            {
                _mUiButton.enabled = true;
                _mTextState.text = "装备";
                _mGoLocked.SetActive(false);
            }
            else
            {
                _mUiButton.enabled = true;
                _mTextState.text = "装备";
                _mGoLocked.SetActive(true);
            }
        }
      
    }

}

