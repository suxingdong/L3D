/***********************************************
	FileName: INetHandler.cs	    
	Creation: 2017-07-13
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

namespace GF.NET
{
    public interface INetHandler
    {
        bool Handle(byte[] data, ushort offset, ushort length);
        void StateChanged(NetState state);
        void ConnectSuccess();
        void ConnectFail();
    }
}

