using UnityEngine.Timeline;

namespace CZToolKit.TimelineLite.Editors
{
    public interface ISceneGUI
    {
        void SceneGUI(PlayableDirectorLite _playable, TimelineClip _timelineClip, int _indicator);
        void SceneGUISelected(PlayableDirectorLite _playable, TimelineClip _timelineClip, int _indicator);
    }
}
