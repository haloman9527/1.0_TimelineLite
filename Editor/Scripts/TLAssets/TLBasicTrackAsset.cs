using UnityEngine;
using UnityEngine.Timeline;

namespace CZToolKit.TimelineLite
{
    public interface ITLBasicTrackAsset
    {
        TLBasicTrackData CreateTrackData();
    }

    public abstract class TLBasicTrackAsset : TrackAsset, ITLBasicTrackAsset
    {
        [TextArea]
        public string description;

        public abstract TLBasicTrackData CreateTrackData();
    }
}
