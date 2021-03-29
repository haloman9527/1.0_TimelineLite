using CZToolKit.Core;
using CZToolKit.TimelineLite;
using System;
using UnityEngine;

namespace CZToolKit.TimelineLite.Example
{
    [Serializable]
    public class TweenRotationTLActionData : TLActionData
    {
        public Vector3 from, to;
        public EasingType ease;
    }
}