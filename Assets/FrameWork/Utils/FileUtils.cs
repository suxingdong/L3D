/***********************************************
	FileName: FileUtils.cs	    
	Creation: 2017-07-12
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace GF
{
    public class FileUtils : Singleton<FileUtils>
    {
#if UNITY_EDITOR
        public static string persistPath = UnityEngine.Application.persistentDataPath;
        //persistPath = string.Format(@"Assets/StreamingAssets/{0}", fileName);
#elif UNITY_ANDROID
            public static string persistPath  = "jar:file://" + Application.dataPath + "!/assets";
#elif UNITY_IOS
            public static string persistPath =  Application.dataPath + "/Raw";
#else
            public static string persistPath = UnityEngine.Application.persistentDataPath;
            //persistPath = Application.dataPath + "/StreamingAssets/" + fileName;  //UNITY_WINRT, UNITY_WP8 in this path
#endif
        private FileUtils()
        {

        }

        public string GetAppPath(string fileName)
        {
            return string.Format(@"{0}/{1}", persistPath, fileName);

            //安卓可能需要www去load
            //          WWW loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + LOCAL_DB_NAME); 
            //          while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended,
            //          File.WriteAllBytes(filepath, loadDb.bytes);
        }

        //写入文件
        public void WriteFile(string fileName, byte[] datas)
        {
            string persistPath = GetAppPath(fileName);
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = new FileStream(persistPath, FileMode.Create, FileAccess.Write))
            {
                bf.Serialize(fs, datas);
            }
        }


        public  bool isExists(string fileName)
        {
            string persistPath = GetAppPath(fileName);
            FileInfo file = new FileInfo(persistPath);
            if (file.Exists)
            {
                return true;
            }
            return false;
        }
        //读取文件
        public byte[] ReadFile(string fileName)
        {
            string persistPath = GetAppPath(fileName);
            BinaryFormatter bf = new BinaryFormatter();
            FileInfo file = new FileInfo(persistPath);
            Debug.Log(persistPath);
            if (file.Exists)
            {
                Debug.Log("find file successfully!!");
                using (FileStream fs = new FileStream(persistPath, FileMode.Open, FileAccess.Read))
                {
                    byte[] data = (Byte[])bf.Deserialize(fs);
                    return data;
                }
            }
            return null;
        }


        public string LoadResFile(string fileName)
        {
            TextAsset ta = (TextAsset)Resources.Load(fileName, typeof(TextAsset));
            if (ta != null)
                return ta.text;
            else
                return "";               
        }

        

       
        /// 清空指定的文件夹，但不删除文件夹 
        /// </summary> 
        /// <param name="dir"></param> 
        public void DeleteFolder(string dir)
        {
            foreach (string d in Directory.GetFileSystemEntries(dir))
            {
                if (File.Exists(d))
                {
                    FileInfo fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(d);//直接删除其中的文件   
                }
                else
                {
                    DirectoryInfo d1 = new DirectoryInfo(d);
                    if (d1.GetFiles().Length != 0)
                    {
                        DeleteFolder(d1.FullName);////递归删除子文件夹 
                    }
                    Directory.Delete(d);
                }
            }
        }

    }
}



