/***********************************************
	FileName: NetUtility.cs	    
	Creation: 2017-07-13
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

namespace GF.NET
{
    public class NetUtility
    {

        public static uint GetTickCount()
        {
            return (uint)(GlobalTimer.Ticks);// 转换成毫秒
        }

    }
}

