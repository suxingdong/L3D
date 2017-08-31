/***********************************************
	FileName: SceneBoot.cs	    
	Creation: 2017-08-08
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using GF;
using GF.UI;
using Lobby;
using UnityEngine;

namespace BuYu
{
    public class SceneBoot : MonoBehaviour
    {

        private static SceneBoot instance;
        public static SceneBoot Instance
        {
            get
            {
                if (instance == null)
                {
                    Debug.Log("SceneBoot fail");
                    return null;
                }
                return instance;
            }
        }


        public Camera UICamera;
        public Transform UIPanelTransform;

        private void Awake()
        {
            instance = this;
            UIManager.Instance.ShowView<RoomView>();
            ModelManager.Instance.Register<SceneModel>();
            ModelManager.Instance.Register<SkillModel>();
            UIPanelTransform = GameObject.Find("Canvas").transform;
        }

        void Start()
        {

        }

        void Update()
        {
            FishManager.Instance.Update(Time.deltaTime);
        }

        void OnLevelWasLoaded(int level)
        {
           
        }

        public void SwapBackgroundImage(byte imgIdx)
        {
            
        }

        public void StartInnerCoroutine()
        {
            
        }
    }


}
