/***********************************************
	FileName: ICmdHandler.cs	    
	Creation: 2017-07-27
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

namespace GF.NET
{
    public interface ICmdHandler
    {
        bool CanProcessCmd();
        bool Handle(NetCmdPack cmd);
        void StateChanged(NetState state);
    }
}

