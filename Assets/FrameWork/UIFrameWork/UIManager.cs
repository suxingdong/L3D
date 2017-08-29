/***********************************************
	FileName: UIManager.cs	    
	Creation: 2017-07-10
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GF.UI
{
    public class UIManager: Singleton<UIManager>
    {
        public Dictionary<Type, GameObject> _UIDict = new Dictionary<Type, GameObject>();

        private const string VIEWPATH = "/Prefab/UI/";
        private MessageBoxPanel messageBoxPanel;
        private Transform canvas;
        private Canvas topCanvas;
         private UIManager()
        {
            canvas = GameObject.Find("Canvas").transform;
            //foreach (Transform item in canvas)
            //{
            //    GameObject.Destroy(item.gameObject);
            //}
            canvas.GetComponent<Canvas>().overrideSorting = true;
            canvas.GetComponent<Canvas>().sortingOrder = 0;

            GameObject topUI = GameObject.Find("TopCanvas");
            if (topUI == null)
            {
                GameObject obj = new GameObject("TopCanvas");
                topCanvas = obj.AddComponent<Canvas>();
            }
            else
            {
                topCanvas = topUI.GetComponent<Canvas>();
            }
            GameObject.DontDestroyOnLoad(topCanvas);
            topCanvas.overrideSorting = true;
            topCanvas.sortingOrder = 1;
        }

        public T ShowTopView<T>(object param = null) where T : AppView, new()
        {
            if (_UIDict.ContainsKey(typeof(T)) == false || _UIDict[typeof(T)] == null)
            {
                string path = typeof(T).Namespace.ToString() + VIEWPATH + typeof(T).Name;

                GameObject go = GameObject.Instantiate(Resources.Load<GameObject>(path)) as GameObject;
                go.transform.SetParent(topCanvas.transform, false);
                go.name = typeof(T).Name;
                _UIDict[typeof(T)] = go;
                return go.GetComponent<T>();
            }
            return _UIDict[typeof(T)].GetComponent<T>();
        }

        public T ShowView<T>(object param = null) where T : AppView, new()
        {
            if (_UIDict.ContainsKey(typeof(T)) == false || _UIDict[typeof(T)] == null)
            {
                string path = typeof(T).Namespace.ToString()+VIEWPATH + typeof(T).Name;
                
                GameObject go = GameObject.Instantiate(Resources.Load<GameObject>(path)) as GameObject;
                go.transform.SetParent(canvas, false);
                go.name = typeof(T).Name;                
                _UIDict[typeof(T)] = go;
                return go.GetComponent<T>();
            }
            return _UIDict[typeof(T)].GetComponent<T>();
        }

        public void HideView<T>(object param = null)
        {
            if (_UIDict.ContainsKey(typeof(T)) == true)
            {
                GameObject go = _UIDict[typeof(T)];
                if(go!=null)
                {
                    GameObject.Destroy(go);
                }
                _UIDict.Remove(typeof(T));
            }
        }

        public void CloseView(object param = null)
        {

        }

        public void ShowMessage(string context, MessageBoxEnum.Style style, MessageBoxEnum.OnReceiveMessageBoxResult callback)
        {
            if(messageBoxPanel == null)
            {
                messageBoxPanel = GameObject.FindObjectOfType<MessageBoxPanel>();
                if(messageBoxPanel==null)
                {
                    messageBoxPanel = ResManager.Instance.LoadResComponent<MessageBoxPanel>("Lobby/Prefab/UI/MessageUI");
                }
                messageBoxPanel.transform.SetParent(canvas);
                messageBoxPanel.transform.localPosition = Vector3.zero;
            }
            messageBoxPanel.ShowMessageBox(context, style, callback);
        }
    }
}

