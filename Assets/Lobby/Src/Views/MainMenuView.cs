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
        private Button btnAddFriend;
        [SerializeField]
        private GridLayoutGroup gridGames;

        protected override void OnStart()
        {
            btnAddFriend = GameObject.Find("BtnAddFriends").GetComponent<Button>();
            btnAddFriend.onClick.AddListener(delegate () {
                this.onBtnFriends();
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

        public void onLoadGame(GameObject obj)
        {
            SceneManager.LoadScene("BuYuScene");
            UIManager.Instance.HideView<MainMenuView>();
            UIManager.Instance.HideView<RegisterView>();
            UIManager.Instance.HideView<LoginView>();
        }
    }
}

