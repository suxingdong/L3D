/***********************************************
	FileName: NativeInterface.cs	    
	Creation: 2017-07-25
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GF
{
    public class NativeInterface
    {
        public static bool ComputeCrc(string str, string pwd, out uint crc1, out uint crc2, out uint crc3)
        {
            string pwd1 = (pwd + "OnePwd").ToLower().Trim();
            string pwd2 = (pwd + "SecodPwd").ToLower().Trim();
            string pwd3 = (pwd + "ThreePwd").ToLower().Trim();

            crc1 = Crc.Crc32(Encoding.Default.GetBytes(pwd1.ToCharArray()), 0, pwd1.Length);
            crc2 = Crc.Crc32(Encoding.Default.GetBytes(pwd2.ToCharArray()), 0, pwd2.Length);
            crc3 = Crc.Crc32(Encoding.Default.GetBytes(pwd3.ToCharArray()), 0, pwd3.Length);
            return true;
        }

        public static string GetPackageName()
        {
            if (SDKMgr.IS_DISABLED_SDK)
                return "Test";
#if UNITY_ANDROID
        return SDKMgr.Instance.AndroidObj.CallStatic<string>("_GetPackageName");
#elif UNITY_IOS
        return _GetChannelCode();
#else
            return "com.leduo.buyu.self";
#endif
        }
        public static void OpenCamera()
        {
#if UNITY_ANDROID
        SDKMgr.Instance.AndroidObj.CallStatic("_OpenCamera", Application.persistentDataPath + "/images");
#elif UNITY_IOS
        _OpenCamera();
#endif
        }

    }
}

