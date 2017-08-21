/***********************************************
	FileName: XmlUtils.cs	    
	Creation: 2017-07-13
	Author：East.Su
	Version：V1.0.0
	Desc: XML文件辅助文档
**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;


namespace GF
{
    public class XmlUtils : Singleton<XmlUtils>
    {

        private XmlUtils()
        {

        }

        public bool CreateXmlFile(string filePath, string rootName = "XmlRoot")
        {
            XmlDocument xmlDoc = new XmlDocument();
            if (null == xmlDoc)
            {
                return false;
            }
            XmlDeclaration xmlDec = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDoc.AppendChild(xmlDec);
            xmlDoc.AppendChild(xmlDoc.CreateElement(rootName));
            xmlDoc.Save(filePath);
            return true;
        }

        public XmlDocument LoadXMLFile(string fileName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(FileUtils.Instance.GetAppPath(fileName));
            return xmlDoc;
        }

        public void SaveXmlFile(XmlDocument xmlDoc, string fileName)
        {
            string filePath = FileUtils.Instance.GetAppPath(fileName);
            xmlDoc.Save(filePath);
        }


        public void AddNodeToXML(XmlDocument xmlDoc, XmlNode root, string titleValue, string infoValue)
        {
            XmlElement element = xmlDoc.CreateElement(titleValue);
            element.InnerText = infoValue;
            root.AppendChild(element);
        }

        public void UpdateNodeToXML(XmlNode curNode, string titleValue, string infoValue)
        {
            curNode.Value = infoValue;
        }
        //public void AddNodeToXML(XmlDocument xmlDoc, XmlNode root, string titleValue, string infoValue )
        //{
        //    XmlElement element = xmlDoc.CreateElement("Node");
        //    element.SetAttribute("Type", "string");
        //    XmlElement titleElelment = xmlDoc.CreateElement("Title");
        //    //titleElelment.SetAttribute("Title", TitleValue);  
        //    titleElelment.InnerText = titleValue;

        //    XmlElement infoElement = xmlDoc.CreateElement("Info");
        //    //infoElement.SetAttribute("Info", infoValue);  
        //    infoElement.InnerText = infoValue;

        //    element.AppendChild(titleElelment);
        //    element.AppendChild(infoElement);
        //    root.AppendChild(element);
        //}

        //void UpdateNodeToXML()
        //{
        //    string filepath = FileUtils.Instance.GetAppPath("");
        //    if (FileUtils.Instance.isExists(filepath))
        //    {
        //        XmlDocument xmldoc = new XmlDocument();
        //        xmldoc.Load(filepath);  //根据指定路径加载xml  
        //        XmlNodeList nodeList = xmldoc.SelectSingleNode("Root").ChildNodes; //Node节点  
        //                                                                           //遍历所有子节点  
        //        foreach (XmlElement xe in nodeList)
        //        {
        //            //拿到节点中属性Type=“string”的节点  
        //            if (xe.GetAttribute("Type") == "string")
        //            {
        //                //更新节点属性  
        //                xe.SetAttribute("type", "text");
        //                //继续遍历  
        //                foreach (XmlElement xelement in xe.ChildNodes)
        //                {
        //                    if (xelement.Name == "TitleNode")
        //                    {
        //                        //修改节点名称对应的数值，而上面的拿到节点连带的属性  
        //                        //xelement.SetAttribute("Title", "企业简介");  
        //                        xelement.InnerText = "企业简介";
        //                    }
        //                }
        //                break;
        //            }
        //        }
        //        xmldoc.Save(filepath);
        //    }
        //}

        public XmlNode GetXMLNodeForKey(XmlDocument xmlDoc, XmlNode rootNode, string pKey )
        {
            XmlNode curNode = null;
            do
            {
                curNode = rootNode.FirstChild;
                while (null != curNode)
                {
                    string nodeName = curNode.Name;
                    if (string.Compare(nodeName, pKey) == 0)
                    {
                        break;
                    }
                    curNode = curNode.NextSibling;
                }
            } while (false);
            return curNode;
        }
    }
}

