 using CZToolKit.Core;
using CZToolKit.TimelineLite.Editors;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

namespace CZToolKit.TimelineLite.Example
{
    [Serializable]
    public class TweenPositionTLClipAsset : TLBasicClipAsset<TweenPositionTLActionData>
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

        protected override void OnSceneGUISelected(PlayableDirectorLite _playable, TimelineClip _timelineClip)
        {
            float startFrame = _timelineClip.GetStartFrame();
            float endFrame = _timelineClip.GetEndFrame();
            int indicator = TimelineLiteEditorWindow.Instance.IndicatorFrame;
            float progress = (indicator - startFrame) / (endFrame - startFrame);
            if (progress < 0 || progress > 1) return;

            Vector3 position = new Vector3(
                Easing.Tween(from.x, to.x, progress, ease),
                Easing.Tween(from.y, to.y, progress, ease),
                Easing.Tween(from.z, to.z, progress, ease)
                );
            Handles.DotHandleCap(0, position, Quaternion.identity, 0.1f, EventType.Repaint);
        }
    }
}
