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
 *  Blog: https://www.mindgear.net/
 *
 */
#endregion
using UnityEngine.Timeline;

namespace CZToolKit.TimelineLite.Editors
{
    public interface IDrawGizmos
    {
        void DrawGizmos_Lite(PlayableDirectorLite _playable, TimelineClip _timelineClip, int _indicator);
        void DrawGizmosSelected_Lite(PlayableDirectorLite _playable, TimelineClip _timelineClip, int _indicator);
    }
}
