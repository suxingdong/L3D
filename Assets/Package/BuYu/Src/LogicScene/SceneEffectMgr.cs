using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using GF;

namespace BuYu
{

    internal class BackgroundEffectData
    {
        public Object EffectObj;
        public Vector3 Pos;
    }

    internal class BackgroundIndexMapping
    {
        public byte[] EffectList;
    }

    public class SceneEffectMgr
    {
        private BackgroundEffectData[] m_BackEffectData;
        private BackgroundIndexMapping[] m_BackEffectMapping;
        private GameObject[] m_LaunchEffectList;
        private Object[] m_FishNetObj = new Object[5];

        private SceneGoldEffect m_GoldEffect = new SceneGoldEffect();
        private float m_Interval;
        private float m_Time;
        private Shader m_FishBlendTexShader;
        private Shader m_FishBlendColorShader;
        private Shader m_FishSimpleShader;
        private Shader m_FishAddTexShader;
        private Shader m_FishDissolveShader;


        private Texture2D m_SkillDisasterTex;
        private Texture2D m_SkillFreezeBaseTex;
        private Texture2D m_SkillFreezeEffectTex;
        private Texture2D m_SkillDissolveTex;
        private Texture2D[] m_SkillFishDeadTexList = new Texture2D[FishSetting.FishDataList.Length];

        private Object[] m_SkillEffectList;
        private Object[] m_LaserSubEffectList;

        private Object m_LightingFishEffect1;
        private Object m_LightingFishEffect2;

        public Texture2D SkillDisasterTex
        {
            get { return m_SkillDisasterTex; }
        }

        public Texture2D SkillFreezeBaseTex
        {
            get { return m_SkillFreezeBaseTex; }
        }

        public Texture2D SkillFreezeEffectTex
        {
            get { return m_SkillFreezeEffectTex; }
        }

        public Texture2D GetLightingTex(byte fishType)
        {
            return m_SkillFishDeadTexList[fishType];
        }

        public Texture2D SkillDissolveTex
        {
            get { return m_SkillDissolveTex; }
        }

        private void InitShaderAndTex()
        {
            m_FishBlendTexShader = Shader.Find("FishShader/BlendTex");
            m_FishBlendColorShader = Shader.Find("FishShader/BlendColor");
            m_FishSimpleShader = Shader.Find("FishShader/Simple");
            m_FishAddTexShader = Shader.Find("FishShader/AddTex");
            m_FishDissolveShader = Shader.Find("FishShader/Dissolve");

            m_SkillDisasterTex =
                (Texture2D)
                    ResManager.Instance.LoadObject("Fire_09_T", "SceneRes/Textures/Effect/Other/", ResType.SceneRes,
                        typeof (Texture2D));
            m_SkillFreezeBaseTex =
                (Texture2D)
                    ResManager.Instance.LoadObject("Other_12_T", "SceneRes/Textures/Effect/Other/", ResType.SceneRes,
                        typeof (Texture2D));
            m_SkillFreezeEffectTex =
                (Texture2D)
                    ResManager.Instance.LoadObject("Other_13_T", "SceneRes/Textures/Effect/Other/", ResType.SceneRes,
                        typeof (Texture2D));
            m_SkillDissolveTex =
                (Texture2D)
                    ResManager.Instance.LoadObject("Other_19_T", "SceneRes/Textures/Effect/Other/", ResType.SceneRes,
                        typeof (Texture2D));
            for (int i = 0; i < m_SkillFishDeadTexList.Length; ++i)
            {
                m_SkillFishDeadTexList[i] =
                    (Texture2D)
                        ResManager.Instance.LoadObject("FishMask" + i, "SceneRes/Textures/Effect/MaskTex/",
                            ResType.SceneRes, typeof (Texture2D));
            }
        }

        private void LoadSkillEffect()
        {
            m_SkillEffectList = new Object[10];
            m_LaserSubEffectList = new Object[3];
            for (int i = 0; i < m_SkillEffectList.Length; ++i)
                m_SkillEffectList[i] = ResManager.Instance.LoadObject("skill" + i, "SceneRes/Prefab/Effect/Skill/",
                    ResType.SceneRes);

            for (int i = 0; i < m_LaserSubEffectList.Length; ++i)
                m_LaserSubEffectList[i] = ResManager.Instance.LoadObject("HitPoint0" + i,
                    "SceneRes/Prefab/Effect/Launcher/HitPoint/", ResType.SceneRes);

            m_LightingFishEffect1 = ResManager.Instance.LoadObject("Ef_thunderFishMain",
                "SceneRes/Prefab/Effect/ThunderFish/", ResType.SceneRes);
            m_LightingFishEffect2 = ResManager.Instance.LoadObject("Ef_thunderFishLight",
                "SceneRes/Prefab/Effect/ThunderFish/", ResType.SceneRes);
        }

        public GameObject GetLightingFishEffect(bool main)
        {
            if (main)
                return (GameObject) GameObject.Instantiate(m_LightingFishEffect1);
            else
                return (GameObject) GameObject.Instantiate(m_LightingFishEffect2);
        }

        public GameObject GetSkillEffect(ushort idx)
        {
            return (GameObject) GameObject.Instantiate(m_SkillEffectList[idx]);
        }

        public int LaserSubEffectCount
        {
            get { return m_LaserSubEffectList.Length; }
        }

        public GameObject GetLaserSubEffect(ushort idx)
        {
            return (GameObject) GameObject.Instantiate(m_LaserSubEffectList[idx]);
        }

        private void LoadBackEffect()
        {
            TextAsset ta =
                (TextAsset)
                    ResManager.Instance.LoadObject("BackEffectSetting", "BuYu/SceneRes/BackEffectSetting/", ResType.SceneRes,
                        typeof (TextAsset));
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(ta.text);
            XmlElement ele = doc.DocumentElement;
            XmlNode effectNodes = ele.SelectSingleNode("EffectList");
            m_BackEffectData = new BackgroundEffectData[effectNodes.ChildNodes.Count];
            for (int i = 0; i < m_BackEffectData.Length; ++i)
            {
                XmlNode node = effectNodes.ChildNodes[i];
                int id = int.Parse(node.Attributes["id"].Value);
                if (m_BackEffectData[id] != null)
                {
                    Debug.Log("存在相同的特效ID:" + id);
                }
                BackgroundEffectData effect = new BackgroundEffectData();
                effect.EffectObj = ResManager.Instance.LoadObject(node.Attributes["path"].Value,
                    "SceneRes/BackEffectSetting/", ResType.SceneRes);
                effect.Pos = new Vector3(
                    float.Parse(node.Attributes["x"].Value),
                    float.Parse(node.Attributes["y"].Value),
                    float.Parse(node.Attributes["z"].Value));
                m_BackEffectData[id] = effect;
            }

            effectNodes = effectNodes.NextSibling;
            int backCount = effectNodes.ChildNodes.Count;
            m_BackEffectMapping = new BackgroundIndexMapping[backCount];
            for (int i = 0; i < backCount; ++i)
            {
                XmlNode curNode = effectNodes.ChildNodes[i];
                byte index = byte.Parse(curNode.Attributes["id"].Value);
                if (m_BackEffectMapping[index] != null)
                {
                    Debug.Log("存在相同的背景索引:" + index);
                }
                BackgroundIndexMapping backmapping = new BackgroundIndexMapping();
                m_BackEffectMapping[index] = backmapping;
                int count = curNode.ChildNodes.Count;
                if (count > 0)
                {
                    backmapping.EffectList = new byte[count];
                    for (int j = 0; j < curNode.ChildNodes.Count; ++j)
                    {
                        byte idx = byte.Parse(curNode.ChildNodes[j].Attributes["id"].Value);
                        if (idx >= m_BackEffectData.Length)
                        {
                            Debug.Log("不存在的特效索引:" + idx);
                        }
                        backmapping.EffectList[j] = idx;
                    }
                }
            }
            ResManager.Instance.UnloadObject(ta);
        }

        public void LoadBackEffect(int backidx)
        {
            if (m_BackEffectMapping[backidx] != null)
            {
                BackgroundIndexMapping backmapping = m_BackEffectMapping[backidx];
                m_LaunchEffectList = new GameObject[backmapping.EffectList.Length];
                for (int i = 0; i < m_LaunchEffectList.Length; ++i)
                {
                    int idx = backmapping.EffectList[i];
                    GameObject EffectObj = (GameObject) GameObject.Instantiate(m_BackEffectData[idx].EffectObj);
                    EffectObj.transform.localPosition = m_BackEffectData[idx].Pos;
                    m_LaunchEffectList[i] = EffectObj;
                }
            }
        }

        public void Init()
        {
            LoadSkillEffect();
            InitShaderAndTex();
            m_GoldEffect.Init();
            for (byte i = 0; i < m_FishNetObj.Length; ++i)
            {
                string name = "LauncherHit" + i.ToString();
                m_FishNetObj[i] = ResManager.Instance.LoadObject(name, "SceneRes/Prefab/Effect/Launcher/Hit/",
                    ResType.SceneRes);
            }
            LoadBackEffect();
        }

        public void Update(float delta)
        {
            m_GoldEffect.Update(delta);
        }

        public void ClearBackEffect()
        {
            if (m_LaunchEffectList != null)
            {
                for (int i = 0; i < m_LaunchEffectList.Length; ++i)
                    GameObject.Destroy(m_LaunchEffectList[i]);
                m_LaunchEffectList = null;
            }
        }

        public void Shutdown()
        {
            m_GoldEffect.Shutdown();
            if (m_LaunchEffectList != null)
            {
                for (int i = 0; i < m_LaunchEffectList.Length; ++i)
                    GameObject.Destroy(m_LaunchEffectList[i]);
            }
            for (byte i = 0; i < m_FishNetObj.Length; ++i)
                if (m_FishNetObj[i] != null)
                    ResManager.Instance.UnloadObject(m_FishNetObj[i]);

            ResManager.Instance.UnloadObject(m_LightingFishEffect1);
            ResManager.Instance.UnloadObject(m_LightingFishEffect2);
        }

        public Shader FishBlendTexShader
        {
            get { return m_FishBlendTexShader; }
        }

        public Shader FishBlendColorShader
        {
            get { return m_FishBlendColorShader; }
        }

        public Shader FishDissolveShader
        {
            get { return m_FishDissolveShader; }
        }

        public Shader FishSimpleShader
        {
            get { return m_FishSimpleShader; }
        }

        public Shader FishAddTexShader
        {
            get { return m_FishAddTexShader; }
        }

        //鱼被捕获调用接口，弹出金币
        public void FishCatched(Fish fish, CatchedData cd)
        {
            if (fish.IsBossFish())
            {
                //GlobalAudioMgr.Instance.PlayOrdianryMusic(Audio.OrdianryMusic.catch_bossfish);
            }
            m_GoldEffect.ShowGoldEffect(cd, fish);
            //KonnoTool.ShowBossCatched(SceneRuntime.WorldToNGUI(fish.Position), cd.GoldNum);

            if (fish.IsBossFish())
            {
                //BossFish彩蛋特效
                m_GoldEffect.ShowBossFishSpecailEft(cd, fish);
            }
            //卡片掉落
            ushort nReward = fish.GetDropReward();
            if (nReward != 0)
            {
                tagRewardOnce pReward;
                if (!FishConfig.Instance.m_RewardConfig.RewardMap.TryGetValue(nReward, out pReward))
                {
                    return;
                }
                for (int i = 0; i < pReward.RewardItemVec.Count; i++)
                {
                    tagItemConfig itemConfig;
                    uint uItemid = pReward.RewardItemVec[i].ItemID;
                    if (!FishConfig.Instance.m_ItemInfo.m_ItemMap.TryGetValue(uItemid, out itemConfig))
                    {
                        continue;
                    }
                    if (itemConfig.ItemTypeID == EItemType.IT_Currey) //钻石特殊处理
                    {
                        m_GoldEffect.ShowDiamond(cd, fish);
                    }
                    else //其他卡片
                    {
                        if (SceneRuntime.SceneLogic.PlayerMgr.GetPlayer(cd.ClientSeat) == null)
                            continue;
                        Vector2 vecPos = SceneRuntime.SceneLogic.PlayerMgr.GetPlayer(cd.ClientSeat).Launcher.LauncherPos;
                        /*SceneRuntime.SceneLogic.LogicUI.MoveDropCard(SceneRuntime.WorldToNGUI(fish.Position),
                            new Vector3(vecPos.x, vecPos.y, 0), (ushort) uItemid);*/
                    }
                }
            }

        }

        public void ShowUnLockRateReward(Vector3 pos)
        {
            m_GoldEffect.ShowUnLockRateReward(pos);
        }

        public void ShowGetGoldNum(int GoldNum, byte ClientSeat)
        {
            m_GoldEffect.ShowGetGoldNum(GoldNum, ClientSeat);
        }

        public void Clear()
        {
            m_GoldEffect.Clear();
        }

        public void PlayFishNet(Vector3 bulletPos, byte bulletType)
        {
            if (m_FishNetObj[bulletType] != null)
            {
                GameObject Gobj = ((GameObject) GameObject.Instantiate(m_FishNetObj[bulletType]));
                Gobj.transform.localPosition = bulletPos;
                GlobalEffectData effect = new GlobalEffectData(Gobj, 0, 1.5f);
                GlobalEffectMgr.Instance.AddEffect(effect);
            }
        }

        public bool HaveFlyGold(byte bySeat)
        {
            return m_GoldEffect.HaveFlyGold(bySeat);
        }
    }
}