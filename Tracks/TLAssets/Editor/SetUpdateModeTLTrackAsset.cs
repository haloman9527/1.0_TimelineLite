// 请将此脚本放在Editor目录下
using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace CZToolKit.TimelineLite
{
    [Serializable]
    [TrackColor(0.84f, 1.00f, 0.00f)]
    [TLTrackAssetMenuItem("Utility/Playable更新模式")]
    [TrackClipType(typeof(SetUpdateModeTLClipAsset))]
    public class SetUpdateModeTLTrackAsset : TLBasicTrackAsset
    {
        // 若有需求可返回自定义的轨道数据对象
        public override TLBasicTrackData CreateTrackData()
        {
            TLBasicTrackData trackData = new TLBasicTrackData();

            return trackData;
        }
    }
}
