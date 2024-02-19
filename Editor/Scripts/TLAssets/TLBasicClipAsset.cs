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


        public override bool TriggerOnSkip { get { return triggerOnSkip; } }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject go) { return ScriptPlayable<T>.Create(graph, new T()); }
    }
}
