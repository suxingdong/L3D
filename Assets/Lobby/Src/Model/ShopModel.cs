/***********************************************
	FileName: ShopModel.cs	    
	Creation: 2017-09-06
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

namespace Lobby
{
    public class ShopModel : AppModel
    {

        public ShopModel()
        {
            RegisterEvent();
        }
        public void RegisterEvent()
        {
            //服务器下发鱼
            _RegisterEvent(NetCmdType.CMD_LC_Recharge, OnRecharge);
            _RegisterEvent(NetCmdType.CMD_LC_ChangeRoleGlobe, OnChangeRoleGlobe);
            _RegisterEvent(NetCmdType.CMD_LC_ChangeRoleTotalRecharge, OnChangeRoleTotalRecharge);

            _RegisterEvent(NetCmdType.CMD_LC_ShopItemResult, OnShopItemResult);

            
        }

        public void OnChangeRoleGlobe(IEvent iEvent)
        {
            NetCmdPack pack = (NetCmdPack)iEvent.parameter;
            //LC_Cmd_ChangeRoleGlobe ncb = (LC_Cmd_ChangeRoleGlobe)pack.cmd;
            //TODO 后续需要改进的地方
            PlayerRole.Instance.RoleInfo.HandleCmd(pack.cmd);
        }

        public void OnChangeRoleTotalRecharge(IEvent iEvent)
        {

        }

        public void OnShopItemResult(IEvent iEvent)
        {
            NetCmdPack pack = (NetCmdPack)iEvent.parameter;
            LC_Cmd_ShopItemResult ncb = (LC_Cmd_ShopItemResult)pack.cmd;

            if (ncb.Result)
            {
                UIManager.Instance.ShowMessage(UserOperateMessage.UOM_Shop_BuyItem_Sucess.Description(),MessageBoxEnum.Style.Ok, null);
            }
            else
            {
                UIManager.Instance.ShowMessage(UserOperateMessage.UOM_Shop_BuyItem_Failed_6.Description(), MessageBoxEnum.Style.Ok, null);
            }
        }

        public void OnRecharge(IEvent iEvent)
        {
            NetCmdPack pack = (NetCmdPack)iEvent.parameter;
            //玩家登陆的结果 或者是注册的结果
            LC_Cmd_Recharge ncb = (LC_Cmd_Recharge)pack.cmd;

            if (ncb.Result)
            {
                UIManager.Instance.ShowMessage("充值成功",MessageBoxEnum.Style.Ok, null);
            }
            else
            {
                UIManager.Instance.ShowMessage("充值失败", MessageBoxEnum.Style.Ok, null);
            }
        }

        public bool SendShopItem(Byte ShopID, Byte ItemIndex, UInt32 ItemSum)
        {
            //玩家购买物品
            //1.判断商店是否存在
            if (!FishConfig.Instance.m_ShopInfo.ShopMap.ContainsKey(ShopID))
            {
                /*tagUserOperationEvent pUOM = new tagUserOperationEvent(UserOperateMessage.UOM_Shop_BuyItem_Failed_1);
                MsgEventHandle.HandleMsg(pUOM);*/
                UIManager.Instance.ShowMessage(UserOperateMessage.UOM_Shop_BuyItem_Failed_1.Description(), MessageBoxEnum.Style.Ok, null
                    );
                return false;
            }
            //2.判断物品是否存在
            if (!FishConfig.Instance.m_ShopInfo.ShopMap[ShopID].ShopItemMap.ContainsKey(ItemIndex))
            {
                /*tagUserOperationEvent pUOM = new tagUserOperationEvent(UserOperateMessage.UOM_Shop_BuyItem_Failed_2);
                MsgEventHandle.HandleMsg(pUOM);*/
                UIManager.Instance.ShowMessage(UserOperateMessage.UOM_Shop_BuyItem_Failed_2.Description(),MessageBoxEnum.Style.Ok, null
                    );
                return false;
            }
            if (ItemSum > 1 && !FishConfig.Instance.m_ShopInfo.ShopMap[ShopID].ShopItemMap[ItemIndex].IsCanPile)
            {
                //不可以堆叠的物品购买多个 
                /*tagUserOperationEvent pUOM = new tagUserOperationEvent(UserOperateMessage.UOM_Shop_BuyItem_Failed_8);
                MsgEventHandle.HandleMsg(pUOM);*/
                UIManager.Instance.ShowMessage(UserOperateMessage.UOM_Shop_BuyItem_Failed_8.Description(), MessageBoxEnum.Style.Ok, null
                    );
                return false;
            }
            //3.判断物品是否已经激活
            if (!FishConfig.Instance.m_ShopInfo.ShopMap[ShopID].ShopItemMap[ItemIndex].IsInTime())
            {
                /*tagUserOperationEvent pUOM = new tagUserOperationEvent(UserOperateMessage.UOM_Shop_BuyItem_Failed_3);
                MsgEventHandle.HandleMsg(pUOM);*/
                UIManager.Instance.ShowMessage(UserOperateMessage.UOM_Shop_BuyItem_Failed_3.Description(), MessageBoxEnum.Style.Ok, null
                    );
                return false;
            }
            if (FishConfig.Instance.m_ShopInfo.ShopMap[ShopID].ShopItemMap[ItemIndex].ShopType == ShopItemType.SIT_Entity)//实体物品购买
            {
                /*tagRoleAddressInfo pEntity = PlayerRole.Instance.EntityManager.GetRoleEntityInfo();
                if (pEntity == null || !PlayerRole.Instance.EntityManager.CheckEntityInfoIsCanUser())
                {
                    tagUserOperationEvent pUOM = new tagUserOperationEvent(UserOperateMessage.UOM_Shop_BuyItem_Failed_4);
                    MsgEventHandle.HandleMsg(pUOM);
                    return false;
                }*/
            }
            if (FishConfig.Instance.m_ShopInfo.ShopMap[ShopID].ShopItemMap[ItemIndex].ShopType == ShopItemType.SIT_PhonePay)
            {
                /*tagRoleAddressInfo pEntity = PlayerRole.Instance.EntityManager.GetRoleEntityInfo();
                if (pEntity == null || !PlayerRole.Instance.EntityManager.CheckIsCanPhonePay())
                {
                    tagUserOperationEvent pUOM = new tagUserOperationEvent(UserOperateMessage.UOM_Shop_BuyItem_Failed_7);
                    MsgEventHandle.HandleMsg(pUOM);
                    return false;
                }*/
            }

            //4.判断物品的价格 玩家是否可以购买  
            if (
                PlayerRole.Instance.RoleInfo.RoleMe.GetGlobel() < FishConfig.Instance.m_ShopInfo.ShopMap[ShopID].ShopItemMap[ItemIndex].PriceGlobel * ItemSum ||
                PlayerRole.Instance.RoleInfo.RoleMe.GetMedal() < FishConfig.Instance.m_ShopInfo.ShopMap[ShopID].ShopItemMap[ItemIndex].PriceMabel * ItemSum ||
                PlayerRole.Instance.RoleInfo.RoleMe.GetCurrency() < FishConfig.Instance.m_ShopInfo.ShopMap[ShopID].ShopItemMap[ItemIndex].PriceCurrey * ItemSum
                )
            {
                /*tagUserOperationEvent pUOM = new tagUserOperationEvent(UserOperateMessage.UOM_Shop_BuyItem_Failed_5);
                MsgEventHandle.HandleMsg(pUOM);*/
                UIManager.Instance.ShowMessage(UserOperateMessage.UOM_Shop_BuyItem_Failed_5.Description(), MessageBoxEnum.Style.Ok, null
                    );
                return false;
            }

            //判断玩家兑换次数是否符合要求
            ShopItemType ItemType = FishConfig.Instance.m_ShopInfo.ShopMap[ShopID].ShopItemMap[ItemIndex].ShopType;
            if (ItemType == ShopItemType.SIT_PhonePay || ItemType == ShopItemType.SIT_Entity)
            {
                if (PlayerRole.Instance.RoleInfo.RoleMe.GetCashSum() >= PlayerRole.Instance.RoleVip.GetUseMedalSum())
                {
                    /*tagUserOperationEvent pUOM = new tagUserOperationEvent(UserOperateMessage.UOM_Shop_BuyItem_Failed_9);
                    MsgEventHandle.HandleMsg(pUOM);*/
                    UIManager.Instance.ShowMessage(UserOperateMessage.UOM_Shop_BuyItem_Failed_9.Description(), MessageBoxEnum.Style.Ok, null
                    );
                    return false;
                }
                //兑换次数符合要求 我们可以继续判断 部分平台上玩家 必须先点击分享才可以使用
                //1.配置文件判断 当前平台是否需要分享 (写入配置文件 360 官网包等 ) 
                /*if (IsNeedShare(ItemType))
                {
                    //必须先分享才可以兑换物品
                    tagUserOperationEvent pUOM = new tagUserOperationEvent(UserOperateMessage.UOM_Shop_BuyItem_Failed_10);
                    MsgEventHandle.HandleMsg(pUOM);
                    return false;
                }*/
            }
            //5.发送命令

            CL_Cmd_ShopItem ncb = new CL_Cmd_ShopItem();
            ncb.SetCmdType(NetCmdType.CMD_CL_ShopItem);
            ncb.ShopItemIndex = ItemIndex;
            ncb.ItemSum = ItemSum;
            ncb.ShopID = ShopID;
            NetManager.Instance.Send<CL_Cmd_ShopItem>(ncb);
            return true;
        }
    }

}
