/***********************************************
	FileName: MessageBoxEnum.cs	    
	Creation: 2017-07-12
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GF
{
    public class MessageBoxEnum
    {
        public delegate void OnReceiveMessageBoxResult(MessageBoxEnum.Result result);

        public enum Style
        {
            Ok,
            OkAndCancel
        }

        public enum Result
        {
            Ok,
            Cancel
        }
    }
}

