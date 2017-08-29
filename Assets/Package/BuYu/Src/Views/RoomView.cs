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
        private Button btnUser;
        private Button btnRoomRect;
        private GridLayoutGroup gridRooms;
        private Text textNickName;
        private Text textLv;
        private Text textGold;
        private Text textDiamond;

        
        void Start()
        {
            
            btnBack = transform.Find("TopUI/BtnBack").GetComponent<Button>();
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

            InitUserInfo();
            //TODO
            FishResManager.Instance.Init();
            PathManager.Instance.Init();
        }

        void InitUserInfo()
        {
            btnUser = transform.Find("TopUI/BtnUser").GetComponent<Button>();
            textNickName = btnUser.transform.Find("TextNickName").GetComponent<Text>();
            textLv = btnUser.transform.Find("TextLV").GetComponent<Text>();
            //textLv = transform.Find("TopUI/TextNickName").GetComponent<Text>();

            textLv.text = PlayerRole.Instance.RoleInfo.RoleMe.GetLevel().ToString();
            textNickName.text = PlayerRole.Instance.RoleInfo.RoleMe.GetNickName();

        }

        void Update()
        {

        }

        public void onBtnClickRoom(GameObject obj)
        {
            ModelManager.Instance.Get<RoomModel>().OnEnterRoom(1);
            UIManager.Instance.HideView<RoomView>();
            UIManager.Instance.ShowTopView<LoadResView>();
        }

    }

}
