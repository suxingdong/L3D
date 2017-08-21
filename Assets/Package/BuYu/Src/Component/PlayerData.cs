/***********************************************
	FileName: PlayerData.cs	    
	Creation: 2017-08-10
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuYu
{
    public class PlayerData
    {
        public string Name;
        public uint ID;         //玩家ID
        public byte Level;      //等级
        public uint ImgCrc;     //头像CRC
        public int GoldNum;    //金币数量
    }
    public class PlayerExtraData
    {
        public PlayerData playerData = new PlayerData();
    }



}
