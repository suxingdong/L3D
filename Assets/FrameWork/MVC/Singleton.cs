/***********************************************
	FileName: Singleton.cs	    
	Creation: 2017-07-07
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


using System.Reflection;

namespace GF
{
    public abstract class Singleton<T> where T : Singleton<T>
    {
        private static T _instance = null;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    //if (!TypeUtils.IsAssignable(typeof(MonoBehaviour), typeof(T)))
                    {
                        ConstructorInfo[] ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                        ConstructorInfo ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);
                        if (ctor == null)
                            throw new Exception("Non-public constructor not found!");
                        _instance = ctor.Invoke(null) as T;
                    }
                }
                return _instance;
            }
        }

        //重置single都需要调用init初始化
        public virtual void init(params object[] paraList)
        {

        }

        public virtual void clear()
        {
        }
    }
}

