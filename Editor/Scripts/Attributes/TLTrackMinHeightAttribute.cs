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
 *  Github: https://github.com/HalfLobsterMan
 *  Blog: https://www.crosshair.top/
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
