#if UNITY_2019_1_OR_NEWER
using CZToolKit.Core;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

namespace CZToolKit.TimelineLite.Editors
{
    [CustomTimelineEditor(typeof(TLBasicTrackAsset))]
    public class TLBasicTrackEditor : TrackEditor
    {
        public override TrackDrawOptions GetTrackOptions(TrackAsset track, Object binding)
        {
            TrackDrawOptions options = base.GetTrackOptions(track, binding);

            if (Utility.TryGetTypeAttribute(track.GetType(), out TLTrackMinHeightAttribute attribute))
                options.minimumHeight = attribute.MinHeight;

            return options;
        }
    }
}
#endif
