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
        private Button btnPetBoss;
        private GridLayoutGroup gridRooms;
        private Text textNickName;
        private Text textLv;
        private Text textGold;
        private Text textDiamond;


        protected override void OnStart()
        {
            btnBack = transform.Find("TopUI/BtnBack").GetComponent<Button>();
            btnBack.onClick.AddListener(delegate ()
            {
                UIManager.Instance.HideView<RoomView>();
                UIManager.Instance.ShowView<MainMenuView>();
                SceneManager.LoadScene("MainScene");
            });

            btnPetBoss = transform.FindChild("BottomUI/BtnPetBoss").GetComponent<Button>();
            btnPetBoss.onClick.AddListener(delegate ()
            {
                Handheld.PlayFullScreenMovie("petboss.mp4", Color.black, FullScreenMovieControlMode.Minimal);
            });

            if (gridRooms == null)
            {
                gridRooms = GetComponentInChildren<GridLayoutGroup>();
            }

            Button[] btnRoomItems = gridRooms.transform.GetComponentsInChildren<Button>();
            foreach (Button item in btnRoomItems)
            {
                item.onClick.AddListener(delegate () {
                    onBtnClickRoom(item.gameObject);
                });
            }
            ModelManager.Instance.Register<RoomModel>();

            InitUserInfo();
            InitBottom();
            //TODO
            FishResManager.Instance.Init();
            PathManager.Instance.Init();
        }

        void InitBottom()
        {
            Button btn = transform.Find("BottomUI/BtnShop").GetComponent<Button>();
            btn.onClick.AddListener(delegate ()
            {
                UIManager.Instance.ShowView<ShopView>();
            });

            btn = transform.Find("BottomUI/BtnPetBoss").GetComponent<Button>();
            btn.onClick.AddListener(delegate ()
            {
                UIManager.Instance.ShowView<IllustratView>();
            });
        }

        void InitUserInfo()
        {
            textGold = transform.Find("TopUI/BtnGold/TextGold").GetComponent<Text>();
            textDiamond = transform.Find("TopUI/BtnDiamond/TextGold").GetComponent<Text>();

            btnUser = transform.Find("TopUI/BtnUser").GetComponent<Button>();
            textNickName = btnUser.transform.Find("TextNickName").GetComponent<Text>();
            textLv = btnUser.transform.Find("TextLV").GetComponent<Text>();

            textLv.text = PlayerRole.Instance.RoleInfo.RoleMe.GetLevel().ToString();
            textNickName.text = PlayerRole.Instance.RoleInfo.RoleMe.GetNickName();
            textDiamond.text = PlayerRole.Instance.RoleInfo.RoleMe.GetCurrency().ToString();
            textGold.text = PlayerRole.Instance.RoleInfo.RoleMe.GetGlobel().ToString();

            btnUser.onClick.AddListener(delegate ()
            {
                UIManager.Instance.ShowView<UserInfoView>();
            });
        }

        void Update()
        {

        }

        public void onBtnClickRoom(GameObject obj)
        {
            bool ret = false;
            if (obj.name == "BaiPao")
            {
                ret = ModelManager.Instance.Get<RoomModel>().OnEnterRoom(1);
            }
            else if (obj.name == "QianPao")
            {
                ret = ModelManager.Instance.Get<RoomModel>().OnEnterRoom(2);
            }
            else
            {
                ret = ModelManager.Instance.Get<RoomModel>().OnEnterRoom(3);
            }
            if (ret)
            {
                UIManager.Instance.HideView<RoomView>();
                UIManager.Instance.ShowTopView<LoadResView>();
            }
            
        }

        public void onBtnPetBoss(GameObject obj)
        {
            
        }


    }

}
