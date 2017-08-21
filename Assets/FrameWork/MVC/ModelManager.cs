/***********************************************
	FileName: MoudelManager.cs	    
	Creation: 2017-07-07
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GF
{
    public class ModelManager:Singleton<ModelManager>
    {
        private Dictionary<System.Type, AppModel> allModelList = new Dictionary<System.Type, AppModel>();

        private ModelManager()
        {

        }

        public T Get<T>() where T : AppModel, new()
        {
            if (!allModelList.ContainsKey(typeof(T)))
            {
                return null;
            }

            T module = allModelList[typeof(T)] as T;
            return module;
        }

        public void Register<T>() where T : AppModel, new()
        {
            T moudel = new T();
            allModelList[typeof(T)] = moudel;
        }

        public void Register(AppModel appModule)
        {
            allModelList[appModule.GetType()] = appModule;
        }
    }
}

