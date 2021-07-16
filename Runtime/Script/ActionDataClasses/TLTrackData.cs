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

namespace CZToolKit.TimelineLite
{
    /// <summary> 轨道数据基类 </summary>
    [Serializable]
    public abstract class TLTrackData
    {
        public string name;
        public bool enabled = true;
    }
}