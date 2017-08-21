/***********************************************
	FileName: SocketMessageReceivedFromServer.cs	    
	Creation: 2017-07-13
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System;

namespace GF.NET
{
    /// <summary>
    /// SocketMessageReceivedFromServer 的摘要说明
    /// 
    /// <para>____________________________________________________________</para>
    /// <para>Version：V1.0.0</para>
    /// <para>Namespace：GameSocket</para>
    /// <para>Author: wboy    Time：2014/4/25 10:21:25</para>
    /// </summary>
    public class SocketMessageReceivedFromServer : EventArgs
    {
        public byte[] Message
        {
            get;
            private set;
        }

        public int BytesTransferred
        {
            get;
            set;
        }

        public SocketMessageReceivedFromServer(byte[] data, int dataSize)
        {
            this.Message = data;
            this.BytesTransferred = dataSize;
        }
    }
}

