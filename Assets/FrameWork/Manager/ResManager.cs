/***********************************************
	FileName: ResManager.cs	    
	Creation: 2017-07-12
	Author：East.Su
	Version：V1.0.0
	Desc: 资源加载辅助类
**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GF
{
    //不要改动枚举变量名称，与Version.xml中一致
    public enum ResType
    {
        GlobalRes,         //全局配置相关资源           \
        FishRes,           //鱼资源                    -- 全局加载      
        PathRes,           //路径资源                   /

        LogonRes,          //登陆资源
        HallRes,           //大厅资源
        SceneRes,          //场景资源

        ExtraRes,          //附加资源
        GameRes,           //游戏资源
        MAX
    }
    public enum ResLocation
    {
        StreamingAsset,
        Persistent,
        Resources,
        MAX,
    }

    public class ResManager :Singleton<ResManager>
    {
        //开发选项，是否使用Editor/Resources资源，“是”不进行更新。
#if UNITY_EDITOR
        public static bool ENABLE_RESOURCES = CheckEnableRes();
#else
    public static bool ENABLE_RESOURCES = false;
#endif

        public const ushort RES_NUM = (int)ResType.MAX;
        private VerManager _mVerMgr = VerManager.Instance;
        Dictionary<int, Object> m_LoadObjList = new Dictionary<int, Object>();
        AssetBundle[] m_AssetList = new AssetBundle[RES_NUM];
        bool[] m_AssetManager = new bool[RES_NUM];
        bool m_bLoadedRes;
        public static bool CheckEnableRes()
        {
            return System.IO.Directory.Exists(Application.dataPath + "/Resources/FishRes");
        }

        private ResManager()
        {

        }
        public T LoadResComponent<T>(string path) 
        {
            GameObject tObj = LoadRes(path);
            return tObj.GetComponent<T>();
        }

        public GameObject LoadRes(string path)
        {
            Object resPreb = Resources.Load(path, typeof(GameObject));
            GameObject tObj = GameObject.Instantiate(resPreb) as GameObject;
            //tObj.transform.SetParent(canv.transform, false);
            return tObj;
        }

        public Object LoadObject(string id, string path, ResType res, bool bManager = true)
        {

            Object obj;
            int idx = (int)res;
            if (ENABLE_RESOURCES || m_AssetList[idx] == null)
            {
                obj = Resources.Load(path + id);
            }
            else
            {
                //if (m_bLoadedRes == false)
                //{
                //    LogMgr.Log("资源加载必须在Init中进行, id:" + id + ", path:" + path);
                //}
                obj = m_AssetList[idx].LoadAsset(id);
#if !UNITY_EDITOR
            if (bManager && m_AssetManager[idx] && obj != null)
            {
                //m_LoadObjList.Add(obj.GetHashCode(), obj);
            }
#endif
            }
            if (obj == null && res != ResType.FishRes)
                LogMgr.Log("加载失败, id:" + id + ", path:" + path);
            return obj;
        }
        public Object LoadObject(string id, string path, ResType res, System.Type type, bool bManager = true)
        {
            Object obj;
            int idx = (int)res;
            if (ENABLE_RESOURCES || m_AssetList[idx] == null)
            {
                obj = Resources.Load(path + id, type);
            }
            else
            {
                //if (m_bLoadedRes == false)
                //{
                //    LogMgr.Log("资源加载必须在Init中进行, id:" + id + ", path:" + path);
                //}
                obj = m_AssetList[idx].LoadAsset(id, type);
#if !UNITY_EDITOR
            if (bManager && m_AssetManager[idx] && obj != null)
            {
                //m_LoadObjList.Add(obj.GetHashCode(), obj);
            }
#endif
            }
            if (obj == null && res != ResType.FishRes && res != ResType.PathRes)
                LogMgr.Log("加载失败, id:" + id + ", path:" + path);
            return obj;
        }

        public void UnloadObject(Object obj)
        {
#if !UNITY_EDITOR
        if(ENABLE_RESOURCES == false)
        {
            //m_LoadObjList.Remove(obj.GetHashCode());
            //Resources.UnloadAsset(obj);
        }
#endif
        }


    }
}

