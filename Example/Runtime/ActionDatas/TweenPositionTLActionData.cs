using CZToolKit.Core;
using System;
using UnityEngine;

namespace CZToolKit.TimelineLite.Example
{
    [Serializable]
    public class TweenPositionTLActionData : TLActionData
    {
        public Vector3 startPosition, endPosition;
        public EasingType ease = EasingType.Linear;
    }
}
