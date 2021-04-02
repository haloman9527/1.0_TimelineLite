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

            if (AttributeCache.TryGetTypeAttribute(track.GetType(), out TLTrackMinHeightAttribute attribute))
                options.minimumHeight = attribute.MinHeight;

            return options;
        }
    }
}
