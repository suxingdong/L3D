/***********************************************
	FileName: EventProtocol.cs	    
	Creation: 2017-07-07
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GF
{
    public interface iEventProtocol
    {
        void AddEventListener(string eventName, object listener);

        void RemoveEventListener(string eventName);

        void RemoveAllEventListeners();
        
    }
}

