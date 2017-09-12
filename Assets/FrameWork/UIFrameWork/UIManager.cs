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
using UnityEngine.UI;

namespace GF.UI
{
    public class UIManager: Singleton<UIManager>
    {
        public Dictionary<Type, GameObject> _UIDict = new Dictionary<Type, GameObject>();

        private const string VIEWPATH = "/Prefab/UI/";
        private MessageBoxPanel messageBoxPanel;
        private Transform canvas;
         private UIManager()
        {
            canvas = GameObject.Find("Canvas").transform;
            //foreach (Transform item in canvas)
            //{
            //    GameObject.Destroy(item.gameObject);
            //}
            canvas.GetComponent<Canvas>().overrideSorting = true;
            canvas.GetComponent<Canvas>().sortingOrder = 0;

            GameObject ui = GameObject.Find("TopCanvas");
            if (ui == null)
            {
                GameObject obj = new GameObject("TopCanvas");
                TopCanvas = obj.AddComponent<Canvas>();
            }
            else
            {
                TopCanvas = ui.GetComponent<Canvas>();
            }

            ui = GameObject.Find("BottomCanvas");
            if (ui == null)
            {
                GameObject obj = new GameObject("BottomCanvas");
                BottomCanvas = obj.AddComponent<Canvas>();
            }
            else
            {
                BottomCanvas = ui.GetComponent<Canvas>();
            }

            GameObject.DontDestroyOnLoad(TopCanvas);
            GameObject.DontDestroyOnLoad(BottomCanvas);
            TopCanvas.overrideSorting = true;
            TopCanvas.sortingOrder = 1;
            BottomCanvas.overrideSorting = true;
            BottomCanvas.sortingOrder = -1;
        }

        public Vector3 WordToScenePoint(Vector3 pos)
        {
            CanvasScaler canvasScaler = canvas.GetComponent<CanvasScaler>();
            float resolutionX = canvasScaler.referenceResolution.x;
            float resolutionY = canvasScaler.referenceResolution.y;
            float offect = (Screen.width / canvasScaler.referenceResolution.x) * (1 - canvasScaler.matchWidthOrHeight) + (Screen.height / canvasScaler.referenceResolution.y) * canvasScaler.matchWidthOrHeight;
            Vector2 a = RectTransformUtility.WorldToScreenPoint(Camera.main, pos);
            return new Vector3(a.x / offect, a.y / offect, 0);

            //return canvas.worldCamera.ScreenToWorldPoint(v_v3);
        }

        public T ShowTopView<T>(object param = null) where T : AppView, new()
        {
            if (_UIDict.ContainsKey(typeof(T)) == false || _UIDict[typeof(T)] == null)
            {
                string path = typeof(T).Namespace.ToString() + VIEWPATH + typeof(T).Name;

                GameObject go = GameObject.Instantiate(Resources.Load<GameObject>(path)) as GameObject;
                go.transform.SetParent(TopCanvas.transform, false);
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

        public Canvas BottomCanvas { get; private set; }
        public Canvas TopCanvas { get; private set; }
    }
}

