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
#if UNITY_2019_1_OR_NEWER
using Jiange;
using Sirenix.Utilities;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

namespace Jiange.TimelineLite.Editors
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

            var attr = clip.asset.GetType().GetCustomAttribute<TLClipTooltipAttribute>(true);
            if (attr != null)
                options.tooltip = attr.Tooltip;

            return options;
        }

        public override void DrawBackground(TimelineClip clip, ClipBackgroundRegion region)
        {
            base.DrawBackground(clip, region);
        }
    }
}
#endif