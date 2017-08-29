/***********************************************
	FileName: TransformExtension.cs	    
	Creation: 2017-08-29
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GF
{
    public static class TransformExtension
    {
        public static void DestroyChildren(this Transform trans)
        {
            foreach (Transform child in trans)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        public static Transform AddChildFromPrefab(this Transform trans, Transform prefab, string name = null)
        {
            Transform childTrans = GameObject.Instantiate(prefab) as Transform;
            childTrans.SetParent(trans, false);
            if (name != null)
            {
                childTrans.gameObject.name = name;
            }
            return childTrans;
        }
    }
}

