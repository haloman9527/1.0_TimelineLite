using UnityEditor;
using UnityEngine;
using CZToolKit.Core.Editors;

namespace CZToolKit.TimelineLite.Editors
{
    [CustomEditor(typeof(PlayableDirectorLite))]
    public class PlayableDirectorLiteEditor : BasicEditor
    {
        PlayableDirectorLite playableDirectorLite;

        bool HasTarget { get { return playableDirectorLite != null; } }
        bool HasTimeline { get { return HasTarget && playableDirectorLite.Timeline != null; } }

        protected override void OnEnable()
        {
            base.OnEnable();
            playableDirectorLite = target as PlayableDirectorLite;
            EditorApplication.update += CustomRepaint;
        }

        protected override void RegisterDrawers()
        {
            base.RegisterDrawers();
            //RegisterDrawer("updateMode", property =>
            //{
            //    EditorGUI.BeginDisabledGroup(!HasTimeline);
            //    int currentFrame = HasTimeline ? playableDirectorLite.CurrentFrame : 0;
            //    int frameCount = HasTimeline ? playableDirectorLite.Timeline.FrameCount : 0;
            //    Rect rect = GUILayoutUtility.GetRect(18, 30, "TextField");
            //    rect.width -= 35;
            //    int newFrame = (int)EditorGUIExtension.ProgressBar(rect, currentFrame, 0, frameCount, currentFrame + "/" + frameCount, true);

            //    if (newFrame != currentFrame)
            //    {
            //        if (playableDirectorLite.IsPlaying)
            //            playableDirectorLite.Pause();
            //        playableDirectorLite.SetFrame(newFrame);
            //    }
            //    rect.x += rect.width + 5;
            //    rect.width = 30;
            //    switch (playableDirectorLite.PlayStatus)
            //    {
            //        case PlayStatus.Playing:
            //            if (GUI.Button(rect, EditorGUIUtility.IconContent("PauseButton")))
            //                playableDirectorLite.Pause();
            //            break;
            //        case PlayStatus.Pausing:
            //            if (GUI.Button(rect, EditorGUIUtility.IconContent("PlayButton")))
            //                playableDirectorLite.Resume();
            //            break;
            //        case PlayStatus.Stopped:
            //            if (GUI.Button(rect, EditorGUIUtility.IconContent("PlayButton")))
            //                playableDirectorLite.Play();
            //            break;
            //    }
            //    EditorGUI.EndDisabledGroup();

            //    EditorGUILayout.PropertyField(property);
            //});
            RegisterDrawer("speed", property =>
            {
                playableDirectorLite.Speed = EditorGUILayout.FloatField(new GUIContent("Speed"), playableDirectorLite.Speed);
            });
        }

        void CustomRepaint()
        {

        }

        private void OnDisable()
        {
            EditorApplication.update -= CustomRepaint;
        }
    }
}
