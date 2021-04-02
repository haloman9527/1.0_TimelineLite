using CZToolKit.TimelineLite;
using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace CZToolKit.TimelineLite.Example
{
    [Serializable]
    [TrackColor(0f, 0f, 0.5f)]
    [TLTrackMinHeight(20)]
    [TLTrackAssetMenuItem("Example/插值缩放")]
    [TrackClipType(typeof(TweenScaleTLClipAsset))]
    public class TweenScaleTLTrackAsset : TLBasicTrackAsset
    {
        // 若有需求可返回自定义的轨道数据对象
        public override TLBasicTrackData CreateTrackData()
        {
            TLBasicTrackData trackData = new TLBasicTrackData();

            return trackData;
        }
    }
}
