using UnityEngine.Timeline;

namespace CZToolKit.TimelineLite.Editors
{
    public interface IOnPreview
    {
        void Preview(PlayableDirectorLite _playable, TimelineClip _timelineClip, int _indicator);
    }
}
