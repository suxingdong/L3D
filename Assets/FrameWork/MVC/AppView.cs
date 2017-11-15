/***********************************************
	FileName: AppView.cs	    
	Creation: 2017-07-07
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GF
{
    using System;
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

    public class AppView : MonoBehaviour
    {
        public static GameObject UIRoot;

        private static Dictionary<string, AppView> CacheView = new Dictionary<string, AppView>();

        protected Dictionary<string, EventDelegate>  eventList = new Dictionary<string, EventDelegate>();
        #region 底层方法

        //public static AppView ShowView(Type viewType, AppView msg, CallBack<AppView> loadViewComplete)
        //{
        //    if (CacheView.ContainsKey(viewType.Name))
        //    {
        //        CloseView(viewType.Name);
        //    }
        //    GameObject obj = (GameObject)Resources.Load(Paths.getUIPath(viewType.Name));
        //    GameObject inst = NGUITools.AddChild(NGUIRoot, obj);
        //    BaseView view = (BaseView)inst.AddComponent(viewType);
        //    loadViewComplete(view);
        //    CacheView.Add(viewType.Name, view);
        //    return view;
        //}
        
        public static void CloseView(string name)
        {
            CacheView[name].OnClose();
            Destroy(CacheView[name].gameObject);
            CacheView.Remove(name);
        }

        public static void ClearAllView()
        {
            foreach (var v in CacheView)
            {
                Destroy(v.Value.gameObject);
            }
            CacheView.Clear();
        }

        public static void DisableView(string name)
        {

        }



        public void Close()
        {
            CacheView[GetType().Name].OnClose();
            Destroy(CacheView[GetType().Name].gameObject);
            CacheView.Remove(GetType().Name);
        }

        #endregion

        #region Override Mono
        void Awake()
        {
            OnAwake();
        }

        void Start()
        {
            OnStart();

            OnShow();
        }

        void Update()
        {
            OnUpdate(Time.deltaTime);
        }

        public void ViewVisible(bool b)
        {
            gameObject.SetActive(b);
            OnViewVisible(b);
        }



        protected virtual void OnAwake()
        {

        }
        protected virtual void OnStart()
        {
        }

        protected virtual void OnShow()
        {

        }

        protected virtual void OnUpdate(float time)
        {

        }

        protected virtual void OnClose()
        {

        }

        public virtual void OnViewVisible(bool b)
        {

        }

        protected virtual void OnDestroy()
        {
            foreach (var var in eventList)
            {
                EventManager.Instance.RemoveEventListener(var.Key, var.Value);
            }
        }
        //动画回调
        protected virtual void OnAnimationComplete(string nam)
        {
            
        }
        public virtual void OnParams(object param)
        {
            
        }

        protected virtual void _RegisterEvent(string aEventName_string, EventDelegate aEventDelegate)
        {
            eventList[aEventName_string] = aEventDelegate;
            EventManager.Instance.AddEventListener(aEventName_string, aEventDelegate);
        }
        #endregion

        #region 查找子部件
        protected virtual T find<T>(string name) where T : Component
        {
            return find<T>(transform, name);
        }
        protected virtual T find<T>(GameObject parent, string name) where T : Component
        {
            return find<T>(parent.transform, name);
        }
        protected virtual T find<T>(Transform parent, string name) where T : Component
        {
            T rt = null;
            Transform transf = parent.Find(name);
            if (transf != null)
            {
                rt = transf.GetComponent<T>();
            }
            return rt;
        }
        protected virtual GameObject find(string name)
        {
            return find(transform, name);
        }
        protected virtual GameObject find(GameObject parent, string name)
        {
            return find(parent.transform, name);
        }
        protected virtual GameObject find(Transform parent, string name)
        {
            GameObject rt = null;
            Transform transf = parent.Find(name);
            if (transf != null)
            {
                rt = transf.gameObject;
            }
            return rt;
        }
        #endregion
    }


}
