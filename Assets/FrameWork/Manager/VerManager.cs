/***********************************************
	FileName: VerManager.cs	    
	Creation: 2017-07-21
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
namespace GF
{
    public struct ResVersion
    {
        public uint ResCrc;
        public uint ResSize;
    }


    public class VerManager : Singleton<VerManager>
    {

        private VerManager()
        {
            LoadVersion();
        }

        public void LoadVersion()
        {
            TextAsset ta = (TextAsset)Resources.Load("Lobby/Config/Version", typeof(TextAsset));
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(ta.text);
            XmlElement ele = doc.DocumentElement;
            string verstr = ele.SelectSingleNode("Params/Version").FirstChild.Value;
            string resverstr = ele.SelectSingleNode("Params/Resource").FirstChild.Value;
            uint editor = uint.Parse(ele.SelectSingleNode("Params/Editor").FirstChild.Value);
            ResVer = verstr;
            ServerSetting.SHOW_PING = uint.Parse(ele.SelectSingleNode("Params/Ping").FirstChild.Value) != 0;
            ServerSetting.RES_VERSION = Utility.VersionToUint(resverstr);
            ServerSetting.ClientVer = Utility.VersionToUint(verstr);
            Resources.UnloadAsset(ta);
        }

        public string ResVer
        {
            set;
            get;
        }
    }
}

