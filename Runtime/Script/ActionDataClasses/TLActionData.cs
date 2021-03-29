using System;
using UnityEngine.Playables;
using UnityEngine;
using System.Collections.Generic;

namespace CZToolKit.TimelineLite
{
    /// <summary> 逻辑片段数据提供者抽象基类 </summary>
    [Serializable]
    public abstract class TLActionData : PlayableBehaviour
    {
        [HideInInspector] public string name;
        [HideInInspector] public bool triggerOnSkip;
        [HideInInspector] public int start;
        [HideInInspector] public int frameCount;
        [HideInInspector] public int end;
        [HideInInspector] public float startTime;
        [HideInInspector] public float duration;
        [HideInInspector] public float endTime;

        [NonSerialized] private PlayableDirectorLite playable;

        protected PlayableDirectorLite Playable { get { return playable; } }
        public bool HasTriggered { get; private set; }
        public bool HasFinished { get; private set; }

        public void Initialize(PlayableDirectorLite _owner)
        {
            playable = _owner;
            OnInitialize();
        }

        /// <summary> 初始化时触发 </summary>
        protected virtual void OnInitialize() { }

        public void GraphStart() { OnGraphStart(); }

        /// <summary> 整个Timeline开始时触发 </summary>
        protected virtual void OnGraphStart() { }

        public void Pause() { OnPause(); }

        /// <summary> 暂停时触发 </summary>
        protected virtual void OnPause() { }

        public void Resume() { OnResume(); }

        /// <summary> 暂停后恢复时触发 </summary>
        protected virtual void OnResume() { }

        public void ClipStart(float _timeSinceClipStart)
        {
            HasTriggered = true;
            OnClipStart(_timeSinceClipStart);
        }

        /// <summary> 片段开始时触发 </summary>
        /// <param name="_timeSinceClipStart"></param>
        protected virtual void OnClipStart(float _timeSinceClipStart) { }

        public void UpdateEvent(int _frameSinceClipStart, float _timeSinceClipStart)
        {
            if (!HasTriggered)
                ClipStart(_timeSinceClipStart);

            OnUpdateEvent(_timeSinceClipStart);

            if (!HasFinished && _frameSinceClipStart >= frameCount)
                ClipFinish();
        }

        /// <summary> 每帧触发 </summary>
        /// <param name="_timeSinceClipStart"></param>
        protected virtual void OnUpdateEvent(float _timeSinceClipStart) { }

        public void ClipFinish()
        {
            HasFinished = true;
            OnClipFinish();
        }

        /// <summary> 片段完成时触发 </summary>
        protected virtual void OnClipFinish() { }

        public void ClipStop()
        {
            HasTriggered = false;
            HasFinished = false;
            OnClipStop();
        }

        /// <summary> 片段结束时触发 </summary>
        protected virtual void OnClipStop() { }

        public void GraphStop() { ClipStop(); OnGraphStop(); }

        /// <summary> Timeline播放结束时触发 </summary>
        protected virtual void OnGraphStop() { }

        /// <summary> 外部重新设置时间时触发 </summary>
        protected virtual void OnRelocate(float _timeSinceClipStart) { }

        /// <summary> 调用重置函数时触发 </summary>
        public void Reset()
        {
            HasTriggered = false;
            HasFinished = false;
            OnReset();
        }
        protected virtual void OnReset() { }

        public void SpeedChanged(float _speed) { OnSpeedChanged(_speed); }

        /// <summary> 播放速度被修改时触发 </summary>
        /// <param name="_speed"></param>
        protected virtual void OnSpeedChanged(float _speed) { }
    }
}