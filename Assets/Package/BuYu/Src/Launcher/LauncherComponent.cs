using UnityEngine;
using System.Collections.Generic;
using GF.UI;
using Lobby;
using UnityEngine.UI;


namespace BuYu
{
    public class LauncherComponent : MonoBehaviour
    {
        public bool m_PlaySpriteAnim;
        public bool m_LightDot;
        public bool m_MoveLight;
        public bool m_MuzzleEft;
        public int m_ComponentCount;
        public string m_MuzzleNamePrefix;                 //炮口特效序列帧名字

        private Button btnGun;

        void Start()
        {
            btnGun = gameObject.transform.Find("Button").GetComponent<Button>();
            btnGun.onClick.AddListener(delegate ()
            {
                Debug.Log("OKKKK");
            });
        }
        public LauncherComponent()
        {
            m_PlaySpriteAnim = false;
        }

    }

}

