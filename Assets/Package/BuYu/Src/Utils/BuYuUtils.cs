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

        //如果角度在指定范围内
        const float LOCK_FISH_DIST_SQR = 0.07f * 0.07f;
        public static bool CheckLauncherAngle(Fish fish, Vector3 scrStartPoint, Vector3 viewStartPoint)
        {
            Vector3 dir = fish.ScreenPos - scrStartPoint;
            dir.Normalize();
            float dot = Vector2.Dot(dir, Vector2.up);
            if (dot < -0.5f && (fish.ViewPos - viewStartPoint).sqrMagnitude > LOCK_FISH_DIST_SQR)
            {
                return false;
            }
            return true;
        }


    }


}
