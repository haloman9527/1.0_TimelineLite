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
using System.Reflection;
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

            var attr = track.GetType().GetCustomAttribute<TLTrackMinHeightAttribute>(true);
            if (attr != null)
                options.minimumHeight = attr.MinHeight;

            return options;
        }
    }
}
#endif
