#if UNITY_2019_1_OR_NEWER
using CZToolKit.Core;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

namespace CZToolKit.TimelineLite.Editors
{
    [CustomTimelineEditor(typeof(TLBasicClipAsset))]
    public class TLBasicClipEditor : ClipEditor
    {
        public override void OnCreate(TimelineClip clip, TrackAsset track, TimelineClip clonedFrom)
        {
            base.OnCreate(clip, track, clonedFrom);
        }

        public override ClipDrawOptions GetClipOptions(TimelineClip clip)
        {
            ClipDrawOptions options = base.GetClipOptions(clip);

            if (Utility_Attribute.TryGetTypeAttribute(clip.asset.GetType(), out TLClipTooltipAttribute attribute))
                options.tooltip = attribute.Tooltip;

            return options;
        }

        public override void DrawBackground(TimelineClip clip, ClipBackgroundRegion region)
        {
            base.DrawBackground(clip, region);
        }
    }
}
#endif