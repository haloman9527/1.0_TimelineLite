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
using System;
using UnityEngine;

namespace CZToolKit.TimelineLite
{
    /// <summary> <see cref="SetUpdateModeTLAction"/> </summary>
    [Serializable]
    public class SetUpdateModeTLActionData : TLActionData
    {
        public UpdateMode updateMode;
    }
}