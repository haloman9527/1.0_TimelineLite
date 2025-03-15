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

using System;
using Atom.TimelineLite.Editors;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

namespace Atom.TimelineLite.Example
{
    [Serializable]
    [HideInMenu]
    public class TweenPositionTLClipAsset : TLBasicClipAsset<TweenPositionTLActionData>, ISceneGUI
    {
        public Vector3 from;
        public Vector3 to;
        public EasingType ease = EasingType.Linear;

        public override TLActionData CreateActionData()
        {
            TweenPositionTLActionData actionData = new TweenPositionTLActionData();

            actionData.startPosition = from;
            actionData.endPosition = to;
            actionData.ease = ease;

            return actionData;
        }

        public void SceneGUISelected(PlayableDirectorLite _playable, TimelineClip _timelineClip, int _indicator)
        {
            float startFrame = _timelineClip.GetStartFrame();
            float endFrame = _timelineClip.GetEndFrame();
            float progress = (_indicator - startFrame) / (endFrame - startFrame);
            if (progress < 0 || progress > 1) return;

            Vector3 position = new Vector3(
                Easing.Tween(from.x, to.x, progress, ease),
                Easing.Tween(from.y, to.y, progress, ease),
                Easing.Tween(from.z, to.z, progress, ease)
                );
            Handles.DotHandleCap(0, position, Quaternion.identity, 0.1f, EventType.Repaint);
        }

        public void SceneGUI(PlayableDirectorLite _playable, TimelineClip _timelineClip, int _indicator)
        {

        }
    }
}
