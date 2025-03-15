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
using Atom;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Atom.TimelineLite
{
    [Serializable]
    public class PlayableEvent : UnityEvent<ITimelineLiteObject> { }

    /// <summary> 播放状态 </summary>
    public enum PlayStatus { Playing = 0, Pausing = 1, Stopped = 2 }

    [Serializable]
    public class PlayableDirectorLite : MonoBehaviour
    {
        #region 静态常量
        public const float DEFAULT_SPEED = 1;
        #endregion

        ITimelineLiteObject timeline;

        [SerializeField] UpdateMode updateMode;
        [SerializeField] float speed = DEFAULT_SPEED;
        bool isPlayingForward;

        int lastFrame = -1;
        int currentFrame;
        float currentTime;
        PlayStatus playStatus = PlayStatus.Stopped;
        Blackboard<string> blackboard = new Blackboard<string>();

        public PlayableEvent onFinishedCallback = new PlayableEvent();

        public ITimelineLiteObject Timeline { get { return timeline; } protected set { timeline = value; } }

        public UpdateMode UpdateMode { get { return updateMode; } set { updateMode = value; } }

        /// <summary> 播放速度 </summary>
        public virtual float Speed
        {
            get { return speed; }
            set
            {
                //                if (IsPlaying)
                //                {
                //#if UNITY_EDITOR
                //                    Debug.LogError("无法在播放中设置速度");
                //#endif
                //                    return;
                //                }
                speed = value;
                isPlayingForward = speed * Time.timeScale >= 0;
            }
        }

        public PlayStatus PlayStatus { get { return playStatus; } }

        /// <summary> 是否正在播放 </summary>
        public bool IsPlaying { get { return playStatus == PlayStatus.Playing; } }

        /// <summary> 当前时间进度 </summary>
        public float CurrentTime
        {
            get { return currentTime; }
            private set
            {
                currentTime = Mathf.Clamp(value, 0, timeline == null ? 0 : timeline.Duration);
                CurrentFrame = TimelineLiteUtility.ConvertToFrameIndex(currentTime * (timeline == null ? 0 : timeline.FrameRate));
            }
        }

        /// <summary> 当前帧进度 </summary>
        public int CurrentFrame
        {
            get { return currentFrame; }
            private set
            {
                lastFrame = value < 0 ? -1 : currentFrame;
                currentFrame = value;
            }
        }

        /// <summary> 帧进度是否发生了更改(有时时间增加的量不足一帧时间) </summary>
        public bool FrameChanged { get { return lastFrame != currentFrame; } }

        public bool IsPlayingForward { get { return isPlayingForward; } }

        protected virtual void Awake()
        {
            isPlayingForward = Speed * Time.timeScale >= 0;
        }

        protected virtual void Start() { }

        protected virtual void Update()
        {
            if ((updateMode != UpdateMode.Normal && updateMode != UpdateMode.UnscaledTime) || !IsPlaying)
                return;
            float delta = updateMode == UpdateMode.Normal ? Time.deltaTime : Time.unscaledDeltaTime;
            if (delta != 0)
                InternalUpdate(delta);
        }

        protected virtual void FixedUpdate()
        {
            if (updateMode != UpdateMode.AnimatePhysics || !IsPlaying)
                return;

            float delta = Time.fixedDeltaTime;
            if (delta != 0)
                InternalUpdate(delta);
        }

        public void Evaluate(float _delta)
        {
            if (updateMode != UpdateMode.Manual || !IsPlaying)
                return;
            if (_delta != 0)
                InternalUpdate(_delta);
        }

        protected virtual void OnDisable()
        {
            if (playStatus != PlayStatus.Stopped)
                Stop();
        }

        /// <summary> 设置时间进度 </summary>
        public void SetTime(float _time)
        {
            SetCurrentTimeInternal(_time);
        }

        /// <summary> 设置帧进度 </summary>
        public void SetFrame(int _frame)
        {
            SetTime(_frame * timeline.FrameRateSecond);
        }

        private void SetCurrentTimeInternal(float _time)
        {
            CurrentTime = Mathf.Clamp(_time, 0, timeline.Duration);
            timeline.UpdateTracks(currentFrame, currentTime);
        }

        private void InternalUpdate(float _delta)
        {
            float targetTime = currentTime + _delta * Speed;
            SetTime(targetTime);
            if (isPlayingForward)
            {
                if (currentTime >= timeline.Duration)
                {
                    onFinishedCallback.Invoke(timeline);
                    if (timeline.Loop)
                        Play(timeline, targetTime % timeline.Duration);
                    else
                        Stop();
                }
            }
            else
            {
                if (currentTime <= 0)
                {
                    onFinishedCallback.Invoke(timeline);
                    if (timeline.Loop)
                        Play(timeline, timeline.Duration + targetTime % timeline.Duration);
                    else
                        Stop();
                }
            }
            AfterInternalUpdate();
        }

        protected virtual void AfterInternalUpdate() { }

        #region Control
        /// <summary> 播放一个Timeline对象 </summary>
        public virtual void Play<T>(T _timeline, float _startTime) where T : ITimelineLiteObject
        {
            if (playStatus != PlayStatus.Stopped)
                Stop();
            timeline = _timeline;
            if (timeline == null) return;

            if (!timeline.Initialized)
                timeline.Initialize(this);
            timeline.Start();
            playStatus = PlayStatus.Playing;
            SetCurrentTimeInternal(_startTime);
        }

        /// <summary> 播放一个Timeline对象 </summary>
        public virtual void Play<T>(T _timeline, int _startFrame) where T : ITimelineLiteObject
        {
            Play(_timeline, _startFrame * _timeline.FrameRateSecond);
        }

        /// <summary> 播放一个Timeline对象 </summary>
        public virtual void Play<T>(T _timeline) where T : ITimelineLiteObject
        {
            if (isPlayingForward)
                Play(_timeline, 0f);
            else
                Play(_timeline, _timeline.Duration);
        }


        public virtual void Play()
        {
            if (isPlayingForward)
                Play(timeline, 0f);
            else
                Play(timeline, timeline.Duration);
        }

        /// <summary> 暂停正在播放的Timeline对象 </summary>
        public virtual void Pause()
        {
            // 若不在播放，则不做处理
            if (!IsPlaying)
                return;
            playStatus = PlayStatus.Pausing;
            timeline.Pause();
        }

        /// <summary> 恢复播放已暂停的Timeline对象 </summary>
        public virtual void Resume()
        {
            // 若已经正在播放，则不做处理
            if (playStatus != PlayStatus.Pausing || timeline == null)
                return;
            playStatus = PlayStatus.Playing;
            timeline.Resume();
        }

        /// <summary> 停止正在播放的Timeline对象 </summary>
        public virtual void Stop()
        {
            playStatus = PlayStatus.Stopped;
            if (timeline != null)
                timeline.Stop();
        }
        #endregion
    }
}