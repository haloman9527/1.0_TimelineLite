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
using CZToolKit;
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