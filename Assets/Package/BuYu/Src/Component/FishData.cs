/***********************************************
	FileName: FishData.cs	    
	Creation: 2017-08-03
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuYu
{
    using UnityEngine;
    using System.Collections.Generic;

    public class GroupData
    {
        public Vector3[] PosList = null;
        public float SpeedScaling;
        public ushort FishNum;
        public byte FishIndex;
        public float FishScaling;
        public float ActionSpeed = 1;
        public bool ActionUnite;
    }

    public class FishPathGroupData
    {
        public ushort PathGroupIndex;
        public float Speed;
        public byte FishIndex;
        public float FishScaling;
        public float ActionSpeed = 1;
        public bool ActionUnite;
    }
    public class GroupDataList
    {
        public FishPathGroupData PathGroupData; //路径群数据
        public GroupData[] GroupDataArray;
        public Vector3 FrontPosition;
        public ushort[] PathList;
    }
    public enum FishClipType
    {
        CLIP_YOUYONG = 0,   //游泳
        CLIP_SIWANG,        //死亡
        CLIP_CHAOFENG,      //嘲讽
        CLIP_GONGJI,        //攻击
        CLIP_MAX
    };
    public class ResFishData
    {
        public byte FishIndex;
        public Vector3 Size;
        public float[] ClipLength = new float[(int)FishClipType.CLIP_MAX];
    };

  


}
