using CZToolKit.Core;
using System;
using UnityEngine;

namespace CZToolKit.TimelineLite
{
    /// <summary> <see cref="SetUpdateModeTLAction"/> </summary>
    [Serializable]
    public class SetUpdateModeTLActionData : TLActionData
    {
        public PlayableUpdateMode updateMode;
    }
}