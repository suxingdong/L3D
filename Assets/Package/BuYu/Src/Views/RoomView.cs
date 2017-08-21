/***********************************************
	FileName: RoomView.cs	    
	Creation: 2017-07-21
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GF;
using GF.UI;
using Lobby;

namespace BuYu
{
    public class RoomView : AppView
    {
        private Button btnBack;
        private Button btnRoomRect;
        private GridLayoutGroup gridRooms;
        void Start()
        {
            btnBack = transform.Find("BtnBack").GetComponent<Button>();
            btnBack.onClick.AddListener(delegate ()
            {
                UIManager.Instance.HideView<RoomView>();
                UIManager.Instance.ShowView<MainMenuView>();
            });

            if (gridRooms == null)
            {
                gridRooms = GetComponentInChildren<GridLayoutGroup>();
            }

            Button[] btnRoomItems = gridRooms.transform.GetComponentsInChildren<Button>();
            foreach (Button item in btnRoomItems)
            {
                item.onClick.AddListener(delegate () {
                    this.onBtnClickRoom(item.gameObject);
                });
            }
            ModelManager.Instance.Register<RoomModel>();

            //TODO
            FishResManager.Instance.Init();
            PathManager.Instance.Init();
        }

        void Update()
        {

        }

        public void onBtnClickRoom(GameObject obj)
        {
            ModelManager.Instance.Get<RoomModel>().OnEnterRoom(1);
            UIManager.Instance.HideView<RoomView>();
            UIManager.Instance.ShowView<LoadResView>();
        }

    }

}
