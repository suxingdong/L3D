/***********************************************
	FileName: QuickShopView.cs	    
	Creation: 2017-09-12
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using GF;
using GF.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lobby
{
    public class QuickShopView : AppView, IPointerClickHandler
    {
        private Text textNum;
        private Text textCost;
        private Button btnAddNum;
        private Button btnSubNum;
        private UInt32 itemNum = 1;

        private byte onlyID;
        private byte shopID;
        private uint itemSum;
        private uint m_ItemPrice;
        private byte m_PriceType;        //购买货币的种类 ‘1’为金币 ‘2’为钻石 '3'为奖牌
        private ShopModel shopModel;
        public void OnPointerClick(PointerEventData eventData)
        {
            UIManager.Instance.HideView<QuickShopView>();
        }

        private void RegisterEvent()
        {
            //_RegisterEvent(EventMsg.REGISTER_SUCCESS, OnRegisterSuccess);
        }

        protected override void OnStart()
        {
            RegisterEvent();
            btnAddNum = transform.FindChild("BackGround/Option/BtnAdd").GetComponent<Button>();
            btnAddNum.onClick.AddListener(OnAddBuyItem);

            btnSubNum = transform.FindChild("BackGround/Option/BtnSub").GetComponent<Button>();
            btnSubNum.onClick.AddListener(OnSubBuyItem);

            textCost = transform.FindChild("BackGround/CostIcon/Text").GetComponent<Text>();
            textCost.text = "0";

            textNum = transform.FindChild("BackGround/Option/Image/TextNum").GetComponent<Text>();
            textNum.text = itemSum.ToString();

            Button button = transform.FindChild("BackGround/Button").GetComponent<Button>();
            button.onClick.AddListener(OnClickBuy);
            shopModel = ModelManager.Instance.Get<ShopModel>();
            InitData();
        }


        public void OnClickBuy()
        {
            UIManager.Instance.HideView<QuickShopView>();
            /*if (!IsMoneyEnough())
            {
                return;
            }*/
            if (shopModel.SendShopItem(shopID, onlyID, itemNum))
            {
                
            }
            
        }
        private void InitData()
        {
            //ShopItemType byShopType = FishConfig.Instance.m_ShopInfo.ShopMap[shopID].ShopItemMap[onlyID].ShopType;
            /*if (PlayerRole.Instance.ShopManager.IsNeedShare(byShopType))
            {
                GlobalHallUIMgr.Instance.GameShare.ShowExchage(FishConfig.Instance.m_ShopInfo.ShopMap[shopID].ShopItemMap[onlyID].ItemInfo.ItemID, PlayerRole.Instance.RoleInfo.RoleMe.GetTotalCashSum());
                return;
            }*/
            if (FishConfig.Instance.m_ShopInfo.ShopMap.ContainsKey(shopID) == false)
                return;
            tagShopItemStr pItemStr = FishConfig.Instance.m_ShopInfo.ShopMap[shopID].ShopItemStrMap[onlyID];


            Text text = transform.FindChild("BackGround/ItemIcon/TextNum").GetComponent<Text>();
            text.text = "当前拥有: x0 ";
            text = transform.FindChild("BackGround/ItemIcon/TextName").GetComponent<Text>();
            text.text = pItemStr.ItemName;

            Image image = transform.FindChild("BackGround/ItemIcon/Image").GetComponent<Image>();
            image.sprite = ResManager.Instance.LoadSprite("BuYu/Texture/GoodsIcon/" + pItemStr.ItemIcon);

            if (FishConfig.Instance.m_ShopInfo.ShopMap[shopID].ShopItemMap[onlyID].ShopType == ShopItemType.SIT_Entity ||
            FishConfig.Instance.m_ShopInfo.ShopMap[shopID].ShopItemMap[onlyID].ShopType == ShopItemType.SIT_PhonePay)
            {
            }
            else
            {
                textCost.text = "x " + FishConfig.Instance.m_ShopInfo.ShopMap[shopID].ShopItemMap[onlyID].ItemInfo.ItemSum.ToString();
                //m_ItemSum = itemSum;
                //m_ItemSumLabel.text = m_ItemSum.ToString();
            }
            GetItemPrice();
            textCost.text = (itemNum * m_ItemPrice).ToString();
            //textCost = itemNum*
        }

        void GetItemPrice()
        {
            if (FishConfig.Instance.m_ShopInfo.ShopMap[shopID].ShopItemMap[onlyID].PriceGlobel > 0)
            {
                m_ItemPrice = FishConfig.Instance.m_ShopInfo.ShopMap[shopID].ShopItemMap[onlyID].PriceGlobel;
                //显示响应的货币图标
                //m_MoneyTypeSprite.spriteName = "HallBtn_Gold";
                m_PriceType = 1;
            }
            else if (FishConfig.Instance.m_ShopInfo.ShopMap[shopID].ShopItemMap[onlyID].PriceMabel > 0)
            {
                m_ItemPrice = FishConfig.Instance.m_ShopInfo.ShopMap[shopID].ShopItemMap[onlyID].PriceMabel;
                //
                //m_MoneyTypeSprite.spriteName = "HallBtn_Medal";
                m_PriceType = 3;
            }
            else
            {
                m_ItemPrice = FishConfig.Instance.m_ShopInfo.ShopMap[shopID].ShopItemMap[onlyID].PriceCurrey;
                //
                //m_MoneyTypeSprite.spriteName = "HallBtn_Diamond";
                m_PriceType = 2;

            }
        }

        private void OnAddBuyItem()
        {
            itemNum++;
            textNum.text = itemNum.ToString();
            textCost.text = (itemNum * m_ItemPrice).ToString();
            if (itemNum > 1)
            {
                btnSubNum.enabled = true;
            }
        }

        private void OnSubBuyItem()
        {
            if (itemNum > 1)
            {
                itemNum--;
                textNum.text = itemNum.ToString();
                textCost.text = (itemNum * m_ItemPrice).ToString();
            }
            else
            {
                btnSubNum.enabled = false;
            }
        }

        public override void OnParams(object param)
        {
            object[] tParam = param as object[];
            shopID = (byte)tParam[0];
            onlyID = (byte)tParam[1];
            itemSum = (uint)tParam[2];
        }

    }


}
