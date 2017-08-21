/***********************************************
	FileName: NetManager.cs	    
	Creation: 2017-07-21
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GF.NET;
using System;
using GF.UI;
namespace GF
{
    public class NetCmdPack
    {
        public NetCmdBase cmd;
        public uint tick;
    }

    public class NetManager : Singleton<NetManager>, INetHandler
    {
        INetClient tCPClient;

        public EventDispatcher eventDispatcher;
        SafeArray<NetCmdPack> cmdList = new SafeArray<NetCmdPack>(256);
        private bool _canProcessCmd = true;
        private NetManager()
        {
            eventDispatcher = new EventDispatcher(this);
        }

        public bool AddNetEventListener(NetCmdType aEventName_cmd, EventDelegate aEventDelegate)
        {
            return AddEventListener(aEventName_cmd + "", aEventDelegate);
        }

        public bool AddEventListener(string aEventName_string, EventDelegate aEventDelegate)
        {
            return eventDispatcher.addEventListener(aEventName_string, aEventDelegate);
        }

        public bool RemoveEventListener(string aEventType_string, EventDelegate aEventDelegate)
        {
            return eventDispatcher.removeEventListener(aEventType_string, aEventDelegate);
        }

        public void removeAllEventListeners()
        {
            eventDispatcher.removeAllEventListeners();
        }


        public void DispatchEvent(IEvent aIEvent)
        {
            eventDispatcher.dispatchEvent(aIEvent);
        }


        public bool Connect(bool bTcp, string ip, ushort port, uint newip = 0, ushort newport = 0)
        {
            if (tCPClient != null && tCPClient.IsConnected)
            {
                Debug.Log("TCP is connected.");
                return false;
            }
            INetClient tt;
            bool bret = true;
            if (bTcp)
            {
                tt = new TCPClient(this);
                bret = tt.Connect(ip, port);
            }
            else
            {
                tt = new UDPClient(this);
                bret = tt.Connect(ip, port);
                //tt = new UDPClient(this);
                //bret = tt.Connect(ip, port, newip, newport);
            }

            tCPClient = tt;
            return bret;
        }

        public bool Handle(byte[] data, ushort offset, ushort length)
        {
            NetCmdBase cmd = NetCmdHelper.ByteToCmd(data, offset, length);
            if (cmd == null)
            {
                byte cmdType = data[offset + 3];
                byte cmdSubType = data[offset + 2];
                Debug.LogError("错误类型:" + cmdType + "," + cmdSubType);
                return false;
            }
            if (cmd.GetCmdType() == NetCmdType.CMD_HEARTBEAT)
            {
                Debug.Log("CMD_HEARTBEAT");
                //NetCmd nc = new NetCmd();
                //nc.SetCmdType(NetCmdType.CMD_HEARTBEAT);
                //Send<NetCmd>(nc);
            }
            else
            {
                NetCmdPack ncp = new NetCmdPack();
                ncp.cmd = cmd;
                Debug.Log("收到数据 = "+ cmd.GetCmdTypeToString());
                ncp.tick = Utility.GetTickCount();
                cmdList.AddItem(ncp);               
            }
            return true;
        }

        public void StateChanged(NetState state)
        {
            //TODO
        }

        public void ConnectSuccess()
        {
            
        }

        public void ConnectFail()
        {

        }


        public void Send<T>(NetCmdBase ncb)
        {
            if (typeof(T) != ncb.GetType())
            {
                LogMgr.Log("命令类型不相等:" + ncb.ToString());
                return;
            }
            if (IsConnected)
            {
                tCPClient.Send<T>(ncb);
            }
            //else
            //    LogMgr.Log("TCPClient don't connected, send cmd:" + ncb.ToString());
        }

        public bool IsConnected
        {
            get
            {
                return tCPClient != null && tCPClient.IsConnected;
            }
        }

        public INetClient TcpClient
        {
            get
            {
                return tCPClient;
            }
        }

        public void Disconnect()
        {
            if (tCPClient != null)
            {
                tCPClient.Disconnect();
                tCPClient = null;
            }
        }

        public bool Update(float delta)
        {
            if (tCPClient != null)
                tCPClient.Update();

            if (CanProcessCmd)
            {
                while (cmdList.HasItem())
                {
                    NetCmdPack nc = cmdList.GetItem();
                    IEvent evt = new Event(nc.cmd.GetCmdTypeToString());
                    evt.parameter = nc;
                    DispatchEvent(evt);
                }
            }
            
            return true;
        }

        public bool CanProcessCmd
        {
            set { _canProcessCmd = value; }
            get { return _canProcessCmd; }
        }
    }
}

