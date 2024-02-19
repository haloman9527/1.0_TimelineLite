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
 *  Blog: https://www.haloman.net/
 *
 */
#endregion
using System;

namespace CZToolKit.TimelineLite.Editors
{
    /// <summary> menuitem </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TLTrackAssetMenuItemAttribute : Attribute
    {
        public string defaultTrackName;

        public TLTrackAssetMenuItemAttribute() { }

        public TLTrackAssetMenuItemAttribute(string _defaultTrackName)
        {
            defaultTrackName = _defaultTrackName;
        }
    }
}
