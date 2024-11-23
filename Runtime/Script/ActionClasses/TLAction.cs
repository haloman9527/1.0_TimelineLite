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

using UnityEngine.Playables;

namespace Jiange.TimelineLite
{
    public interface ITLAction
    {
        TLActionData ActionData { get; }
        /// <summary> Action name </summary>
        string Name { get; }
        /// <summary>  </summary>
        bool TriggerOnSkip { get; }
        /// <summary> Start Frame </summary>
        int Start { get; }
        int FrameCount { get; }
        /// <summary> End Frame </summary>
        int End { get; }
        float StartTime { get; }
        float Duration { get; }
        float EndTime { get; }
        bool HasTriggered { get; }
        bool HasFinished { get; }
        ITLTrack Track { get; }

        void Initialize(PlayableDirectorLite _master);

        void GraphStart();

        void Pause();

        void Resume();

        void ActionStart(float _timeSinceActionStart);

        void UpdateAction(int _frameSinceActionStart, float _timeSinceActionStart);

        void ActionFinish();

        void ActionStop();

        void GraphStop();

        void Reset();

        void SpeedChanged(float _speed);
    }

    /// <summary> 运行时逻辑基类</summary>
    /// <typeparam name="T"> 数据类 </typeparam>
    public abstract class TLAction<T> : ITLAction, IPlayableBehaviour where T : TLActionData, new()
    {
        public T TActionData { get; set; }
        public TLActionData ActionData { get { return TActionData; } }

        public string Name { get { return ActionData.ActionBaseInfo.name; } }

        public bool TriggerOnSkip { get { return ActionData.triggerOnSkip; } }

        public int Start { get { return ActionData.ActionBaseInfo.start; } }

        public int FrameCount { get { return ActionData.ActionBaseInfo.frameCount; } }

        public int End { get { return ActionData.ActionBaseInfo.end; } }

        public float StartTime { get { return ActionData.ActionBaseInfo.startTime; } }

        public float Duration { get { return ActionData.ActionBaseInfo.duration; } }

        public float EndTime { get { return ActionData.ActionBaseInfo.endTime; } }

        public bool HasTriggered { get; private set; }

        public bool HasFinished { get; private set; }

        /// <summary> Action所属Track </summary>
        public ITLTrack Track { get; private set; }

        protected PlayableDirectorLite Master { get; private set; }

        public TLAction() { }

        public TLAction(ITLTrack _track, T _actionData) { Track = _track; TActionData = _actionData; }

        public void Initialize(PlayableDirectorLite _master)
        {
            Master = _master;
            OnInitialized();
        }

        /// <summary> 初始化时触发 </summary>
        protected virtual void OnInitialized() { }

        public void GraphStart() { OnGraphStart(); }

        /// <summary> 整个Timeline开始时触发 </summary>
        protected virtual void OnGraphStart() { }

        public void Pause() { OnPause(); }

        /// <summary> 暂停时触发 </summary>
        protected virtual void OnPause() { }

        public void Resume() { OnResume(); }

        /// <summary> 暂停后恢复时触发 </summary>
        protected virtual void OnResume() { }

        public void ActionStart(float _timeSinceActionStart)
        {
            HasTriggered = true;
            OnActionStart(_timeSinceActionStart);
        }

        /// <summary> 片段开始时触发 </summary>
        /// <param name="_timeSinceActionStart">  </param>
        protected virtual void OnActionStart(float _timeSinceActionStart) { }

        public void UpdateAction(int _frameSinceClipStart, float _timeSinceActionStart)
        {
            if (!HasTriggered)
                ActionStart(_timeSinceActionStart);

            OnUpdateAction(_timeSinceActionStart);

            if (!HasFinished)
            {
                if (Master.IsPlayingForward)
                {
                    if (_frameSinceClipStart >= FrameCount)
                        ActionFinish();
                }
                else
                {
                    if (_frameSinceClipStart <= 0)
                        ActionFinish();
                }
            }
        }

        /// <summary> 每帧触发 </summary>
        /// <param name="_timeSinceActionStart">  </param>
        protected virtual void OnUpdateAction(float _timeSinceActionStart) { }

        public void ActionFinish()
        {
            HasFinished = true;
            OnActionFinish();
        }

        /// <summary> 片段完成时触发 </summary>
        protected virtual void OnActionFinish() { }

        public void ActionStop()
        {
            HasTriggered = false;
            HasFinished = false;
            OnActionStop();
        }

        /// <summary> 片段结束时触发 </summary>
        protected virtual void OnActionStop() { }

        public void GraphStop() { if (HasTriggered) ActionStop(); OnGraphStop(); }

        /// <summary> Timeline播放结束时触发 </summary>
        protected virtual void OnGraphStop() { }

        public void Reset()
        {
            HasTriggered = false;
            HasFinished = false;
            OnReset();
        }

        /// <summary> 调用重置函数时触发 </summary>
        protected virtual void OnReset() { }

        public void SpeedChanged(float _speed) { OnSpeedChanged(_speed); }

        /// <summary> 播放速度被修改时触发 </summary>
        /// <param name="_speed"></param>
        protected virtual void OnSpeedChanged(float _speed) { }

        public void OnGraphStart(Playable playable)
        {
            throw new System.NotImplementedException();
        }

        public void OnGraphStop(Playable playable)
        {
            throw new System.NotImplementedException();
        }

        public void OnPlayableCreate(Playable playable)
        {

        }

        public void OnPlayableDestroy(Playable playable)
        {

        }

        public void OnBehaviourPlay(Playable playable, FrameData info)
        {

        }

        public void OnBehaviourPause(Playable playable, FrameData info)
        {

        }

        public void PrepareFrame(Playable playable, FrameData info)
        {

        }

        public void ProcessFrame(Playable playable, FrameData info, object playerData)
        {

        }
    }
}
