/***********************************************
	FileName: IllustratView.cs	    
	Creation: 2017-09-14
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using GF;
using GF.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BuYu
{
    public class IllustratView : AppView, IPointerClickHandler
    {
        private Animation animation;

        public void OnPointerClick(PointerEventData eventData)
        {
            animation.Play("ViewOut");
            StartCoroutine(OnClose());
        }

        IEnumerator OnClose()
        {
            yield return new WaitForSeconds(0.2f);
            UIManager.Instance.HideView<IllustratView>();
        }

    }

}

