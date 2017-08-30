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
    public class ChangeCanonView : AppView, IPointerClickHandler
    {
        [SerializeField]
        private GridLayoutGroup grid;
        private GameObject itemCanon;
        private SceneModel sceneModel;
        public void OnPointerClick(PointerEventData eventData)
        {
            UIManager.Instance.HideView<ChangeCanonView>();
        }

        void Start()
        {
            InitLauncherList();
        }


        void InitLauncherList()
        {
            sceneModel = ModelManager.Instance.Get<SceneModel>();
            itemCanon = grid.transform.Find("CellBtnCanon").gameObject;
            RectTransform rectTransform = grid.GetComponent<RectTransform>();
            for (byte i = 0; i < (byte)LauncherType.LAUNCHER_MAX; ++i)
            {
                uint ItemID = LauncherSetting.LauncherDataList[i].nItemid;

                if (ItemID != 0 && FishConfig.Instance.m_ItemInfo.m_ItemMap.ContainsKey(ItemID) == false)
                    continue;
                tagItemConfig pItem = FishConfig.Instance.m_ItemInfo.m_ItemMap[ItemID];

                var item = GameObject.Instantiate(itemCanon);
                item.transform.parent = grid.transform;
                Button btn = item.GetComponent<Button>();
                byte type = (byte) i;
                btn.onClick.AddListener(delegate ()
                {
                    sceneModel.ChangeDestLauncher(type);
                });
                Image icon = item.transform.FindChild("Icon").GetComponent<Image>();
                icon.sprite = ResManager.Instance.LoadSprite("BuYu/Texture/Gun/Icon/" + pItem.ItemIcon);

                Text title = item.transform.FindChild("Title").GetComponent<Text>();
                title.text = pItem.ItemName;

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


                /*LauncherItem item = new LauncherItem();
                item.Init(m_BaseChild);
                if (PlayerRole.Instance.RoleLauncher.IsCanUseLauncher(i))
                {
                    if (i == SceneRuntime.PlayerMgr.MySelf.Launcher.LauncherType)
                        item.ShowLaunchInfo(LaunchState.AlreadyEquip, pItem.ItemIcon, i);
                    else
                        item.ShowLaunchInfo(LaunchState.AlreadyGet, pItem.ItemIcon, i);
                }
                else
                    item.ShowLaunchInfo(LaunchState.WithOutGet, pItem.ItemIcon, i);

                m_UIGrid.AddChild(item.m_BaseTrans);
                item.ResetScale();*/

            }
            Destroy(itemCanon);
            var count = grid.transform.childCount - 1;
            rectTransform.sizeDelta = new Vector2(count * 360, 400);
        }

    }

}

