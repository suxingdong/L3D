/***********************************************
	FileName: KnapsackItem.cs	    
	Creation: 2017-09-01
	Author：East.Su
	Version：V1.0.0
	Desc: 背包物品Item
**********************************************/

using System.Collections;
using System.Collections.Generic;
using GF;
using UnityEngine;
using UnityEngine.UI;
namespace Lobby
{

    public class KnapsackItem
    {
        public GameObject m_BaseWndObject;
        public Transform m_BaseTrans;

        Image m_ItemIcon;
        Text m_ItemNum;
        Button btnItem;
        uint m_ItemID = 0;
        uint m_ItemOnlyID;
        EItemType m_ItemType;

        public uint ItemID
        {
            get { return m_ItemID; }
        }


        public void Init(GameObject go)
        {
            m_BaseWndObject = GameObject.Instantiate(go) as GameObject;
            m_BaseWndObject.SetActive(true);
            m_BaseTrans = m_BaseWndObject.transform;
            if (m_BaseWndObject.activeSelf != true)
                m_BaseWndObject.SetActive(true);
            m_ItemIcon = m_BaseTrans.FindChild("Icon").GetComponent<Image>();
            m_ItemNum = m_BaseTrans.FindChild("Num").GetComponent<Text>();
            btnItem = m_BaseTrans.GetComponent<Button>();
            btnItem.onClick.AddListener(OnShowDescTips);
        }

        public void OnShowDescTips()
        {
            
        }

        public void ResetLoaclScale()
        {
            m_BaseTrans.localScale = Vector3.one;
        }

        public void SetItemInfo(uint itemID, uint itemNum, string iconName, uint OnlyItemID, EItemType type)
        {
            m_ItemIcon.gameObject.SetActive(true);
            m_ItemNum.gameObject.SetActive(true);
            m_ItemID = itemID;
            m_ItemOnlyID = OnlyItemID;
            m_ItemType = type;
            m_ItemIcon.sprite = ResManager.Instance.LoadSprite("BuYu/Texture/GoodsIcon/"+iconName);
            m_ItemNum.text = itemNum.ToString();
        }

    

    }

}
