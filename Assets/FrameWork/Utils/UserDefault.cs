/***********************************************
	FileName: UserDefault.cs	    
	Creation: 2017-07-12
	Author：East.Su
	Version：V1.0.0
	Desc: 保存数据到本地
**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

namespace GF
{


    public class UserDefault : Singleton<UserDefault>
    {
        // root name of xml

        const string USERDEFAULT_ROOT_NAME  = "userDefaultRoot";
        const string XML_FILE_NAME = "UserDefault.xml";
        private XmlDocument xmlDoc = null;
        private XmlNode rootNode = null;

        private UserDefault()
        {
            if (!IsXMLFileExist())
            {
                if (!CreateXMLFile())
                {

                }
            }
            LoadXMLFile();
        }

        void InitXMLFilePath()
        {

        }

        public bool GetBoolForKey(string pKey, bool defaultValue=false)
        {
            XmlNode tNode = XmlUtils.Instance.GetXMLNodeForKey(xmlDoc, rootNode, pKey);
            string value = "";
            bool ret = defaultValue;
            if (tNode != null)
            {
                value = tNode.InnerText;
                ret = (string.Compare(value, "true") == 0);
            }   
            return ret;
        }

       

        public int GetIntegerForKey(string pKey, int defaultValue = 0)
        {
            XmlNode tNode = XmlUtils.Instance.GetXMLNodeForKey(xmlDoc, rootNode, pKey);
            string value = "";
            int ret = defaultValue;
            if (tNode != null)
            {
                value = tNode.InnerText;
                return int.Parse(value);
            }
            return ret;
        }

        public float GetFloatForKey(string pKey, float defaultValue = 0)
        {
            XmlNode tNode = XmlUtils.Instance.GetXMLNodeForKey(xmlDoc, rootNode, pKey);
            string value = "";
            float ret = defaultValue;
            if (tNode != null)
            {
                value = tNode.InnerText;
                return float.Parse(value);
            }
            return ret;
        }
        

        public double GetDoubleForKey(string pKey, float defaultValue = 0)
        {
            XmlNode tNode = XmlUtils.Instance.GetXMLNodeForKey(xmlDoc, rootNode, pKey);
            string value = "";
            double ret = defaultValue;
            if (tNode != null)
            {
                value = tNode.InnerText;
                return float.Parse(value);
            }
            return ret;
        }

        public string GetStringForKey(string pKey, string  defaultValue = "")
        {
            XmlNode tNode = XmlUtils.Instance.GetXMLNodeForKey(xmlDoc, rootNode, pKey);
            string value = "";
            string ret = defaultValue;
            if (tNode != null)
            {
                value = tNode.InnerText;
                return value;
            }
            return ret;
        }

        public void SetBoolForKey(string pKey, bool value)
        {
            if (true == value)
            {
                SetStringForKey(pKey, "true");
            }
            else
            {
                SetStringForKey(pKey, "false");
            }
        }



        public void SetIntegerForKey(string pKey, int value)
        {
            SetStringForKey(pKey, Convert.ToString(value));
        }


        public void SetFloatForKey(string pKey, float value)
        {
            SetStringForKey(pKey, Convert.ToString(value));
        }

        public  void SetDoubleForKey(string pKey, double value)
        {
            SetStringForKey(pKey, Convert.ToString(value));
        }
  
        public void SetStringForKey(string pKey, string pValue)
        {
            XmlNode tNode = XmlUtils.Instance.GetXMLNodeForKey(xmlDoc, rootNode, pKey);
            if (tNode != null)
            {
                if (tNode.FirstChild != null)
                {
                    tNode.FirstChild.Value = pValue;
                }
                else
                {
                    XmlUtils.Instance.UpdateNodeToXML(tNode, pKey, pValue);
                }
            }
            else
            {
                if (rootNode != null)
                {
                    XmlUtils.Instance.AddNodeToXML(xmlDoc, rootNode, pKey, pValue);
                }
            }
            XmlUtils.Instance.SaveXmlFile(xmlDoc, XML_FILE_NAME);
        }
        
        public void Flush()
        {

        }

        public static string GetXMLFilePath()
        {
            return FileUtils.Instance.GetAppPath(XML_FILE_NAME); ;
        }

        public static bool IsXMLFileExist()
        {
            return FileUtils.Instance.isExists(XML_FILE_NAME);
        }

        public static bool CreateXMLFile()
        {
            string path = FileUtils.Instance.GetAppPath(XML_FILE_NAME);
            return XmlUtils.Instance.CreateXmlFile(path, USERDEFAULT_ROOT_NAME);
        }

        public void LoadXMLFile()
        {
            xmlDoc = XmlUtils.Instance.LoadXMLFile(XML_FILE_NAME);
            rootNode = xmlDoc.SelectSingleNode(USERDEFAULT_ROOT_NAME);
        }
    }
}

