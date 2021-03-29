
namespace CZToolKit.TimelineLite
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

        void Initialize(PlayableDirectorLite _owner);

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
    public abstract class TLAction<T> : ITLAction where T : TLActionData, new()
    {
        T actionData;
        ITLTrack track;
        PlayableDirectorLite playable;

        public T TActionData { get { return actionData; } }
        public TLActionData ActionData { get { return actionData; } }

        public string Name { get { return TActionData.name; } }

        public bool TriggerOnSkip { get { return actionData.triggerOnSkip; } }

        public int Start { get { return TActionData.start; } }

        public int FrameCount { get { return TActionData.frameCount; } }

        public int End { get { return TActionData.end; } }

        public float StartTime { get { return TActionData.startTime; } }

        public float Duration { get { return TActionData.duration; } }

        public float EndTime { get { return TActionData.endTime; } }

        public bool HasTriggered { get; private set; }

        public bool HasFinished { get; private set; }

        /// <summary> Action所属Track </summary>
        public ITLTrack Track { get { return track; } }

        protected PlayableDirectorLite Playable { get { return playable; } }

        public TLAction() { }

        public TLAction(ITLTrack _track, T _logicData) { track = _track; actionData = _logicData; }

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

        public void ActionStart(float _timeSinceActionStart)
        {
            HasTriggered = true;
            OnActionStart(_timeSinceActionStart);
        }

        /// <summary> 片段开始时触发 </summary>
        /// <param name="_timeSinceActionStart"></param>
        protected virtual void OnActionStart(float _timeSinceActionStart) { }

        public void UpdateAction(int _frameSinceClipStart, float _timeSinceActionStart)
        {
            if (!HasTriggered)
                ActionStart(_timeSinceActionStart);

            OnUpdateAction(_timeSinceActionStart);

            if (!HasFinished)
            {
                if (Playable.IsPlayingForward)
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
        /// <param name="_timeSinceActionStart"></param>
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
    }
}
