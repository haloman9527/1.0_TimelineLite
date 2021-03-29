using CZToolKit.TimelineLite.Editors;
using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CZToolKit.TimelineLite
{
    public abstract class TLBasicClipAsset : PlayableAsset
    {
        public abstract bool TriggerOnSkip { get; }

        public abstract TLActionData CreateActionData();
        public abstract void DrawGizmos_Lite(PlayableDirectorLite _playable, TimelineClip _timelineClip);
        public abstract void DrawGizmosSelected_Lite(PlayableDirectorLite _playable, TimelineClip _timelineClip);
        public abstract void SceneGUI(PlayableDirectorLite _playable, TimelineClip _timelineClip);
        public abstract void SceneGUISelected(PlayableDirectorLite _playable, TimelineClip _timelineClip);
    }

    [Serializable]
    public abstract class TLBasicClipAsset<T> : TLBasicClipAsset where T : TLActionData, new()
    {
        [SerializeField] bool triggerOnSkip = true;

        private PlayableGraph graph;

        public override bool TriggerOnSkip { get { return triggerOnSkip; } }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject go) { this.graph = graph; return ScriptPlayable<T>.Create(graph, new T()); }

        public override void DrawGizmos_Lite(PlayableDirectorLite _playable, TimelineClip _timelineClip) { OnDrawGizmos_Lite(_playable, _timelineClip); }
        protected virtual void OnDrawGizmos_Lite(PlayableDirectorLite _playable, TimelineClip _timelineClip) { }

        public override void DrawGizmosSelected_Lite(PlayableDirectorLite _playable, TimelineClip _timelineClip) { OnDrawGizmosSelected_Lite(_playable, _timelineClip); }
        protected virtual void OnDrawGizmosSelected_Lite(PlayableDirectorLite _playable, TimelineClip _timelineClip) { }

        public override void SceneGUI(PlayableDirectorLite _playable, TimelineClip _timelineClip) { OnSceneGUI(_playable, _timelineClip); }
        protected virtual void OnSceneGUI(PlayableDirectorLite _playable, TimelineClip _timelineClip) { }

        public override void SceneGUISelected(PlayableDirectorLite _playable, TimelineClip _timelineClip) { OnSceneGUISelected(_playable, _timelineClip); }
        protected virtual void OnSceneGUISelected(PlayableDirectorLite _playable, TimelineClip _timelineClip) { }
    }
}
