/***********************************************
	FileName: ServerSetting.cs	    
	Creation: 2017-07-25
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GF
{
    class LauncherInfo
    {
        public byte nmincatch;
        public byte nmaxcatch;
        public ushort nrevise;
        public ushort nincome;
        public float denergyratio;
    }
    public enum ISPType
    {
        ISP_DX = 0,
        ISP_LT,
        ISP_YD,
        ISP_MAX,
    }
    public class ServerIPData
    {
        public ServerIPData()
        {
            IP = "";
            Port = 0;
            Connected = true;
        }
        public byte ISP;
        public string IP;
        public ushort Port;
        public bool Connected;
    }

    public class ServerSetting
    {

        public static List<ServerIPData> ServerList = new List<ServerIPData>();
        public static string ResFtp;
        public static ushort ResFtpPort = 45045;
        public static string RunFtp;
        public static string HallServerIP;
        public static ushort HallServerPort;
        public static uint NewIP;
        public static ushort NewPort;
        public static string RT_IMAGE_DIR;
        public static string RT_XML_DIR;
        public static string CALLBACK_URL;
        public static string SERVICES_URL;
        public static bool IS_TEST = true;
        public static string VERSION_DIR;
        public static uint RES_VERSION;
        public static bool SHOW_PING;
        public static string ReporterIP = null;
        public static ushort ReporterPort;
        public static int ConnectServerIdx = -1;
        public static uint ClientVer;
        public static bool ShowExchange = true;    //对换
        public static bool ShowExtraBtn = true;
        public static bool ShowGame = true;
        public static bool ShowHallThirdBtn = true;
        public static bool ShowJBP = true;
        public static string ShareWebUrl = "", ShareTxt = "", ShareImgUrl = "";
        public static bool ShowMatch = false;//显示比赛按钮
        public static bool ShowThirdLoginBtn = false;//显示第三方登录按钮

    }

}
