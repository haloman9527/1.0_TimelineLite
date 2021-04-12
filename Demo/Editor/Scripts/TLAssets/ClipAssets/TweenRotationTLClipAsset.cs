using CZToolKit.Core;
using CZToolKit.TimelineLite.Editors;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

namespace CZToolKit.TimelineLite.Example
{
    [Serializable]
    public class TweenRotationTLClipAsset : TLBasicClipAsset<TweenRotationTLActionData>, ISceneGUI
    {
        public Vector3 from, to;
        public EasingType ease = EasingType.Linear;

        /// <summary> 创建<see cref="TweenRotationTLActionData" />对象 </summary>
        public override TLActionData CreateActionData()
        {
            TweenRotationTLActionData actionData = new TweenRotationTLActionData();
            // 在此进行对应的数值设置
            actionData.from = from;
            actionData.to = to;
            actionData.ease = ease;

            return actionData;
        }

        public void SceneGUI(PlayableDirectorLite _playable, TimelineClip _timelineClip, int _indicator)
        {

        }

        public void SceneGUISelected(PlayableDirectorLite _playable, TimelineClip _timelineClip,int _indicator)
        {
            float startFrame = _timelineClip.GetStartFrame();
            float endFrame = _timelineClip.GetEndFrame();
            int indicator = TimelineLiteEditorWindow.Instance.IndicatorFrame;
            float progress = (indicator - startFrame) / (endFrame - startFrame);
            if (progress < 0 || progress > 1) return;

            progress = Mathf.Clamp01(progress);
            Quaternion rotation = Quaternion.Euler(new Vector3(
                Easing.Tween(from.x, to.x, progress, ease),
                Easing.Tween(from.y, to.y, progress, ease),
                Easing.Tween(from.z, to.z, progress, ease)
                ));
            Handles.ArrowHandleCap(0, _playable.transform.position, rotation, 3, EventType.Repaint);
        }
    }
}