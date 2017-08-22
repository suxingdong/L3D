/***********************************************
	FileName: Boot.cs	    
	Creation: 2017-07-10
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GF;
using BuYu;
using GF.UI;

namespace Lobby
{
    public class Boot : MonoBehaviour
    {
        public static Boot Instance;

        void Start()
        {
            Instance = this;
            ModelManager.Instance.Register<LoginModel>();
            //暂时放这里
            ModelManager.Instance.Register<SceneModel>();
            GF.Resolution.GlobalInit();
            NetCmdMapping.GlobalInit();
            DontDestroyOnLoad(this);
            UIManager.Instance.ShowView<LoginView>();
        }

        void Update()
        {
            NetManager.Instance.Update(Time.deltaTime);
            ModelManager.Instance.Update(Time.deltaTime);
        }

        public void StartInnerCoroutine(IEnumerator e)
        {
            StartCoroutine(e);
        }

        void OnLevelWasLoaded(int level)
        {
            if (level == 1)
            {

            }

        }

    }

}
