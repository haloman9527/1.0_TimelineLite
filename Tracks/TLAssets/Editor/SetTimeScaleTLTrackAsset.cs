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
// 请将此脚本放在Editor目录下
using Moyo.TimelineLite.Editors;
using System;
using UnityEngine.Timeline;

namespace Moyo.TimelineLite.Editors
{
    [Serializable]
    [TrackColor(0.99f, 1.00f, 0.83f)]
    [TLTrackAssetMenuItem("Utility/TimeScale")]
    [TrackClipType(typeof(SetTimeScaleTLClipAsset))]
    public class SetTimeScaleTLTrackAsset : TLBasicTrackAsset
    {
        // 若有需求可返回自定义的轨道数据对象
        public override TLBasicTrackData CreateTrackData()
        {
            TLBasicTrackData trackData = new TLBasicTrackData();

            return trackData;
        }
    }
}
