/***********************************************
	FileName: BuYuUtils.cs	    
	Creation: 2017-08-04
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuYu
{
    public class BuYuUtils
    {
        public static float GetPathTimeByDist(float startX, float curX, PathLinearInterpolator pi)
        {
            float dist = startX - curX;
            float dist2 = pi.GetPos(0).x - pi.GetPos(0.01666f).x;
            return dist / dist2 * 0.01666f;
        }

    }


}
