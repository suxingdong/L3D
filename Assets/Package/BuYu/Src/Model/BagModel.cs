/***********************************************
	FileName: BagModel.cs	    
	Creation: 2017-08-31
	Author：East.Su
	Version：V1.0.0
	Desc: 背包数据Model
**********************************************/

using System.Collections;
using System.Collections.Generic;
using GF;
using Lobby;
using UnityEngine;

namespace BuYu
{
    public class BagModel : AppModel
    {
        public BagModel()
        {
            Init();
        }

        private void Init()
        {
            RegisterEvent();
        }
        private void RegisterEvent()
        {
            //服务器下发炮台数据
            _RegisterEvent(NetCmdType.CMD_LC_LauncherData, OnCanonData);
            //服务器下发道具数据
            _RegisterEvent(NetCmdType.CMD_LC_GetUserItem, OnUserItem);

        }

        public void OnCanonData(IEvent iEvent)
        {
            NetCmdPack pack = (NetCmdPack)iEvent.parameter;
            PlayerRole.Instance.RoleLauncher.HandleCmd(pack.cmd);
        }

        public void OnUserItem(IEvent iEvent)
        {
            NetCmdPack pack = (NetCmdPack)iEvent.parameter;
            PlayerRole.Instance.ItemManager.HandleCmd(pack.cmd);
        }

    }

}

