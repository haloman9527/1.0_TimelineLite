using System;
using UnityEngine;
using UnityEngine.Playables;

namespace CZToolKit.TimelineLite.Editors
{
    public abstract class TLBasicClipAsset : PlayableAsset
    {
        public abstract bool TriggerOnSkip { get; }

        public abstract TLActionData CreateActionData();
    }

    [Serializable]
    public abstract class TLBasicClipAsset<T> : TLBasicClipAsset where T : TLActionData, new()
    {
        [SerializeField] bool triggerOnSkip = true;

        private PlayableGraph graph;

        public override bool TriggerOnSkip { get { return triggerOnSkip; } }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject go) { this.graph = graph; return ScriptPlayable<T>.Create(graph, new T()); }
    }
}
