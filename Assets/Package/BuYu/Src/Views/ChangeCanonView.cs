/***********************************************
	FileName: ChangeCanonView.cs	    
	Creation: 2017-08-29
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using GF;
using UnityEngine;
using GF.UI;
using Lobby;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BuYu
{
    public class ChangeCanonView : AppView, IPointerClickHandler
    {
        [SerializeField]
        private GridLayoutGroup grid;
        private GameObject itemCanon;
        private SceneModel sceneModel;
        public void OnPointerClick(PointerEventData eventData)
        {
            UIManager.Instance.HideView<ChangeCanonView>();
        }

        void Start()
        {
            sceneModel = ModelManager.Instance.Get<SceneModel>();
            itemCanon = grid.transform.Find("CellBtnCanon").gameObject;
            for (int i = 0; i < 5; i++)
            {
                var item = GameObject.Instantiate(itemCanon);
                item.transform.parent = grid.transform;
                RectTransform rectTransform = grid.GetComponent<RectTransform>();
                var count = grid.transform.childCount-1;
                rectTransform.sizeDelta = new Vector2(count * 360, 400);
                Button btn = item.GetComponent<Button>();
                btn.onClick.AddListener(delegate ()
                {
                    sceneModel.ChangeDestLauncher((byte)i);
                });
            }
            Destroy(itemCanon);
            
            //rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 400);
        }
        
    }

}

