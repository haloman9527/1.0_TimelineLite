#region 注 释
/***
 *
 *  Title:
 *  
 *  Description:
 *  
 *  Date:
 *  Version:
 *  Writer: 半只龙虾人
 *  Github: https://github.com/haloman9527
 *  Blog: https://www.mindgear.net/
 *
 */
#endregion
using System;

namespace CZToolKit.TimelineLite.Editors
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class TLTrackMinHeightAttribute : Attribute
    {
        public float MinHeight;
        public TLTrackMinHeightAttribute(float _minHeight)
        {
            MinHeight = _minHeight;
        }
    }
}
