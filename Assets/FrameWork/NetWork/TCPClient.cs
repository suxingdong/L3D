/***********************************************
	FileName: TCPClient.cs	    
	Creation: 2017-07-13
	Author：East.Su
	Version：V1.0.0
	Desc: Socket网络
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
    public class TCPClient : INetClient
    {
        //是否连接到服务器
        volatile bool _mBConnected;
        //TCP socket
        Socket _mSocket;
        //异步发送的列表
        readonly SafeArray<SendCmdPack> _mSendList = new SafeArray<SendCmdPack>(128);
        //回调的句柄
        readonly INetHandler _mHandler;

        public event EventHandler<CreateConnectionAsyncArgs> CreateConnectCompleted;
        public event EventHandler CloseHandler;
        public event EventHandler ConnectError;
        //public event EventHandler ReconnectHandle;

        //网络接收的内存池
        byte[] _mBuff;
        //网络接收的偏移长度
        int _mOffset;
        //网络接收的长度
        int _mRecvSize;
        
        public TCPClient(INetHandler handle)
        {
            _mHandler = handle;
        }

        public static Socket CreateTcpSocket( bool blocking)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                ExclusiveAddressUse = false,
                LingerState = new LingerOption(true, 1),
                NoDelay = true,
                ReceiveBufferSize = ClientSetting.SOCKET_RECV_BUFF_SIZE,
                SendBufferSize = ClientSetting.SOCKET_SEND_BUFF_SIZE,
                ReceiveTimeout = ClientSetting.CONNECT_TIMEOUT,
                SendTimeout = ClientSetting.TIMEOUT,
                Blocking = blocking
            };
            s.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, 1);
            return s;
        }

        private static uint GetTickCount()
        {
            return NetUtility.GetTickCount();
        }

        /// <summary>
        /// socket连接创建成功回调
        /// </summary>
        /// <param name="iar"></param>
        private void Connected(IAsyncResult iar)
        {
            try
            {
                if (CreateConnectCompleted != null)
                {
                    CreateConnectCompleted(this, new CreateConnectionAsyncArgs(true));
                }
                _mSocket.EndConnect(iar);

                _mBConnected = _mSocket.Connected;
                if(_mBConnected)
                {
                    _mSocket.Blocking = false;
                    _mBuff = new byte[ClientSetting.RECV_BUFF_SIZE];
                    _mOffset = 0;
                    _mRecvSize = 0;
                    _mSocket.ReceiveTimeout = ClientSetting.TIMEOUT;
                    _mSocket.BeginReceive(_mBuff, 0, ClientSetting.RECV_BUFF_SIZE, SocketFlags.None, new AsyncCallback(this.KeepConnect), _mBuff);
                }
                
                
            }
            catch (SocketException)
            {
                if (ConnectError != null)
                {
                    ConnectError(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// 保持socket连接, 一直监听server发过来的消息
        /// </summary>
        /// <param name="iar"></param>
        private void KeepConnect(IAsyncResult iar)
        {
            try
            {
                int num = _mSocket.EndReceive(iar);
                Debug.Log("TCP收到 = " + num);
                if (num > 0)
                {
                    _mRecvSize = num;                    
                    RecvData(_mBuff);
                    _mSocket.BeginReceive(_mBuff, _mOffset, ClientSetting.RECV_BUFF_SIZE-_mOffset, SocketFlags.None, KeepConnect, _mBuff);
                }
                else
                {
                    if (CloseHandler != null)
                    {
                        CloseHandler(this, new EventArgs());
                    }
                }
            }
            catch (SocketException)
            {
                Debug.LogError("TCP收到数据异常");
            }
        }

        void RecvData(byte[] buff)
        {
            while (_mRecvSize >= ClientSetting.CMD_MIN_SIZE)
            {
                uint recvID = System.BitConverter.ToUInt32(buff, _mOffset);
                if (recvID == ConstValue.HEARBEAT_ID)
                {
                    _mOffset += 4;
                    _mRecvSize -= 4;
                }
                else if (recvID == ConstValue.PING_ID)
                {
                    _mOffset += 4;
                    _mRecvSize -= 4;
                }
                else
                {                    
                    ushort cmdSize = System.BitConverter.ToUInt16(buff, _mOffset);
                    if (cmdSize > buff.Length)
                        return;
                    if (_mRecvSize >= cmdSize)
                    {
                        if (!_mHandler.Handle(buff, (ushort)_mOffset, (ushort)cmdSize))
                        {
                            return;
                        }
                        _mOffset += cmdSize;
                        _mRecvSize -= cmdSize;
                    }
                    else
                        break;
                }
            }
            int freeBuffSize = buff.Length - (_mOffset + _mRecvSize);
            if (freeBuffSize < 128)
            {
                System.Array.Copy(buff, _mOffset, buff, 0, _mRecvSize);
                _mOffset = 0;
            }
            return;
        }

        private void ThreadConnect(object obj)
        {
            try
            {
                _mSocket.BeginConnect((EndPoint)obj, Connected, _mSocket);
                
            }
            catch
            {
                // ignored
            }
        }
              

        void CloseSocket()
        {
            if (_mSocket != null)
            {
                try
                {
                    _mSocket.Shutdown(SocketShutdown.Both);
                    _mSocket.Close();
                    _mSocket = null;
                }
                catch
                {

                }
            }
        }


        /// <summary>
        /// 信息发送到server完成回调
        /// </summary>
        /// <param name="iar"></param>
        private void SendComplete(IAsyncResult iar)
        {
            Debug.Log("发送完成 "+ iar.AsyncState);
            this._mSocket.EndSend(iar);
        }

        public bool IsConnected
        {
            get
            {
                return _mSocket != null && _mSocket.Connected;
            }
        }


        public bool Connect(string ip, int port,bool bThread = true)
        {
            const uint connectTimeOut = 3000;
            try
            {
                _mBConnected = false;
                CloseSocket();
                _mSocket = CreateTcpSocket(true);
                IPEndPoint endpoit = new IPEndPoint(IPAddress.Parse(ip), port);
                Thread th = new Thread(new ParameterizedThreadStart(ThreadConnect));
                th.Start(endpoit);
                uint tick = GetTickCount();
                while (GetTickCount() - tick < connectTimeOut && _mBConnected == false)
                {
                    Thread.Sleep(50);
                }
                if (_mSocket.Connected == false)
                {
                    CloseSocket();
                    return false;
                }
                return true;
            }
            catch
            {
                Debug.LogError("网络连接异常");
                return false;
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

        
        public void Send(byte[] data)
        {
            if (this._mSocket == null)
            {
                if (this._mHandler != null)
                {
                    //this.m_Handler(this, new EventArgs());
                }
                return;
            }
            if (!this._mSocket.Connected)
            {
                if (this._mHandler != null)
                {
                    //this.m_Handler(this, new EventArgs());
                }
                return;
            }
            _mSocket.BeginSend(data, 0, data.Length, SocketFlags.None, SendComplete, _mSocket);
        }

        public void Update()
        {

        }

        public bool Connect(string ip, int port, uint newip, ushort newport, bool bThread = true)
        {
            //TODO
            return false;
        }

        public void Send<T>(NetCmdBase ncb)
        {
            Debug.Log("TCP = " + typeof(T).ToString());
            SendCmdPack scp;
            scp.Cmd = ncb;            
            scp.Hash = Utility.GetHash(typeof(T).ToString());// TypeSize<T>.HASH;
            if (_mSendList.HasSpace())
                _mSendList.AddItem(scp);
            else
                LogMgr.Log("发送命令队列已满");

            byte[] sendData = NetCmdHelper.CmdToBytes(_mSendList.GetItem(), 0);
            if(sendData!=null)
            {
                Send(sendData);
            }
            else
            {
                Debug.LogError("发送数据异常");
            }
            
        }
    }
}

