/***********************************************
	FileName: IEvent.cs	    
	Creation: 2017-07-25
	Author：East.Su
	Version：V1.0.0
	Desc: 
**********************************************/

namespace GF
{
    /// <summary>
    /// IEvent接口
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// type类型.
        /// </value>
        string type { get; set; }

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>
        /// target目标.
        /// </value>
        object target { get; set; }

        object parameter { get; set; }

    }
}
