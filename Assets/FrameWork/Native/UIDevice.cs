/***********************************************
	FileName: UIDevice.cs	    
	Creation: 2017-07-25
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

namespace GF
{
    //只存放本地运行时数据
    public enum PlatformType
    {
        Windows,
        Android,
        IOS,
        OSX,
        MAX
    }

    public class UIDevice
    {

        public static string GetMacAddress()
        {
            string str = "NONE";
#if UNITY_ANDROID
            str = "Android 000000";//SDKMgr.Instance.AndroidObj.CallStatic<string>("_GetMac");
#elif UNITY_IOS
        str = _GetMac();
#else
        str = "Windows_None";
#endif
            return str;
            /*NetworkInterface[] nis = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface ni in nis)
            {
                //Debug.Log("Name = " + ni.Name);
                //Debug.Log("Des = " + ni.Description);
                //Debug.Log("Type = " + ni.NetworkInterfaceType.ToString());
                //Debug.Log("Mac地址 = " + ni.GetPhysicalAddress().ToString());
                return ni.GetPhysicalAddress().ToString();
            }
            return "NONE";*/
        }

        public static PlatformType GetPlatformString()
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            return PlatformType.Windows;
#elif UNITY_ANDROID
        return PlatformType.Android;
#elif UNITY_IOS
        return PlatformType.IOS;
#elif UNITY_STANDALONE_OSX
        return PlatformType.OSX;
#endif
        }


    }
}


