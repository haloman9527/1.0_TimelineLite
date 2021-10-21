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
 *  Github: https://github.com/HalfLobsterMan
 *  Blog: https://www.crosshair.top/
 *
 */
#endregion
using CZToolKit.Core;
using CZToolKit.Core.Blackboards;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CZToolKit.TimelineLite
{
    public interface ITimelineLiteObject
    {
        TimelineLiteObjectData TimelineData { get; }
        bool Initialized { get; }
        bool Loop { get; }
        int FrameCount { get; }
        float FrameRate { get; }
        float FrameRateSecond { get; }

        /// <summary> 持续时间 </summary>
        float Duration { get; }

        Dictionary<string, ICZType> Blackboard { get; }

        List<ITLTrack> Tracks { get; }

        /// <summary> 初始化函数 </summary>
        void Initialize(PlayableDirectorLite _owner);

        /// <summary> 当开始播放时执行 </summary>
        void Start();

        /// <summary> 当播放时每帧执行 </summary>
        void UpdateTracks(int _frame, float _time);

        /// <summary> 当暂停播放时执行 </summary>
        void Pause();

        /// <summary> 当恢复播放时执行 </summary>
        void Resume();

        /// <summary> 当停止播放时执行 </summary>
        void Stop();

        /// <summary> 重置 </summary>
        void Reset();

        /// <summary> 更新帧数(如果激活或禁用了某个轨道，可能导致帧数与原有帧数不符) </summary>
        void UpdateFrameCount();

        /// <summary> 当外部播放速度被修改时触发 </summary>
        void SpeedChanged(float _speed);
    }

    public class TimelineLiteObject<T> : ITimelineLiteObject where T : TimelineLiteObjectData, new()
    {
        #region 静态变量
        protected static Dictionary<Type, Type> ActionDataDict;
        #endregion

        #region 静态方法
        static TimelineLiteObject()
        {
            ActionDataDict = new Dictionary<Type, Type>();
            foreach (var actionType in Util_Reflection.GetChildTypes<ITLAction>())
            {
                if (actionType.IsGenericType || actionType.IsAbstract) continue;
                Type actionDataType = actionType.GetProperty("TActionData",
                    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).PropertyType;
                ActionDataDict[actionDataType] = actionType;
            }
        }

        static ITLTrack GetTrack(ITimelineLiteObject _timelineLiteObject, TLTrackData _trackData)
        {
            Type trackDataType = _trackData.GetType();
            if (trackDataType == typeof(TLGroupTrackData))
            {
                TLGroupTrackData groupTrackData = _trackData as TLGroupTrackData;
                TLGroupTrack groupTrack = new TLGroupTrack(_timelineLiteObject, _trackData as TLGroupTrackData);
                foreach (TLTrackData childTrack in groupTrackData.ChildTracks)
                {
                    ITLTrack track = GetTrack(_timelineLiteObject, childTrack);
                    if (track != null)
                        groupTrack.ChildTracks.Add(track);
                }

                return groupTrack;
            }
            else /* if (typeof(TLBasicTrackData).IsAssignableFrom(trackDataType))*/
            {
                TLBasicTrackData basicTrackData = _trackData as TLBasicTrackData;
                TLBasicTrack basicTrack = new TLBasicTrack(_timelineLiteObject, basicTrackData);
                foreach (TLActionData actionData in basicTrackData.Clips)
                {
                    Type actionType;
                    if (ActionDataDict.TryGetValue(actionData.GetType(), out actionType))
                    {
                        ITLAction action =
                            Activator.CreateInstance(actionType, basicTrack, actionData) as ITLAction;
                        if (action != null)
                            basicTrack.Clips.Add(action);
                    }
                }
                return basicTrack;
            }
        }
        #endregion

        T timelineData;
        bool initialized = false;
        Dictionary<string, ICZType> blackboard = new Dictionary<string, ICZType>();
        List<ITLTrack> tracks = new List<ITLTrack>();

        public bool Initialized { get { return initialized; } }

        public bool Loop { get { return timelineData.Loop; } }

        public Dictionary<string, ICZType> Blackboard { get { return blackboard; } }

        public T TTimelineData { get { return timelineData; } }

        public TimelineLiteObjectData TimelineData { get { return timelineData; } }

        public PlayableDirectorLite Master { get; private set; }

        public int FrameCount
        {
            get { return timelineData.FrameCount; }
            set { timelineData.FrameCount = value; }
        }

        public float FrameRate
        {
            get { return timelineData.FrameRate; }
            set { timelineData.FrameRate = value; }
        }

        public float FrameRateSecond { get { return timelineData.FrameRateSecond; } }

        /// <summary> 持续时间 </summary>
        public float Duration { get { return FrameCount * FrameRateSecond; } }

        public List<ITLTrack> Tracks { get { return tracks; } }

        public TimelineLiteObject() { }

        public TimelineLiteObject(T _timelineData)
        {
            timelineData = _timelineData;
            foreach (TLTrackData trackData in timelineData.Tracks)
            {
                ITLTrack track = GetTrack(this, trackData);
                if (track != null)
                    tracks.Add(track);
            }
        }

        /// <summary> 初始化函数 </summary>
        public void Initialize(PlayableDirectorLite _master)
        {
            Master = _master;
            for (int i = 0; i != tracks.Count; ++i)
            {
                tracks[i].Initialize(_master);
            }
            OnInitialized();
            initialized = true;
        }

        protected virtual void OnInitialized() { }

        /// <summary> 当开始播放时执行 </summary>
        public void Start()
        {
            for (int i = 0; i != tracks.Count; ++i)
            {
                tracks[i].Start();
            }
        }

        /// <summary> 当播放时每帧执行 </summary>
        public void UpdateTracks(int _frame, float _time)
        {
            foreach (var track in tracks)
            {
                if (!track.Enabled) continue;
                track.UpdateActions(_frame, _time);
            }
        }

        /// <summary> 当暂停播放时执行 </summary>
        public void Pause()
        {
            for (int i = 0; i != tracks.Count; ++i)
            {
                if (tracks[i].Enabled)
                    tracks[i].Pause();
            }
        }

        /// <summary> 当恢复播放时执行 </summary>
        public void Resume()
        {
            for (int i = 0; i != tracks.Count; ++i)
            {
                if (tracks[i].Enabled)
                    tracks[i].Resume();
            }
        }

        /// <summary> 当停止播放时执行 </summary>
        public void Stop()
        {
            for (int i = 0; i != tracks.Count; ++i)
            {
                tracks[i].Stop();
            }
        }

        /// <summary> 重置 </summary>
        public void Reset()
        {
            blackboard.Clear();
            for (int i = 0; i != tracks.Count; ++i)
            {
                tracks[i].Reset();
            }
        }

        /// <summary> 更新帧数(如果激活或禁用了某个轨道，可能导致帧数与原有帧数不符) </summary>
        public void UpdateFrameCount()
        {
            int count = 0;
            for (int i = 0; i < Tracks.Count; i++)
            {
                if (Tracks[i].Enabled && Tracks[i].GetFrameCount() > count)
                    count = Tracks[i].GetFrameCount();
            }

            FrameCount = count;
        }

        /// <summary> 当外部播放速度被修改时触发 </summary>
        public void SpeedChanged(float _speed)
        {
            for (int i = 0; i != tracks.Count; ++i)
            {
                tracks[i].SpeedChanged(_speed);
            }
        }
    }
}