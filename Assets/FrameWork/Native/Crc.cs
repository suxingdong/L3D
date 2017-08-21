/***********************************************
	FileName: Crc.cs	    
	Creation: 2017-07-25
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace GF
{
    public class Crc
    {
        public static uint Crc32(byte[] data, int offset, int len)
        {
            _CRC crc = new _CRC();
            crc.Update(data, (uint)offset, (uint)len);
            return crc.GetDigest();
        }
        public static uint Crc32FromFile(string path)
        {
            if (!File.Exists(path))
                return 0;
            FileStream fs = new FileStream(path, FileMode.Open);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            fs.Close();
            return Crc32(data, 0, data.Length);
        }
    }
}
