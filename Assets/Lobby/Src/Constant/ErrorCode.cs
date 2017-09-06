/***********************************************
	FileName: ErrorCode.cs	    
	Creation: 2017-07-25
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GF;

namespace Lobby
{
    public enum ErrorCode
    {
        OK = 0,
        [EnumDisplay("用户名或者密码为空")]
        UID_PWD_IS_NULL,

        [EnumDisplay("用户名或者密码长度大于3小于16")]
        UID_PWD_LONG_WRONG,

        [EnumDisplay("网络连接失败")]
        NET_CONNECT_FAIL,


    }

}

