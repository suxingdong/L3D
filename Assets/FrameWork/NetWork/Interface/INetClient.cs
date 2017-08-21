/***********************************************
	FileName: INetClient.cs	    
	Creation: 2017-07-13
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

namespace GF.NET
{
    public interface INetClient
    {
        bool Connect(string ip, int port, bool bThread = true);
        bool Connect(string ip, int port, uint newip, ushort newport, bool bThread = true);
        void Disconnect();
        bool IsConnected { get; }
        void Update();
        //void Send(byte[] data);
        void Send<T>(NetCmdBase dd);
    }
}

