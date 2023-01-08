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
using CZToolKit.Common;
using CZToolKit.TimelineLite;
using System;
using UnityEngine;

namespace CZToolKit.TimelineLite.Example
{
    [Serializable]
    public class TweenScaleTLActionData : TLActionData
    {
        public Vector3 from, to;
        public EasingType ease;
    }
}