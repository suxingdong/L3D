/***********************************************
	FileName: TableManager.cs	    
	Creation: 2017-08-03
	Author：East.Su
	Version：V1.0.0
	Desc: 桌子管理器
**********************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using  GF;
using GF.UI;
using Lobby;

namespace BuYu
{
    public class TableManager : Singleton<TableManager>
    {
        private TableManager()
        {
            
        }


        public TableError IsCanEnterTable(Byte TableType, bool IsShowErrorID)
        {
            if (!FishConfig.Instance.m_TableInfo.m_TableConfig.ContainsKey(TableType))
            {
                if (!IsShowErrorID)
                    return TableError.TE_IsNotExists;
                tagUserOperationEvent pUOM = new tagUserOperationEvent(UserOperateMessage.UOM_Table_JoinTable_Failed_1);
                //MsgEventHandle.HandleMsg(pUOM);
                UIManager.Instance.ShowMessage(UserOperateMessage.UOM_Table_JoinTable_Failed_1.Description(), MessageBoxEnum.Style.Ok, null);
                return TableError.TE_IsNotExists;
            }

            if (FishConfig.Instance.m_TableInfo.m_TableConfig[TableType].MinLevel != 0xffffffff && PlayerRole.Instance.RoleInfo.RoleMe.GetLevel() < FishConfig.Instance.m_TableInfo.m_TableConfig[TableType].MinLevel)
            {
                if (!IsShowErrorID)
                    return TableError.TE_MinLevel;
                tagUserOperationEvent pUOM = new tagUserOperationEvent(UserOperateMessage.UOM_Table_JoinTable_Failed_9);
                //MsgEventHandle.HandleMsg(pUOM);
                UIManager.Instance.ShowMessage(UserOperateMessage.UOM_Table_JoinTable_Failed_9.Description(), MessageBoxEnum.Style.Ok, null);
                return TableError.TE_MinLevel;
            }
            if (FishConfig.Instance.m_TableInfo.m_TableConfig[TableType].MaxLevel != 0xffffffff && PlayerRole.Instance.RoleInfo.RoleMe.GetLevel() > FishConfig.Instance.m_TableInfo.m_TableConfig[TableType].MaxLevel)
            {
                if (!IsShowErrorID)
                    return TableError.TE_MaxLevel;
                tagUserOperationEvent pUOM = new tagUserOperationEvent(UserOperateMessage.UOM_Table_JoinTable_Failed_10);
                //MsgEventHandle.HandleMsg(pUOM);
                UIManager.Instance.ShowMessage(UserOperateMessage.UOM_Table_JoinTable_Failed_10.Description(), MessageBoxEnum.Style.Ok, null);
                return TableError.TE_MaxLevel;
            }

            if (!int256Function.GetBitStates(PlayerRole.Instance.RoleInfo.RoleMe.GetRateValue(), FishConfig.Instance.m_TableInfo.m_TableConfig[TableType].MinRate))
            {
                if (!IsShowErrorID)
                    return TableError.TE_RateError;
                tagUserOperationEvent pUOM = new tagUserOperationEvent(UserOperateMessage.UOM_Table_JoinTable_Failed_8);
                //MsgEventHandle.HandleMsg(pUOM);
                UIManager.Instance.ShowMessage(UserOperateMessage.UOM_Table_JoinTable_Failed_8.Description(), MessageBoxEnum.Style.Ok, null);
                return TableError.TE_RateError;
            }
            if (FishConfig.Instance.m_TableInfo.m_TableConfig[TableType].MinGlobelSum != 0xffffffff && PlayerRole.Instance.RoleInfo.RoleMe.GetGlobel() < FishConfig.Instance.m_TableInfo.m_TableConfig[TableType].MinGlobelSum)
            {
                if (!IsShowErrorID)
                    return TableError.TE_MinGlobel;
                tagUserOperationEvent pUOM = new tagUserOperationEvent(UserOperateMessage.UOM_Table_JoinTable_Failed_2);
                //MsgEventHandle.HandleMsg(pUOM);
                UIManager.Instance.ShowMessage(UserOperateMessage.UOM_Table_JoinTable_Failed_2.Description(), MessageBoxEnum.Style.Ok, null);
                return TableError.TE_MinGlobel;
            }
            if (FishConfig.Instance.m_TableInfo.m_TableConfig[TableType].MaxGlobelSum != 0xffffffff && PlayerRole.Instance.RoleInfo.RoleMe.GetGlobel() > FishConfig.Instance.m_TableInfo.m_TableConfig[TableType].MaxGlobelSum)
            {
                if (!IsShowErrorID)
                    return TableError.TE_MaxGlobel;
                tagUserOperationEvent pUOM = new tagUserOperationEvent(UserOperateMessage.UOM_Table_JoinTable_Failed_3);
                //MsgEventHandle.HandleMsg(pUOM);
                UIManager.Instance.ShowMessage(UserOperateMessage.UOM_Table_JoinTable_Failed_3.Description(), MessageBoxEnum.Style.Ok, null);
                return TableError.TE_MaxGlobel;
            }
            if (FishConfig.Instance.m_TableInfo.m_TableConfig[TableType].MinCurreySum != 0xffffffff && PlayerRole.Instance.RoleInfo.RoleMe.GetCurrency() < FishConfig.Instance.m_TableInfo.m_TableConfig[TableType].MinCurreySum)
            {
                if (!IsShowErrorID)
                    return TableError.TE_MinCurrcey;
                tagUserOperationEvent pUOM = new tagUserOperationEvent(UserOperateMessage.UOM_Table_JoinTable_Failed_4);
                //MsgEventHandle.HandleMsg(pUOM);
                UIManager.Instance.ShowMessage(UserOperateMessage.UOM_Table_JoinTable_Failed_4.Description(), MessageBoxEnum.Style.Ok, null);
                return TableError.TE_MinCurrcey;
            }
            if (FishConfig.Instance.m_TableInfo.m_TableConfig[TableType].MaxCurreySum != 0xffffffff && PlayerRole.Instance.RoleInfo.RoleMe.GetCurrency() > FishConfig.Instance.m_TableInfo.m_TableConfig[TableType].MaxCurreySum)
            {
                if (!IsShowErrorID)
                    return TableError.TE_MaxCurrcey;
                tagUserOperationEvent pUOM = new tagUserOperationEvent(UserOperateMessage.UOM_Table_JoinTable_Failed_5);
                //MsgEventHandle.HandleMsg(pUOM);
                UIManager.Instance.ShowMessage(UserOperateMessage.UOM_Table_JoinTable_Failed_5.Description(), MessageBoxEnum.Style.Ok, null);
                return TableError.TE_MaxCurrcey;
            }
            if (FishConfig.Instance.m_TableInfo.m_TableConfig[TableType].JoinItemMap.Count > 0)
            {
                for (int i = 0; i < FishConfig.Instance.m_TableInfo.m_TableConfig[TableType].JoinItemMap.Count; ++i)
                {
                    UInt32 ItemID = FishConfig.Instance.m_TableInfo.m_TableConfig[TableType].JoinItemMap.Keys.ElementAt<UInt32>(i);
                    UInt32 ItemSum = FishConfig.Instance.m_TableInfo.m_TableConfig[TableType].JoinItemMap.Values.ElementAt<UInt32>(i);
                    if (PlayerRole.Instance.ItemManager.GetItemSum(ItemID) < ItemSum)
                    {
                        if (!IsShowErrorID)
                            return TableError.TE_ItemError;
                        tagUserOperationEvent pUOM = new tagUserOperationEvent(UserOperateMessage.UOM_Table_JoinTable_Failed_6);
                        //MsgEventHandle.HandleMsg(pUOM);
                        UIManager.Instance.ShowMessage(UserOperateMessage.UOM_Table_JoinTable_Failed_6.Description(), MessageBoxEnum.Style.Ok, null);
                        return TableError.TE_ItemError;
                    }
                }
            }
            return TableError.TE_Sucess;
        }
    }

}

