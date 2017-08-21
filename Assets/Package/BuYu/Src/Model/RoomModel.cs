/***********************************************
	FileName: RoomModel.cs	    
	Creation: 2017-08-03
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using GF;
using GF.UI;
using UnityEngine;

namespace BuYu
{
    public enum RoomState
    {
        HALL_WAIT,
        HALL_JOIN_ROOM,
        HALL_JOIN_FAILED,
    }

    public class RoomModel : AppModel
    {
        byte _mState;

        public RoomModel()
        {
            
        }

        public RoomState State
        {
            get { return (RoomState)_mState; }
            set { _mState = (byte)value; }
        }


        public void OnEnterRoom(byte roomid)
        {
            /*TableError pError = TableManager.Instance.IsCanEnterTable(roomid, false);
            if (pError != TableError.TE_Sucess)
            {
                //进入房间失败了 我们进行处理 
                tagJoinTableEvent pEvent = new tagJoinTableEvent(roomid, pError);
                MsgEventHandle.HandleMsg(pEvent);//无法进入房间的事件
                UIManager.Instance.ShowMessage("",MessageBoxEnum.Style.Ok,null);
                return;
            }*/

            //GlobalEffectMgr.Instance.ShowLoadingMessage();

            //发送进入房间的命令到服务端去
            CL_Cmd_JoinTable ncb = new CL_Cmd_JoinTable();
            ncb.SetCmdType(NetCmdType.CMD_CL_JoinTable);
            ncb.bTableType = roomid;
            NetManager.Instance.Send<CL_Cmd_JoinTable>(ncb);

            State = RoomState.HALL_JOIN_ROOM;
        }

    }

}
