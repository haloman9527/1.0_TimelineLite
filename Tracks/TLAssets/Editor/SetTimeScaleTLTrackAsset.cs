// 请将此脚本放在Editor目录下
using CZToolKit.TimelineLite;
using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace CZToolKit.TimelineLite
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
