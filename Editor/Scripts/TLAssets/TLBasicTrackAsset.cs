using UnityEngine;
using UnityEngine.Timeline;

namespace CZToolKit.TimelineLite.Editors
{
    public interface ITLBasicTrackAsset
    {
        TLBasicTrackData CreateTrackData();
    }

    public interface ITLBasicTrackAssetEditorEnter
    {
        void Enter();
    }

    public abstract class TLBasicTrackAsset : TrackAsset, ITLBasicTrackAsset
    {
        [TextArea]
        public string description;

        public abstract TLBasicTrackData CreateTrackData();
    }
}
