/***********************************************
	FileName: EventManager.cs	    
	Creation: 2017-07-25
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GF
{
    public class EventManager : Singleton<EventManager>
    {
        /// <summary>  
        /// The event dispatcher.  
        /// </summary>  
        //public Dictionary<object, EventDispatcher> alleventDispatcher;
        public EventDispatcher eventDispatcher;
        private EventManager()
        {
            //alleventDispatcher = new Dictionary<object, EventDispatcher>();           
            eventDispatcher = new EventDispatcher(this);
        }

        public bool AddNetEventListener(NetCmdType aEventName_cmd, EventDelegate aEventDelegate)
        {
            return AddEventListener(aEventName_cmd + "", aEventDelegate);
        }

        public bool AddEventListener( string aEventName_string, EventDelegate aEventDelegate)
        {
            return eventDispatcher.addEventListener(aEventName_string, aEventDelegate);
        }

        public bool RemoveEventListener(string aEventType_string, EventDelegate aEventDelegate)
        {
            return eventDispatcher.removeEventListener(aEventType_string, aEventDelegate);
            //if (alleventDispatcher.ContainsKey(obj))
            //{
            //   return alleventDispatcher[obj].removeEventListener(aEventType_string, aEventDelegate);     
            //}
            //return false;
        }

        public void removeAllEventListeners()
        {
            eventDispatcher.removeAllEventListeners();
            //foreach (object obj in alleventDispatcher)
            //{
            //    alleventDispatcher[obj].removeAllEventListeners();
            //}
        }
               

        public void DispatchEvent(IEvent aIEvent)
        {
            eventDispatcher.dispatchEvent(aIEvent);
            //cmdList.AddItem(aIEvent);
        }

        public void Update()
        {

        }
    }

}
