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
 *  Blog: https://www.mindgear.net/
 *
 */
#endregion
using CZToolKit.TimelineLite;
using System;
using UnityEngine;

namespace CZToolKit.TimelineLite
{
    /// <summary> <see cref="SetTimeScaleTLAction"/> </summary>
    [Serializable]
    public class SetTimeScaleTLActionData : TLActionData
    {
        public float timeScale = 1;
    }
}