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

namespace Atom.TimelineLite
{
    /// <summary> 轨道数据基类 </summary>
    [Serializable]
    public abstract class TLTrackData
    {
        public string name;
        public bool enabled = true;
    }
}