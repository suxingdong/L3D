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

        public void OnPointerClick(PointerEventData eventData)
        {
            UIManager.Instance.HideView<UserInfoView>();
        }


        void Start()
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

        }
    }

}

