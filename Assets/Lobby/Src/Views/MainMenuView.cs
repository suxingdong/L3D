/***********************************************
	FileName: MainMenuView.cs	    
	Creation: 2017-07-12
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GF;
using GF.UI;
using UnityEngine.SceneManagement;

namespace Lobby
{
    public class MainMenuView : AppView
    {
        private Button btnFriends;
        private Button btnUserInfo;
        private Button btnRank;
        private Button btnBag;
        [SerializeField]
        private GridLayoutGroup gridGames;

        protected override void OnStart()
        {
            btnFriends = GameObject.Find("BtnFriends").GetComponent<Button>();
            btnRank = GameObject.Find("BtnRank").GetComponent<Button>();
            btnUserInfo = GameObject.Find("BtnUserInfo").GetComponent<Button>();
            btnBag = GameObject.Find("BtnBag").GetComponent<Button>();
            btnFriends.onClick.AddListener(delegate () {
                CL_Cmd_RoleRankBox ncb = new CL_Cmd_RoleRankBox();
                ncb.SetCmdType(NetCmdType.CMD_CL_CHANG_ROLERANKBOX);
                ncb.dwUserID = PlayerRole.Instance.RoleInfo.RoleMe.GetUserID();
                ncb.accout = 1;
                ncb.bSaveType = false;
                NetManager.Instance.Send<CL_Cmd_RoleRankBox>(ncb);
            });

            btnUserInfo.onClick.AddListener(delegate () {
                UIManager.Instance.ShowView<UserInfoView>();
            });

            btnRank.onClick.AddListener(delegate () {
                OnBtnShop();
            });

            btnBag.onClick.AddListener(delegate () {
                UIManager.Instance.ShowView<BagView>();
            });

            string v = VerManager.Instance.ResVer;
            Debug.Log("V"+v);
            if(gridGames == null)
            {
                gridGames = GetComponentInChildren<GridLayoutGroup>();
            }
            
            Button[] btnRoomItems = gridGames.transform.GetComponentsInChildren<Button>();
            foreach (Button item in btnRoomItems)
            {
                item.onClick.AddListener(delegate () {
                    this.onLoadGame(item.gameObject);
                });
            }
            StartCoroutine(LogonHall());

        }

        IEnumerator LogonHall()
        {
            while (!NetManager.Instance.IsConnected)
            {
                yield return new WaitForSeconds(0.01f);
            }
            ModelManager.Instance.Get<LoginModel>().SendLogonHallData();
            yield return null;
        }
        void Update()
        {

        }

        public void onShowTips()
        {
            UIManager.Instance.ShowMessage("HelloWorld", MessageBoxEnum.Style.Ok,null);
        }

        public void onBtnFriends()
        {
            UIManager.Instance.ShowView<FriendsView>();
        }

        public void OnBtnShop()
        {
            UIManager.Instance.ShowView<ShopView>();
        }

        public void onLoadGame(GameObject obj)
        {
            SceneManager.LoadScene("BuYuScene");
            UIManager.Instance.HideView<AccountLoginView>();
            UIManager.Instance.HideView<LoginView>();
        }
    }
}

