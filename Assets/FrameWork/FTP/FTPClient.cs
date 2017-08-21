using UnityEngine;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
namespace GF
{
    class NetCmdCheckVer : NetCmd
    {
        [TypeInfo(0)]
        public uint Version;
        [TypeInfo(1)]
        public byte Plateform;
        [TypeInfo(2)]
        public ushort ScreenWidth;
        [TypeInfo(3)]
        public ushort ScreenHeight;
        [TypeInfo(4, 48)]
        public byte[] MacAddress;
        [TypeInfo(5, 48)]
        public byte[] Package;
        [TypeInfo(6)]
        public ushort Length;
        [TypeInfo(7, true)]
        public byte[] FileName;
    }
    class NetCmdFileRequest : NetCmd
    {
        [TypeInfo(0)]
        public ushort Length;
        [TypeInfo(1)]
        public ushort Count; //文件个数
        [TypeInfo(2, true)]
        public byte[] FileName;
    };
    public class MultiFileOK
    {
        //public DownResData Drd;
        public byte[] Data;
    }
    public class RecvFileDataFlag
    {
        public byte[] FileData;
        public uint RecvSize;
    }
    public class CheckServerIP
    {
        public CheckServerIP(byte idx)
        {
            ServerIdx = idx;
        }
        public uint UseCount;
        public uint OkCount;
        public uint FailedCount;
        public byte ServerIdx;
        public void BeginUse()
        {
            ++UseCount;
        }
        public void EndUse(bool bOK)
        {
            --UseCount;
            if (bOK) ++OkCount;
            else ++FailedCount;
        }
    }
    public enum ChunkState
    {
        CHUNK_CONNECT,
        CHUNK_SEND_CMD,
        CHUNK_RECV_SIZE,
        CHUNK_RECV_DATA,
        CHUNK_COMPLETION,
    }
    public class DownChunkData
    {
        public ChunkState State;
        public byte FileIdx;
        public uint ChunkIdx;
        public byte RetryCount;
        public int Offset;
        public int Length;
        public int RecvSize;
        public uint RecvTick;
        public byte XOR;
        public byte[] RecvData;
        public byte[] SendCmdData;
        public FTPConnectData Fcd;
        public CheckServerIP checkServer;
    }
    public class MultiFileDownData
    {
        //public List<DownResData> FileList;
        //public List<ServerIPData> FTPIPList;
        //public volatile MultiFileOK[] CompletionList;
        //public RecvFileDataFlag[] RecvFileList;
        //public int OutsideRecvIndex;
        //public volatile int RecvCount;
        //public int CurrentRecvSize;
        //public object ExtraData;
    }
    public class FTPConnectData
    {
        public Socket socket;
        public IPEndPoint ippoint;
        public volatile bool connected;
    }
    
}
