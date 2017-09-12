/***********************************************
	FileName: ShopView.cs	    
	Creation: 2017-09-05
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
    public class ShopView : AppView, IPointerClickHandler
    {
        private Animation animation;

        List<ShopItem> m_PayInfoList = new List<ShopItem>();

        private GameObject diamondPanel;
        private GameObject goldPanel;
        private GameObject itemPanel;

        private Button btnDiamond;
        private Button btnGold;
        private Button btnItem;

        private GridLayoutGroup gridDiamond;
        private GridLayoutGroup gridGold;
        private GridLayoutGroup gridItem;

        private GameObject diamondTemp;
        private GameObject goldTemp;
        private GameObject itemTemp;

        public void OnPointerClick(PointerEventData eventData)
        {
            animation.Play("ViewOut");
            StartCoroutine(OnClose());
        }

        IEnumerator OnClose()
        {
            yield return new WaitForSeconds(0.2f);
            UIManager.Instance.HideView<ShopView>();
        }

        protected override void OnStart()
        {
            Transform background = transform.Find("BackGround").transform;

            animation = background.GetComponent<Animation>();
            animation.Play("ViewIn");

            diamondPanel = background.Find("DiamondPanel").gameObject;
            goldPanel = background.Find("GoldPanel").gameObject;
            itemPanel = background.Find("ItemPanel").gameObject;

            btnDiamond = GameObject.Find("BackGround/BtnDiamond").GetComponent<Button>();
            btnGold = GameObject.Find("BackGround/BtnGold").GetComponent<Button>();
            btnItem = GameObject.Find("BackGround/BtnItem").GetComponent<Button>();

            gridDiamond = diamondPanel.transform.Find("Grid").GetComponent<GridLayoutGroup>();
            gridGold = goldPanel.transform.Find("Grid").GetComponent<GridLayoutGroup>();
            gridItem = itemPanel.transform.Find("Grid").GetComponent<GridLayoutGroup>();

            diamondTemp = diamondPanel.transform.Find("Templet").gameObject;
            goldTemp = goldPanel.transform.Find("Templet").gameObject;
            itemTemp = itemPanel.transform.Find("Templet").gameObject;

            btnDiamond.onClick.AddListener(delegate ()
            {
                OnShowDiamondPanel();
            });

            btnGold.onClick.AddListener(delegate ()
            {
                OnShowGoldPanel();
            });

            btnItem.onClick.AddListener(delegate ()
            {
                OnShowItemPanel();
            });

            InitDiamondPanel();
            InitGoldPanel();
            InitItemPanel();
        }

        private void OnShowDiamondPanel()
        {
            diamondPanel.SetActive(true);
            goldPanel.SetActive(false);
            itemPanel.SetActive(false);
        }

        private void OnShowGoldPanel()
        {
            diamondPanel.SetActive(false);
            goldPanel.SetActive(true);
            itemPanel.SetActive(false);
        }
        
        private void OnShowItemPanel()
        {
            diamondPanel.SetActive(false);
            goldPanel.SetActive(false);
            itemPanel.SetActive(true);
        }

        private void InitDiamondPanel()
        {
            int num = 0;
            Vector3 gridPos = gridDiamond.transform.localPosition;
            foreach (KeyValuePair<uint, tagFishRechargeInfo> map in FishConfig.Instance.m_FishRecharge.m_FishRechargeMap)
            {
                if (!map.Value.IsAddCurrcey())
                    continue;
                if (map.Value.IsFirstAdd() && !PlayerRole.Instance.RoleInfo.RoleMe.GetIsFirstPayCurrcey())
                    continue;
                ShopItem item = new ShopItem();
                item.Init(diamondTemp);
                item.ShowGoodsInfo(map.Key, map.Value, PayType.Diamond);
                item.m_BaseTrans.parent = gridDiamond.transform;
                //item.ResetLocalScale();
                num++;
            }
            RectTransform rectTransform = gridDiamond.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(1100, 240* num);
            diamondTemp.SetActive(false);
            gridDiamond.transform.localPosition = gridPos;
        }

        private void InitGoldPanel()
        {
            int num = 0;
            foreach (KeyValuePair<uint, tagFishRechargeInfo> map in FishConfig.Instance.m_FishRecharge.m_FishRechargeMap)
            {
                if (!map.Value.IsAddGlobel())
                    continue;
                if (map.Value.IsFirstAdd() && !PlayerRole.Instance.RoleInfo.RoleMe.GetIsFirstPayGlobel())
                    continue;
                ShopItem item = new ShopItem();
                item.Init(goldTemp);
                item.ShowGoodsInfo(map.Key, map.Value, PayType.Gold);
                item.m_BaseTrans.parent = gridGold.transform;
                num++;
                //item.ResetLocalScale();
            }
            RectTransform rectTransform = gridGold.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(1100, 240*num);
            goldTemp.SetActive(false);
        }

        private void InitItemPanel()
        {
            ClearGird();
            int num = 0;
            byte propertyID = (byte)(Shop_Type.Shop_Property + 1);
            if (FishConfig.Instance.m_ShopInfo.ShopMap.ContainsKey(propertyID) == false)
                return;
            tagShopConfig shopItemMap = FishConfig.Instance.m_ShopInfo.ShopMap[propertyID];
            if (shopItemMap == null)
                return;
            foreach (KeyValuePair<byte, tagShopItemConfig> map in shopItemMap.ShopItemMap)
            {

                PropertyItem item = new PropertyItem();
                item.Init(itemTemp);
                item.ShowGoodsInfo(map.Key, map.Value, shopItemMap.ShopItemStrMap[map.Key]);
                item.m_BaseTrans.parent = gridItem.transform;
                num++;
            }
            RectTransform rectTransform = gridItem.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(1100, 240 * num);
            itemTemp.SetActive(false);
        }
        void ClearGird()
        {
           
        }

    }



}

