/***********************************************
	FileName: UdpClient.cs	    
	Creation: 2017-07-30
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

namespace GF.NET
{
    public class UDPClient : INetClient
    {
        private const uint MDelay = 0;
        volatile bool _mBConnected;
        volatile bool _mBNotify;

        uint _mSendId = 1;
        uint _mSendTick = 0;
        uint _mRecvId = 1;
        uint _mSendCmdTick = 0;
        uint _mRecvTick = 0;
        readonly byte[] _buff = new byte[ClientSetting.RECV_BUFF_SIZE];
        readonly byte[] _hearbeatCmd = new byte[4] { 0xff, 0xff, 0xff, 0xff };
        bool _mBSendBackId = false;
        Socket _mSocket;
        readonly INetHandler _mHandler;
        readonly SafeArray<SendCmdPack> _mSendList = new SafeArray<SendCmdPack>(256);
        byte[] _mSendBuff;
        //---------------------------------------
        public UDPClient(INetHandler handler)
        {
            _mHandler = handler;
        }
        uint GetTickCount()
        {
            return Utility.GetTickCount();
        }
        public bool IsConnected
        {
            get
            {
                return _mSocket != null && _mSocket.Connected;
            }
        }
        public void Disconnect()
        {
            if (_mSocket != null && _mSocket.Connected)
            {
                CloseSocket();
                _mHandler.StateChanged(NetState.NET_DISCONNECT);
            }
        }

        Socket CreateUDPSocket()
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
            {
                ExclusiveAddressUse = false,
                ReceiveBufferSize = ClientSetting.SOCKET_RECV_BUFF_SIZE,
                SendBufferSize = ClientSetting.SOCKET_SEND_BUFF_SIZE,
                ReceiveTimeout = ClientSetting.CONNECT_TIMEOUT,
                SendTimeout = ClientSetting.TIMEOUT,
                Blocking = true
            };
            return s;
        }
        void ThreadConnect(object obj)
        {
            try
            {
                _mSocket.Connect((EndPoint)obj);
                _mBConnected = _mSocket.Connected;
            }
            catch
            {
            }
        }

        public bool Connect(string ip, int port, bool bThread = true)
        {
            return Connect(ip, port, 0, 0, bThread);
        }

        public bool Connect(string ip, int port, uint newip, ushort newport, bool bThread = true)
        {
            const uint CONNECT_TIME_OUT = 3000;
            try
            {
                _mBConnected = false;
                CloseSocket();
                _mSocket = TCPClient.CreateTcpSocket( true);
                IPEndPoint endpoit = new IPEndPoint(IPAddress.Parse(ip), port);
                Thread th = new Thread(new ParameterizedThreadStart(ThreadConnect));
                th.Start(endpoit);
                //连接TCP
                //===============================================
                uint tick = GetTickCount();
                while (GetTickCount() - tick < CONNECT_TIME_OUT && _mBConnected == false)
                {
                    Thread.Sleep(10);
                }
                if (_mSocket.Connected == false)
                {
                    CloseSocket();
                    return false;
                }
                //接收TCP返回数据
                //===============================================
                byte[] buff = new byte[64];
                int ret = _mSocket.Receive(buff, SocketFlags.None);
                CloseSocket();

                if (ret != 16 || System.BitConverter.ToUInt32(buff, 0) != ClientSetting.CONNECT_RESULT)
                    return false;
                uint Rand1 = System.BitConverter.ToUInt32(buff, 4) | 0xc0000000;
                uint Rand2 = System.BitConverter.ToUInt32(buff, 8) | 0xc0000000;

                int ServerPort = System.BitConverter.ToInt32(buff, 12);
                System.Array.Copy(System.BitConverter.GetBytes(0x8fffffff), 0, buff, 0, 4);
                System.Array.Copy(System.BitConverter.GetBytes(Rand1), 0, buff, 4, 4);
                System.Array.Copy(System.BitConverter.GetBytes(Rand2), 0, buff, 8, 4);
                System.Array.Copy(System.BitConverter.GetBytes(newip), 0, buff, 12, 4);
                System.Array.Copy(System.BitConverter.GetBytes(newport), 0, buff, 16, 2);

                _mSocket = CreateUDPSocket();
                endpoit = new IPEndPoint(IPAddress.Parse(ip), ServerPort);
                _mSocket.Connect(endpoit);
                _mSocket.Blocking = false;
                //等待UDP双向确认。
                //===============================================
                bool bind = false;
                tick = GetTickCount();
                while (GetTickCount() - tick < CONNECT_TIME_OUT)
                {
                    try
                    {
                        _mSocket.Send(buff, 0, 18, SocketFlags.None);
                        ret = _mSocket.Receive(_buff, 0, _buff.Length, SocketFlags.None);
                    }
                    catch
                    {
                        ret = 0;
                    }

                    if (ret == 4 && System.BitConverter.ToUInt32(_buff, 0) == ConstValue.HEARBEAT_ID)
                    {
                        //与服务器绑定成功, 发送3次心跳。
                        for (int i = 0; i < 3; ++i)
                        {
                            try
                            {
                                _mSocket.Send(_hearbeatCmd, SocketFlags.None);
                            }
                            catch (System.Exception e)
                            {
                                //LogMgr.Log("UDP最后确认异常:" + e.ToString());
                            }
                            Thread.Sleep(50);
                        }
                        bind = true;
                        break;
                    }
                    Thread.Sleep(500);
                }
                if (bind == false)
                {
                    CloseSocket();
                    return false;
                }
                //UDP连接成功。
                //===============================================
                _mBNotify = false;
                _mRecvTick = GetTickCount();
                _mSendTick = GetTickCount();
                _mSendId = 1;
                _mRecvId = 1;
                _mBConnected = true;
                th = new Thread(new ThreadStart(ThreadRecv));
                th.Start();
                return true;
            }
            catch (System.Exception e)
            {
                LogMgr.Log("UDP连接失败:" + e.ToString());
            }
            return false;
        }
        public void Update()
        {
            if (_mBNotify)
            {
                _mBNotify = false;
                _mHandler.StateChanged(NetState.NET_DISCONNECT);
            }
        }
        void CloseSocket()
        {
            if (_mSocket != null)
            {
                try
                {
                    _mSocket.Close();
                    _mSocket = null;
                }
                catch
                {
                }
            }
            _mBConnected = false;
        }
        public uint Ping
        {
            get
            {
                return MDelay;
            }
        }
        bool IS_ANSWER(uint X)
        {
            return (X & 0x80000000) != 0;
        }
        uint SET_ANSWER(uint X)
        {
            return X | 0x80000000;
        }
        uint RESET_ANSWER(uint X)
        {
            return X & 0x7fffffff;
        }
        bool RecvData(byte[] buff, int recvSize, Socket soekt)
        {

            return true;
        }
        bool Send(Socket s, byte[] data)
        {
            try
            {
                s.Send(data, SocketFlags.None);
            }
            catch (SocketException e)
            {
                if (e.ErrorCode == 10035)
                    return false;
            }
            return true;
        }
        void ToBytes(uint id, byte[] buff, int offset)
        {
            buff[offset + 0] = (byte)(id & 0xff);
            buff[offset + 1] = (byte)((id >> 8) & 0xff);
            buff[offset + 2] = (byte)((id >> 16) & 0xff);
            buff[offset + 3] = (byte)((id >> 24) & 0xff);
        }
        bool RecvData(Socket socket, int recvSize)
        {
            int offset = 0;
            while (recvSize > 0)
            {
                uint recvID = System.BitConverter.ToUInt32(_buff, offset);
                offset += 4;
                recvSize -= 4;
                if (recvID == ConstValue.HEARBEAT_ID || recvID == ConstValue.PING_ID)
                {
                    //m_bSendPing = true;
                }
                else if (IS_ANSWER(recvID))
                {
                    recvID = RESET_ANSWER(recvID);
                    if (recvID == _mSendId && _mSendBuff != null)
                    {
                        _mSendBuff = null;
                    }
                }
                else
                {
                    ushort cmdSize = System.BitConverter.ToUInt16(_buff, offset);
                    if (cmdSize > recvSize)
                    {
                        return false;
                    }
                    if (recvID == _mRecvId + 1)
                    {
                        //对方第一次发送的命令
                        ++_mRecvId;
                        _mHandler.Handle(_buff, (ushort)offset, (ushort)cmdSize);
                    }
                    _mBSendBackId = true;
                    recvSize -= cmdSize;
                    offset += cmdSize;
                }
            }
            return true;
        }
        public void ThreadRecv()
        {
            Socket socket = _mSocket;
            int recvSize = 0;
            const uint halfTimeout = 1000;
            while (_mBConnected)
            {
                uint tick = GetTickCount();
#if UNITY_EDITOR
//                if (SceneMain.Exited)
//                {
//                    CloseSocket();
//                    break;
//                }
#endif
                for (int i = 0; i < 2; ++i)
                {
                    try
                    {
                        recvSize = socket.Receive(_buff, 0, _buff.Length, SocketFlags.None);
                        if (recvSize > 0)
                        {
                            if (RecvData(socket, recvSize) == false)
                            {
                                CloseSocket();
                                _mBNotify = true;
                                return;
                            }
                            else
                                _mRecvTick = tick;
                        }
                        else
                            break;
                    }
                    catch
                    {
                        break;
                    }
                }
                if (tick - _mRecvTick > ClientSetting.TIMEOUT)
                {
                    CloseSocket();
                    _mBNotify = true;
                    return;
                }
                if (_mSendBuff != null)
                {
                    if (tick - _mSendCmdTick > ClientSetting.UDP_RESEND_TICK || _mBSendBackId)
                    {
                        ToBytes(SET_ANSWER(_mRecvId), _mSendBuff, 0);
                        if (Send(socket, _mSendBuff))
                        {
                            _mBSendBackId = false;
                            _mSendTick = _mSendCmdTick = tick;
                        }
                    }
                }
                else if (_mSendList.HasItem())
                {
                    _mSendBuff = NetCmdHelper.CmdToBytes(_mSendList.GetItem(), 8);
                    ++_mSendId;
                    ToBytes(SET_ANSWER(_mRecvId), _mSendBuff, 0);
                    ToBytes(_mSendId, _mSendBuff, 4);
                    if (Send(socket, _mSendBuff))
                    {
                        _mBSendBackId = false;
                        _mSendTick = _mSendCmdTick = tick;
                    }
                    else
                        _mSendCmdTick = 0;
                }
                else if (_mBSendBackId)
                {
                    ToBytes(SET_ANSWER(_mRecvId), _hearbeatCmd, 0);
                    if (Send(socket, _hearbeatCmd))
                    {
                        _mSendTick = tick;
                        _mBSendBackId = false;
                    }
                }
                else if (tick - _mSendTick > halfTimeout)
                {
                    if (Send(socket, _hearbeatCmd))
                        _mSendTick = tick;
                }
                Thread.Sleep(1);
            }
        }
        public void Send<T>(NetCmdBase ncb)
        {
            Debug.Log("UDP = " + typeof(T).ToString());
            SendCmdPack scp;
            scp.Cmd = ncb;
            scp.Hash = TypeSize<T>.HASH;
            if (_mSendList.HasSpace())
                _mSendList.AddItem(scp);
            else
            {
                LogMgr.Log("发送命令队列已满");
            }
        }
    }
}

