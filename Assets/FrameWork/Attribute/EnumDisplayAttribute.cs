/***********************************************
	FileName: EnumDisplayAttribute.cs	    
	Creation: 2017-07-25
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

using System;
using System.Linq;

namespace GF
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumDisplayAttribute : Attribute
    {
        public EnumDisplayAttribute(string displayStr)
        {
            Description = displayStr;
        }
        public string Description
        {
            get;
            private set;
        }
        

    }

    public static class EnumExtentions
    {
        public static string Description(this Enum t)
        {
            var t_type = t.GetType();
            var fieldName = Enum.GetName(t_type, t);
            var attributes = t_type.GetField(fieldName).GetCustomAttributes(false);
            var enumDisplayAttribute = attributes.FirstOrDefault(p => p.GetType().Equals(typeof(EnumDisplayAttribute))) as EnumDisplayAttribute;
            return enumDisplayAttribute == null ? fieldName : enumDisplayAttribute.Description;
        }
    }

}
