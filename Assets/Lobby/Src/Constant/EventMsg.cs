/***********************************************
	FileName: EventMsg.cs	    
	Creation: 2017-07-25
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using GF;
namespace Lobby
{
    public class EventMsg
    {
        public static string LOGON_SUCCESS         = "LOGON_SUCCESS";          //登录成功
        public static string LOGON_FAIL            = "LOGON_FAIL";             //登录失败
        public static string REGISTER_SUCCESS      = "REGISTER_SUCCESS";       //注册成功
        public static string REGISTER_FAIL         = "REGISTER_FAIL";          //注册失败

        public static string UPDATE_CANON_SKILL    = "UPDATE_CANON_SKILL";     //更新炮台
        public static string HIDE_CANON_SKILL      = "HIDE_CANON_SKILL";       //隐藏炮台
        public static string UPDATE_USERITEM       = "UPDATE_USERITEM";        //刷新ITEM
        public static string UPDATE_SKILLCDTIME    = "UPDATE_SKILLCDTIME";     //刷新技能CD

        public static string UPDATE_PLAYSCORE    = "UPDATE_PLAYSCORE";         //刷新玩家金币
    }
}




