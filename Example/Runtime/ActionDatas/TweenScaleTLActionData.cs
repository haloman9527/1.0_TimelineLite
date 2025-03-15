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
using Atom;
using System;
using UnityEngine;

namespace Atom.TimelineLite.Example
{
    [Serializable]
    public class TweenScaleTLActionData : TLActionData
    {
        public Vector3 from, to;
        public EasingType ease;
    }
}