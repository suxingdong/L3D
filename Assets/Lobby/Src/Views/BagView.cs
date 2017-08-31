/***********************************************
	FileName: BagView.cs	    
	Creation: 2017-08-31
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using GF;
using GF.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lobby
{   
    public class BagView : AppView, IPointerClickHandler
    {
        private Button tableBtn1;
        private Button tableBtn2;
        private Button tableBtn3;

        private GameObject bagPanel;
        private GameObject safeBoxPanel;
        private GameObject propPanel;

        private Animation animation;
        protected override void OnStart()
        {
            Transform parent = transform.Find("Background");
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

            animation = parent.GetComponent<Animation>();
            animation.Play("ViewIn");

            UpdataBagList();
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            animation.Play("ViewOut");
            StartCoroutine(OnClose());
        }

        IEnumerator OnClose()
        {
            yield return new WaitForSeconds(0.2f);
            UIManager.Instance.HideView<BagView>();
        }


        public void UpdataBagList()
        {
            ShowKnapsackItemInfo();
            //ShowShopGoodsListInfo();
        }


        //显示背包物品
        void ShowKnapsackItemInfo()
        {
            /*if (m_BaseWndObject == null || m_BaseWndObject.activeSelf != true)
                return;
            ClearKnapsackGird();
            int Idx = 0;
            if (PlayerRole.Instance.ItemManager.GetAllItemMap() == null)
            {
                InitEmptyKnapsack(Idx, 14);
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

                KnapsackItemInfo knapsack = new KnapsackItemInfo();
                knapsack.Init(m_ScrollView[1].m_BaseChild);
                string IconName = "";
                IconName = pItemConfig.ItemIcon;
                knapsack.SetItemInfo(map.Value.ItemID, PlayerRole.Instance.ItemManager.GetItemSum(map.Value.ItemID, false), IconName,
                    map.Value.ItemOnlyID, pItemConfig.ItemTypeID);
                m_KnapList.Add(knapsack);
            }
            m_KnapList.Sort(CompareItemByID);
            for (byte i = 0; i < m_KnapList.Count; ++i)
            {
                m_ScrollView[1].m_Grid[Idx % 7].AddChild(m_KnapList[i].m_BaseTrans);
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
            m_KnapList.Clear();*/
        }

    }


}
