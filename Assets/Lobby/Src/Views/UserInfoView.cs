/***********************************************
	FileName: UserInfoView.cs	    
	Creation: 2017-08-25
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GF;
using GF.UI;
using System;
using GF.NET;

namespace Lobby
{
    public class UserInfoView : AppView, IPointerClickHandler
    {
        private Button tableBtn1;
        private Button tableBtn2;
        private Button tableBtn3;

        private GameObject bagPanel;
        private GameObject archevePanel;
        private GameObject userInfoPanel;
        private Animation animation;

        public void OnPointerClick(PointerEventData eventData)
        {
            animation.Play("ViewOut");
            StartCoroutine(OnClose());
        }

        IEnumerator OnClose()
        {
            yield return new WaitForSeconds(0.2f);
            UIManager.Instance.HideView<UserInfoView>();
        }


        protected override void OnStart()
        {
            Transform parent = transform.Find("Background");
            tableBtn1 = parent.Find("TabeleBtn1").GetComponent<Button>();
            tableBtn2 = parent.Find("TabeleBtn2").GetComponent<Button>();
            tableBtn3 = parent.Find("TabeleBtn3").GetComponent<Button>();

            userInfoPanel = parent.Find("UserInfo").gameObject;
            archevePanel = parent.Find("Archeve").gameObject;
            bagPanel = parent.Find("Bag").gameObject;

            tableBtn1.onClick.AddListener(delegate ()
            {
                userInfoPanel.SetActive(true);
                archevePanel.SetActive(false);
                bagPanel.SetActive(false);
            });

            tableBtn2.onClick.AddListener(delegate ()
            {
                userInfoPanel.SetActive(false);
                archevePanel.SetActive(true);
                bagPanel.SetActive(false);
            });

            tableBtn3.onClick.AddListener(delegate ()
            {
                userInfoPanel.SetActive(false);
                archevePanel.SetActive(false);
                bagPanel.SetActive(true);
            });

            animation = parent.GetComponent<Animation>();
            animation.Play("ViewIn");
            InitUserData();
        }

        public void InitUserData()
        {
            Text text = userInfoPanel.transform.Find("NickName/Text").GetComponent<Text>();
            text.text = PlayerRole.Instance.RoleInfo.RoleMe.GetNickName();

            text = userInfoPanel.transform.Find("GameID/Text").GetComponent<Text>();
            text.text = PlayerRole.Instance.RoleInfo.RoleMe.GetGameID().ToString();

            text = userInfoPanel.transform.Find("Area/Text").GetComponent<Text>();
            text.text = PlayerRole.Instance.RoleInfo.RoleMe.GetIPAddress();

            text = userInfoPanel.transform.Find("Level/Text").GetComponent<Text>();
            text.text = PlayerRole.Instance.RoleInfo.RoleMe.GetLevel().ToString();

            text = userInfoPanel.transform.Find("Archieve/Text").GetComponent<Text>();
            text.text = PlayerRole.Instance.RoleInfo.RoleMe.GetAchievementPoint().ToString();

            text = userInfoPanel.transform.Find("Diamond/Text").GetComponent<Text>();
            text.text = PlayerRole.Instance.RoleInfo.RoleMe.GetGlobel().ToString();

            text = userInfoPanel.transform.Find("Gold/Text").GetComponent<Text>();
            text.text = PlayerRole.Instance.RoleInfo.RoleMe.GetCurrency().ToString();

            text = userInfoPanel.transform.Find("SafeDepositBox/Text").GetComponent<Text>();
            text.text = "0";

        }
    }

}

