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
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Moyo.TimelineLite
{
    /// <summary> 组轨道数据类 </summary>
    [Serializable]
    public sealed class TLGroupTrackData : TLTrackData
    {
        [SerializeField]
#if UNITY_2019_1_OR_NEWER
        [SerializeReference]
#endif

        private List<TLTrackData> childTracks = new List<TLTrackData>();

        public List<TLTrackData> ChildTracks { get { return childTracks; } }
    }
}
