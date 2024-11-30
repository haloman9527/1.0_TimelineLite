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
using Moyo.TimelineLite;
using System;
using UnityEngine;

namespace Moyo.TimelineLite
{
    /// <summary> <see cref="SetTimeScaleTLAction"/> </summary>
    [Serializable]
    public class SetTimeScaleTLActionData : TLActionData
    {
        public float timeScale = 1;
    }
}