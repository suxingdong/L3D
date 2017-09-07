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
    }

}
