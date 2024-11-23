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
using Jiange.TimelineLite;
using System;
using UnityEngine;

namespace Jiange.TimelineLite.Example
{
    [Serializable]
    public class TweenRotationTLActionData : TLActionData
    {
        public Vector3 from, to;
        public EasingType ease;
    }
}