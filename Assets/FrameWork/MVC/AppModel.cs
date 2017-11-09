/***********************************************
	FileName: AppModel.cs	    
	Creation: 2017-07-07
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GF.NET;
using System;

namespace GF
{
    public class AppModel
    {
        protected Dictionary<string, EventDelegate> msgList = new Dictionary<string, EventDelegate>();

        public virtual void Update(float delt)
        {
            
        }

        protected virtual void _RegisterEvent(NetCmdType aEventName_cmd, EventDelegate aEventDelegate)
        {
            msgList[aEventName_cmd+""] = aEventDelegate;
            NetManager.Instance.AddNetEventListener(aEventName_cmd, aEventDelegate);
        }

        protected virtual void _removeMsgList()
        {
            foreach (var var in msgList)
            {
                NetManager.Instance.RemoveEventListener(var.Key, var.Value);
            }
        }

        public virtual void RemoveMsgList()
        {
            _removeMsgList();
        }
    }

}
