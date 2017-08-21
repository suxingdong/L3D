/***********************************************
	FileName: Event.cs	    
	Creation: 2017-07-25
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

namespace GF
{
    /// <summary>
    /// 事件事件接口
    /// </summary>
    public class Event : IEvent
    {
        // GETTER / SETTER
        /// <summary>
        /// The _type_string.
        /// </summary>
        private string _typeString;
        string IEvent.type
        {
            get
            {
                return _typeString;
            }
            set
            {
                _typeString = value;

            }
        }

        /// <summary>
        /// The _target_object.
        /// </summary>
        private object _targetObject;
        object IEvent.target
        {
            get
            {
                return _targetObject;
            }
            set
            {
                _targetObject = value;
            }
        }

        ///<summary>
        ///	 Constructor
        ///</summary>
        public Event(string aType_str)
        {
            //
            _typeString = aType_str;
        }

        public Event(int aType_int)
        {
            _typeString = aType_int + "";
        }

        private object _parameter;
        object IEvent.parameter
        {
            get
            {
                return _parameter;
            }
            set
            {
                _parameter = value;

            }
        }

        /// <summary>
        /// Deconstructor
        /// </summary>
        //~Event ( )
        //{
        //	Debug.Log ("Event.deconstructor()");
        //}
    }
}