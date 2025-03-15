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

namespace Atom.TimelineLite
{
    /// <summary> <see cref="SetUpdateModeTLAction"/> </summary>
    [Serializable]
    public class SetUpdateModeTLActionData : TLActionData
    {
        public UpdateMode updateMode;
    }
}