using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GF;
using GF.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lobby
{
    public class MusicSettingView : AppView, IPointerClickHandler
    {
        private Animation animation;
        private Slider musicSlider;
        private Slider soundSLider;
        protected override void OnStart()
        {
            Transform background = transform.Find("BackGround").transform;
            Tweener tweener = background.DOLocalMoveX(450, 0.3f);
            tweener.SetUpdate(true);
            tweener.SetEase(Ease.OutBack);

            musicSlider = transform.FindChild("BackGround/MusicSlider").GetComponent<Slider>();
            soundSLider = transform.FindChild("BackGround/SoundSLider").GetComponent<Slider>();
            musicSlider.onValueChanged.AddListener(delegate { MusicChangeCheck(musicSlider.value); });
            soundSLider.onValueChanged.AddListener(delegate { SoundChangeCheck(soundSLider.value); });
        }

        public void OnComplete()
        {
            UIManager.Instance.HideView<MusicSettingView>();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            Transform background = transform.Find("BackGround").transform;
            Tweener tweener = background.DOLocalMoveX(1665, 0.4f);
            tweener.SetUpdate(true);
            tweener.SetEase(Ease.InBack);
            tweener.OnComplete(OnComplete);
        }
        

        public void MusicChangeCheck(float value)
        {
            Debug.Log("music = "+value);
        }


        public void SoundChangeCheck(float value)
        {
            Debug.Log("sound = " + value);
        }

    }

}

