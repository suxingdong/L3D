/***********************************************
	FileName: LogonState.cs	    
	Creation: 2017-07-25
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

public enum LogonState
{
    LOGON_INIT,                 //等待用户选择登陆方式
    LOGON_FAST_LOGINNING,       //快速登陆
    LOGON_REGISTER_LOGINNING,   //注册登陆
    LOGON_NORMAL_LOGINNING,     //普通登陆
    LOGON_PHONE_GETCHECK,       //手机登陆获取验证码
    LOGON_PHONE_LOGINNING,      //手机登陆 
    LOGON_CONNECTING,           //正在连接服务器
    LOGON_WAIT_RESULT,          //正在等待结果
    LOGON_WAIT_HALL_RESULT,     //正在等待结果
    LOGON_ACCOUNT_OR_PWD_ERROR, //帐户或者密码错误
    LOGON_NET_ERROR,            //网络连接错误
    LOGON_ERROR,                //登陆失败，帐户原因
    LOGON_CONNECT_HALL,         //进入游戏大厅
    LOGON_HALL_ERROR,           //进入游戏大厅失败
    LOGON_LOGINED,              //登陆成功
    LOGON_WRITE,                //排队状态
    LOGON_SDK_LOGON_WAIT,       //等待SDK登陆
    LOGON_SDK_LOGINNING,        //SDK登陆成功，连接我们的服务器
    LOGON_PHONE_SECPWD,         //手机二级密码登陆
    LOGON_WEIXIN_LOGINNING,
    LOGON_QQ_LOGINNING,
}