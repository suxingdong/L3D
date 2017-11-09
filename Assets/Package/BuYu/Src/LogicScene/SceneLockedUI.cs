using UnityEngine;
using System.Collections.Generic;

namespace BuYu
{
    public class SceneLockedUI : BaseWnd
    {
        float m_LifeTime;

        public void Init()
        {
            m_BaseWndObject = GameObject.Instantiate(SceneRuntime.PlayerMgr.LockedObj) as GameObject;
            m_BaseTrans = m_BaseWndObject.transform;
            m_BaseTrans.SetParent(SceneBoot.Instance.UIPanelTransform, false);

            Vector2 scernPos = new Vector2();
            SceneRuntime.FishMgr.GetFishScreenPos(SceneRuntime.PlayerMgr.LockedFishID, out scernPos);

            m_BaseTrans.position = scernPos;
        }
        
        public void UpdateLockedUI()
        {
            if (SceneRuntime.PlayerMgr.AutoLocked)
            {
                if (m_BaseWndObject != null)
                {
                    if (SceneRuntime.PlayerMgr.LockedFishID == 0)
                        m_BaseWndObject.SetActive(false);
                    else
                    {
                        m_BaseWndObject.SetActive(true);
                        Vector2 scernPos = new Vector2();
                        SceneRuntime.FishMgr.GetFishScreenPos(SceneRuntime.PlayerMgr.LockedFishID, out scernPos);

                        m_BaseTrans.position = scernPos;
                    }
                }
                else
                {
                    Init();
                }
            }
            else
                DestoryUI();

        }
        public void Update(float deltaTime)
        {
            if (m_LifeTime > 0)
            {
                m_LifeTime -= deltaTime;
            }
            else
                DestoryUI();
        }
        public void ShowOtherUserLocked(byte clientSeat)
        {
            m_BaseWndObject = GameObject.Instantiate(SceneRuntime.PlayerMgr.LockedObj) as GameObject;
            m_BaseTrans = m_BaseWndObject.transform;
            m_BaseTrans.SetParent(SceneBoot.Instance.UIPanelTransform, false);
            if (clientSeat < 2)
            {
                m_BaseTrans.position = new Vector3(SceneRuntime.PlayerMgr.GetPlayer(clientSeat).Launcher.LauncherPos.x,
                    SceneRuntime.PlayerMgr.GetPlayer(clientSeat).Launcher.LauncherPos.y + 0.1f, 0);
            }
            else
            {
                m_BaseTrans.position = new Vector3(SceneRuntime.PlayerMgr.GetPlayer(clientSeat).Launcher.LauncherPos.x,
                    SceneRuntime.PlayerMgr.GetPlayer(clientSeat).Launcher.LauncherPos.y - 0.1f, 0);
            }

            m_LifeTime = SkillSetting.SkillDataList[(byte)SkillType.SKILL_LOCK].CDTime;
        }
        public void DestoryUI()
        {
            if (m_BaseWndObject != null)
            {
                GameObject.Destroy(m_BaseWndObject);
                m_BaseWndObject = null;
            }
        }
        public void ShutDown()
        {
            DestoryUI();
        }
    }


}
