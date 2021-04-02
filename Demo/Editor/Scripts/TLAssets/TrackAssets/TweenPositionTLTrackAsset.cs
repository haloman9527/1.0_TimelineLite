using System;
using UnityEngine.Timeline;

namespace CZToolKit.TimelineLite.Example
{
    [Serializable]
    [TrackColor(0f, 0.5f, 0f)]
    [TLTrackMinHeight(20)]
    [TLTrackAssetMenuItem("Example/插值位移")]
    [TrackClipType(typeof(TweenPositionTLClipAsset))]
    public class TweenPositionTLTrackAsset : TLBasicTrackAsset
    {
        public override TLBasicTrackData CreateTrackData()
        {
            TLBasicTrackData trackData = new TLBasicTrackData();
            return trackData;
        }
    }
}
