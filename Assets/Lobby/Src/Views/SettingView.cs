using System.Collections;
using System.Collections.Generic;
using GF;
using GF.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lobby
{
    public class SettingView : AppView, IPointerClickHandler
    {
        private Animation animation;
        private Slider musicSlider;
        private Slider soundSLider;
        protected override void OnStart()
        {
            Transform background = transform.Find("BackGround").transform;
            animation = background.GetComponent<Animation>();
            animation.Play("ViewIn");

            musicSlider = transform.FindChild("BackGround/MusicSlider").GetComponent<Slider>();
            soundSLider = transform.FindChild("BackGround/SoundSLider").GetComponent<Slider>();
            musicSlider.onValueChanged.AddListener(delegate { MusicChangeCheck(musicSlider.value); });
            soundSLider.onValueChanged.AddListener(delegate { SoundChangeCheck(soundSLider.value); });
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            animation.Play("ViewOut");
            StartCoroutine(OnClose());
        }

        IEnumerator OnClose()
        {
            yield return new WaitForSeconds(0.2f);
            UIManager.Instance.HideView<SettingView>();
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

