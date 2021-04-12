using UnityEngine.Timeline;

namespace CZToolKit.TimelineLite.Editors
{
    public interface IDrawGizmos
    {
        void DrawGizmos_Lite(PlayableDirectorLite _playable, TimelineClip _timelineClip, int _indicator);
        void DrawGizmosSelected_Lite(PlayableDirectorLite _playable, TimelineClip _timelineClip, int _indicator);
    }
}
