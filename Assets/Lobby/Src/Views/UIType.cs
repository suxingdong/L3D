/***********************************************
	FileName: UIType.cs	    
	Creation: 2017-07-12
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
    public class UIType
    {

        public static Dictionary<Type, UIType> Dict = new Dictionary<Type, UIType>();

        public string Path { get; private set; }
        public string Name { get; private set; }

        public UIType(string path)
        {
            Path = path;
            Name = path.Substring(path.LastIndexOf('/') + 1);
        }

        public override string ToString()
        {
            return string.Format("path : {0} name : {1}", Path, Name);
        }

        public static void AddUiType<T>(string filePath) where T : AppView, new()
        {
            if(Dict.ContainsKey(typeof(T)))
            {
                Dict[typeof(Type)] = new UIType(filePath);
            }
            UIType type = new UIType("View/MainMenuView");
        }

    }
}

