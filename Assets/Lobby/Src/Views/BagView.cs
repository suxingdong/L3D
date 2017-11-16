/***********************************************
	FileName: BagView.cs	    
	Creation: 2017-08-31
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GF;
using GF.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lobby
{

    public enum Shop_Type
    {
        Shop_Property,       //道具
        Shop_Launcher,       //大炮
        Shop_Material,      //实物
        Shop_Submarine,     //潜艇
        Shop_Knapsack,      //背包
    }

    public class KnapsackShopWnd 
    {
        public GameObject m_BaseWndObject;
        public Transform m_BaseTrans;
        
        GameObject templateGoodItem;
        GridLayoutGroup gridGoods;
        GameObject templateGanonItem;
        GridLayoutGroup gridCanon;

        List<KnapsackItem> m_KnapList = new List<KnapsackItem>();
        // List<ShopItemInfoUI>    m_ShopItemInfoList = new List<ShopItemInfoUI>();


        public void Init(GameObject go)
        {
            m_BaseWndObject = go;
            m_BaseTrans = go.transform;
            templateGoodItem = m_BaseTrans.FindChild("ScrollDown/CellItem").gameObject;
            gridGoods = m_BaseTrans.FindChild("ScrollDown/Grid").GetComponent<GridLayoutGroup>();

            templateGanonItem = m_BaseTrans.FindChild("ScrollUp/CellItem").gameObject;
            gridCanon = m_BaseTrans.FindChild("ScrollUp/Grid").GetComponent<GridLayoutGroup>();
            templateGanonItem.SetActive(false);
            //m_ScrollView[0].m_BaseChild = m_BaseTrans.GetChild(0).GetChild(0).gameObject;
            //m_ScrollView[0].m_Grid = new UIGrid[1];
            //m_ScrollView[0].m_Grid[0] = m_BaseTrans.GetChild(0).GetChild(1).GetComponent<UIGrid>();

            //m_ScrollView[1].m_Grid = new UIGrid[7];
            //m_ScrollView[1].m_BaseChild = m_BaseTrans.GetChild(1).GetChild(0).gameObject;
            //for (byte i = 0; i < 7; ++i)
            //{
            //    m_ScrollView[1].m_Grid[i] = m_BaseTrans.GetChild(1).GetChild(1 + i).GetComponent<UIGrid>();
            //}
        }
        //背包中物品发生变化 更新事件触发
        public void UpdateKnapsackDate()
        {
            ShowKnapsackItemInfo();
            //ShowShopGoodsListInfo();
        }
        public void UpdateKnapsackLauncherDate()
        {
            ShowShopGoodsListInfo();
        }
        void ShowShopGoodsListInfo()
        {
            ClearShopGoodsGird();
        }
        //显示背包物品
        void ShowKnapsackItemInfo()
        {
            if (m_BaseWndObject == null || m_BaseWndObject.activeSelf != true)
                return;
            ClearKnapsackGird();
            int Idx = 0;
            if (PlayerRole.Instance.ItemManager.GetAllItemMap() == null)
            {
                //InitEmptyKnapsack(Idx, 14);
                return;
            }
            m_KnapList.Clear();
            foreach (KeyValuePair<uint, tagItemInfo> map in PlayerRole.Instance.ItemManager.GetAllItemMap())
            {
                if (FishConfig.Instance.m_ItemInfo.m_ItemMap.ContainsKey(map.Value.ItemID) == false)
                    continue;

                tagItemConfig pItemConfig = FishConfig.Instance.m_ItemInfo.m_ItemMap[map.Value.ItemID];
                if (pItemConfig.ItemTypeID == EItemType.IT_Cannon)
                    continue;

                KnapsackItem knapsack = new KnapsackItem();
                knapsack.Init(templateGoodItem);
                string IconName = "";
                IconName = pItemConfig.ItemIcon;
                knapsack.SetItemInfo(map.Value.ItemID, PlayerRole.Instance.ItemManager.GetItemSum(map.Value.ItemID, false), IconName,
                    map.Value.ItemOnlyID, pItemConfig.ItemTypeID);
                m_KnapList.Add(knapsack);
            }
            templateGoodItem.SetActive(false);
            //m_KnapList.Sort(CompareItemByID);
            RectTransform rectTransform = gridGoods.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, m_KnapList.Count*300/3);
            for (byte i = 0; i < m_KnapList.Count; ++i)
            {
                m_KnapList[i].m_BaseTrans.parent = gridGoods.transform;
                m_KnapList[i].ResetLoaclScale();
                ++Idx;
            }
            if (Idx < 14)
                InitEmptyKnapsack(Idx, 14);
            else
            {
                if ((Idx % 7) != 0)
                    InitEmptyKnapsack(Idx, (Idx / 7 + 1) * 7);
            }
            m_KnapList.Clear();
        }

        static int CompareItemByID(KnapsackItem item1, KnapsackItem item2)
        {
            if (item1.ItemID < item2.ItemID)
                return -1;
            else if (item1.ItemID > item2.ItemID)
                return 1;
            else
                return 0;
        }

        void InitEmptyKnapsack(int Idx, int MaxIdx)
        {
            for (int i = Idx; i < MaxIdx; ++i)
            {
                KnapsackItem knapsack = new KnapsackItem();
                knapsack.Init(templateGoodItem);
                knapsack.m_BaseTrans.parent = gridGoods.transform;
                knapsack.ResetLoaclScale();
            }
        }

        void ClearShopGoodsGird()
        {
            Utility.DetoryChilds(gridGoods.transform);
        }

        void ClearKnapsackGird()
        {
            /*for (byte i = 0; i < 7; ++i)
            {
                List<Transform> gridChid = m_ScrollView[1].m_Grid[i].GetChildList();

                foreach (Transform tr in gridChid)
                {
                    GameObject.Destroy(tr.gameObject);
                }
                m_ScrollView[1].m_Grid[i].transform.DetachChildren();
            }*/
        }
    }


    public class BagView : AppView, IPointerClickHandler
    {
        private Button tableBtn1;
        private Button tableBtn2;
        private Button tableBtn3;

        private GameObject bagPanel;
        private GameObject safeBoxPanel;
        private GameObject propPanel;

        private Animation animation;
        Shop_Type m_ShopType;

        


        KnapsackShopWnd m_KnapsackShopWnd = new KnapsackShopWnd();    //背包商店

     

        private void RegisterEvent()
        {
            _RegisterEvent(EventMsg.UPDATE_USERITEM, ChangeShopWndUI);
        }

        protected override void OnStart()
        {
            RegisterEvent();
            Transform parent = transform.Find("Background");
            Tweener tweener = parent.DOLocalMoveX(450, 0.3f);
            tweener.SetUpdate(true);
            tweener.SetEase(Ease.OutBack);

            tableBtn1 = parent.Find("TabeleBtn1").GetComponent<Button>();
            tableBtn2 = parent.Find("TabeleBtn2").GetComponent<Button>();
            tableBtn3 = parent.Find("TabeleBtn3").GetComponent<Button>();

            bagPanel = parent.Find("BagPanel").gameObject;
            propPanel = parent.Find("PropPanel").gameObject;
            safeBoxPanel = parent.Find("SafeBoxPanel").gameObject;

            tableBtn1.onClick.AddListener(delegate ()
            {
                bagPanel.SetActive(true);
                propPanel.SetActive(false);
                safeBoxPanel.SetActive(false);
            });

            tableBtn2.onClick.AddListener(delegate ()
            {
                bagPanel.SetActive(false);
                propPanel.SetActive(true);
                safeBoxPanel.SetActive(false);
            });

            tableBtn3.onClick.AddListener(delegate ()
            {
                bagPanel.SetActive(false);
                propPanel.SetActive(false);
                safeBoxPanel.SetActive(true);
            });

            m_KnapsackShopWnd.Init(bagPanel);
            SetUIStatue(Shop_Type.Shop_Knapsack);
            ChangeShopWndUI(null);
        }


        void SetUIStatue(Shop_Type tType)
        {
            m_ShopType = tType;
            ChangeShopWndUI(null);
        }

        public void OnComplete()
        {
            UIManager.Instance.HideView<BagView>();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            Transform background = transform.Find("Background").transform;
            Tweener tweener = background.DOLocalMoveX(1665, 0.4f);
            tweener.SetUpdate(true);
            tweener.SetEase(Ease.InBack);
            tweener.OnComplete(OnComplete);
        }
        
        public void InitSaveBox()
        {
            Button btn = safeBoxPanel.transform.FindChild("BtnWithdrawMoney").GetComponent<Button>();
            btn.onClick.AddListener(delegate ()
            {
                InputField inputField = GetComponent<InputField>();
                string str = inputField.text;
                CL_Cmd_RoleRankBox ncb = new CL_Cmd_RoleRankBox();
                ncb.SetCmdType(NetCmdType.CMD_CL_CHANG_ROLERANKBOX);
                ncb.dwUserID = PlayerRole.Instance.RoleInfo.RoleMe.GetUserID();
                ncb.accout = 1;
                ncb.bSaveType = false;
                NetManager.Instance.Send<CL_Cmd_RoleRankBox>(ncb);
            });

            btn = safeBoxPanel.transform.FindChild("BtnWithdrawMoney").GetComponent<Button>();
            btn.onClick.AddListener(delegate ()
            {

            });
        }

        public void UpdataBagList()
        {
            //ShowKnapsackItemInfo();
            //ShowShopGoodsListInfo();
        }

        void ChangeShopWndUI(IEvent iEvent)
        {
            switch (m_ShopType)
            {
                case Shop_Type.Shop_Property:
                    //m_PropertyShopWnd.UpdatePropertyDate();
                    break;
                case Shop_Type.Shop_Launcher:
                    //m_DaPaoShopWnd.UpdateDaPaoDate();
                    break;
                case Shop_Type.Shop_Material:
                    //m_MaterailShopWnd.UpateMaterailDate();
                    break;
                case Shop_Type.Shop_Submarine:
                    break;
                case Shop_Type.Shop_Knapsack:
                    m_KnapsackShopWnd.UpdateKnapsackDate();
                    break;
            }
        }


    }


}
