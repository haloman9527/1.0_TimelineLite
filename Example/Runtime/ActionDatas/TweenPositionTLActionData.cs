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
using Jiange;
using System;
using UnityEngine;

namespace Jiange.TimelineLite.Example
{
    [Serializable]
    public class TweenPositionTLActionData : TLActionData
    {
        public Vector3 startPosition, endPosition;
        public EasingType ease = EasingType.Linear;
    }
}
