/***********************************************
	FileName: Swirl.cs	    
	Creation: 2017-08-29
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Swirl : MonoBehaviour
{
    float angle = 0;
    float radius = 0.1f;
    Material mat;
    void Start()
    {
        mat = this.GetComponent<Image>().material;

        //延迟2秒开始，每隔0.2s调用一次
        InvokeRepeating("DoSwirl", 1f, 0.1f);
    }

    void DoSwirl()
    {
        angle += 1f;
        radius += 0.1f;

        mat.SetFloat("_Angle", angle);
        mat.SetFloat("_Radius", radius);

        //rest
        if (radius >= 0.6f)
        {
            angle = 0;
            radius = 0.1f;
        }
    }
}
