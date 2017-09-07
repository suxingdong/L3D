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
            ModelManager.Instance.Register<BagModel>();
            ModelManager.Instance.Register<ShopModel>();
            GF.Resolution.GlobalInit();
            NetCmdMapping.GlobalInit();
            DontDestroyOnLoad(this);
            UIManager.Instance.ShowView<LogonLoadResView>();
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
            if (level != 0)
            {
                UIManager.Instance.HideView<MainMenuView>();
                UIManager.Instance.HideView<RegisterView>();
            }

        }

    }

}
