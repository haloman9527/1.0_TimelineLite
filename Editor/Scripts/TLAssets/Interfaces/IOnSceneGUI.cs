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
using UnityEngine.Timeline;

namespace CZToolKit.TimelineLite.Editors
{
    public interface ISceneGUI
    {
        void SceneGUI(PlayableDirectorLite _playable, TimelineClip _timelineClip, int _indicator);
        void SceneGUISelected(PlayableDirectorLite _playable, TimelineClip _timelineClip, int _indicator);
    }
}
