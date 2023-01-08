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
 *  Github: https://github.com/HalfLobsterMan
 *  Blog: https://www.crosshair.top/
 *
 */
#endregion
#if UNITY_2019_1_OR_NEWER
using CZToolKit.Common;
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

            if (Util_Attribute.TryGetTypeAttribute(track.GetType(), out TLTrackMinHeightAttribute attribute))
                options.minimumHeight = attribute.MinHeight;

            return options;
        }
    }
}
#endif
