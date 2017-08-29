/***********************************************
	FileName: GameView.cs	    
	Creation: 2017-08-08
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GF;
using UnityEngine.EventSystems;

namespace BuYu
{
    public class GameView : AppView, IPointerDownHandler,IPointerUpHandler
    {

        private SceneModel scemModel;

        void Start()
        {
            scemModel = ModelManager.Instance.Get<SceneModel>();

        }
        public void OnPointerDown(PointerEventData eventData)
        {
            scemModel.PlayerMgr.GetPlayer(0).Launcher.IsPress = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            scemModel.PlayerMgr.GetPlayer(0).Launcher.IsPress = false;
        }
    }
}

