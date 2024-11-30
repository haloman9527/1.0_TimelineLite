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
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Moyo.TimelineLite
{
    public interface ITLTrack
    {
        TLTrackData TrackData { get; }
        string TrackName { get; }
        bool Enabled { get; set; }
        /// <summary> Track所属TimelineLiteObject </summary>
        ITimelineLiteObject TimelineLiteObject { get; }

        /// <summary> 初始化方法 </summary>
        /// <param name="_owner">所有者</param>
        void Initialize(PlayableDirectorLite _owner);

        void Start();

        void UpdateActions(int _frame, float _time);

        void Pause();

        void Resume();

        void Stop();

        void Reset();

        void SpeedChanged(float _speed);

        int GetFrameCount();
    }

    /// <summary> 轨道基类 </summary>
    /// <typeparam name="T">轨道数据提供类</typeparam>
    public abstract class TLTrack<T> : ITLTrack where T : TLTrackData
    {
        #region 字段
        [SerializeField]
        T trackData = null;
        PlayableDirectorLite playable = null;
        ITimelineLiteObject timelineLiteObject = null;
        #endregion

        #region 属性
        public T TTrackData { get { return trackData; } }
        public TLTrackData TrackData { get { return trackData; } }
        public string TrackName { get { return trackData.name; } }

        public bool Enabled
        {
            get { return trackData.enabled; }
            set
            {
                if (trackData.enabled == value) return;
                trackData.enabled = value;
                timelineLiteObject.UpdateFrameCount();
            }
        }
        public ITimelineLiteObject TimelineLiteObject { get { return timelineLiteObject; } }
        public PlayableDirectorLite Playable { get { return playable; } }

        public double duration => throw new System.NotImplementedException();

        #endregion

        #region 公共方法
        public TLTrack() { }
        public TLTrack(ITimelineLiteObject _timelineLiteObject, T _trackData) { timelineLiteObject = _timelineLiteObject; trackData = _trackData; }

        public virtual void Initialize(PlayableDirectorLite _owner) { playable = _owner; }

        public virtual void Start() { }

        public virtual void Pause() { }

        public virtual void Resume() { }

        public virtual void Stop() { }

        public virtual void UpdateActions(int _frame, float _time) { }

        public virtual void Reset() { }

        public virtual void SpeedChanged(float _speed) { }

        public abstract int GetFrameCount();
        #endregion
    }
}
