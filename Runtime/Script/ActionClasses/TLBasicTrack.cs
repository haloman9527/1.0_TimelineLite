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

namespace Atom.TimelineLite
{
    /// <summary> 标准轨道 </summary>
    public class TLBasicTrack : TLTrack<TLBasicTrackData>
    {
        #region 私有变量
        int currentClipIndex = 0;
        [SerializeField]
        List<ITLAction> clips = new List<ITLAction>();
        #endregion

        #region 公共属性
        public List<ITLAction> Clips { get { return clips; } }
        #endregion

        #region 公共方法
        public TLBasicTrack() { }

        public TLBasicTrack(ITimelineLiteObject _timelineLiteObject, TLBasicTrackData _trackData) : base(_timelineLiteObject, _trackData) { }

        public override void Initialize(PlayableDirectorLite _owner)
        {
            base.Initialize(_owner);
            for (int i = 0; i != clips.Count; ++i)
            {
                clips[i].Initialize(_owner);
            }
        }

        public override void Start()
        {
            for (int i = clips.Count - 1; i >= 0; --i)
            {
                clips[i].GraphStart();
            }
        }

        public override void Pause()
        {
            for (int i = 0; i != clips.Count; ++i)
            {
                if (clips[i].HasTriggered && !clips[i].HasFinished)
                    clips[i].Pause();
            }
        }

        public override void Resume()
        {
            for (int i = 0; i != clips.Count; ++i)
            {
                if (clips[i].HasTriggered && !clips[i].HasFinished)
                    clips[i].Resume();
            }
        }

        public override void Stop()
        {
            for (int i = clips.Count - 1; i >= 0; --i)
            {
                clips[i].GraphStop();
            }

            currentClipIndex = 0;
        }

        public override void UpdateActions(int _frame, float _time)
        {
            int limit = clips.Count;

            if (limit == 0)
                return;
            int increment = 1;

            if (!Playable.IsPlayingForward)
            {
                limit = -1;
                increment = -1;
            }

            if (Playable.IsPlayingForward)
            {
                if (_frame < clips[currentClipIndex].Start)
                {
                    for (int i = clips.Count - 1; i >= 0; i--)
                    {
                        if (_frame > clips[i].Start)
                            currentClipIndex = i;
                    }
                }
            }
            else
            {
                if (_frame > clips[currentClipIndex].End)
                {
                    for (int i = 0; i < clips.Count; i++)
                    {
                        if (_frame < clips[i].End)
                            currentClipIndex = i;
                    }
                }
            }
            for (int i = currentClipIndex; i != limit; i += increment)
            {
                ITLAction clip = clips[i];
                if (_frame < clip.Start)
                {
                    // 正向播放与反向播放的处理
                    if (Playable.IsPlayingForward)
                    {
                        if (clip.HasTriggered)
                            clip.ActionStop();
                        break;
                    }
                    else
                    {
                        if (!clip.HasFinished && (clip.HasTriggered || clip.TriggerOnSkip))
                            clip.UpdateAction(clip.FrameCount, clip.Duration);

                        currentClipIndex = Mathf.Clamp(i - 1, 0, clips.Count - 1);
                    }
                }
                else if (_frame >= clip.Start && _frame <= clip.End)
                {
                    if (clip.HasFinished && Playable.FrameChanged)
                        clip.ActionStop();

                    if (!clip.HasFinished)
                    {
                        int frameSinceActionStart = _frame - clip.Start;
                        float timeSinceActionStart = _time - clip.StartTime;

                        clip.UpdateAction(frameSinceActionStart, timeSinceActionStart);
                    }
                }
                else //if(_frame > clip.end) // 如果完成
                {
                    // 正向播放与反向播放的处理
                    if (Playable.IsPlayingForward)
                    {
                        if (!clip.HasFinished && (clip.HasTriggered || clip.TriggerOnSkip))
                            clip.UpdateAction(clip.FrameCount, clip.Duration);

                        currentClipIndex = Mathf.Clamp(i + 1, 0, clips.Count - 1);
                    }
                    else
                    {
                        if (clip.HasTriggered)
                            clip.ActionStop();
                        break;
                    }
                }
            }
        }

        public override void Reset()
        {
            currentClipIndex = 0;
            foreach (var clip in clips)
            {
                clip.Reset();
            }
        }

        public override void SpeedChanged(float _speed)
        {
            foreach (var clip in clips)
            {
                clip.SpeedChanged(_speed);
            }
        }

        public override int GetFrameCount()
        {
            return clips[clips.Count - 1].End;
        }
        #endregion
    }
}