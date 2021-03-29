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