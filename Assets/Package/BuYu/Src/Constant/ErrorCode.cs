/***********************************************
	FileName: ErrorCode.cs	    
	Creation: 2017-08-03
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using GF;
using UnityEngine;

namespace Lobby
{
    public enum TableError
    {
        [EnumDisplay("可以进入")]
        TE_Sucess = 1,//可以进入
        [EnumDisplay("乐币太多")]
        TE_MaxCurrcey = 2,//乐币太多
        [EnumDisplay("乐币太少")]
        TE_MinCurrcey = 3,//乐币太少
        [EnumDisplay("金币太多")]
        TE_MaxGlobel = 4,//金币太多
        [EnumDisplay("金币太少")]
        TE_MinGlobel = 5,//金币太少
        [EnumDisplay("缺少进入物品")]
        TE_ItemError = 6,//缺少进入物品
        [EnumDisplay("桌子不存在")]
        TE_IsNotExists = 7,//桌子不存在
        TE_RateError = 8,
        TE_MinLevel = 9,
        TE_MaxLevel = 10,
    }

}
