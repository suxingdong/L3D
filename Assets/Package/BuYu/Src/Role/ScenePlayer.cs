/***********************************************
	FileName: ScenePlayer.cs	    
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
    public class ScenePlayer
    {
        public PlayerExtraData Player = new PlayerExtraData();
        public Launcher Launcher;
        public byte ClientSeat;  //反转之后的客户端位置
        public byte RateIndex;
        public SceneComboEft ComboEft = null;     //连击效果显示
    }

}
