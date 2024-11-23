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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jiange.TimelineLite
{
    /// <summary> 组轨道 </summary>
    public class TLGroupTrack : TLTrack<TLGroupTrackData>
    {
        [SerializeField]
        private List<ITLTrack> childTracks = new List<ITLTrack>();

        public TLGroupTrack() { }

        public TLGroupTrack(ITimelineLiteObject _timelineLiteObject, TLGroupTrackData _trackData) : base(_timelineLiteObject, _trackData) { }

        public List<ITLTrack> ChildTracks { get { return childTracks; } }

        public override void Initialize(PlayableDirectorLite _owner)
        {
            foreach (ITLTrack track in childTracks)
            {
                track.Initialize(_owner);
            }
        }

        public override void Start()
        {
            foreach (ITLTrack track in childTracks)
            {
                track.Start();
            }
        }

        public override void Pause()
        {
            foreach (ITLTrack track in childTracks)
            {
                if (Enabled)
                    track.Pause();
            }
        }

        public override void Resume()
        {
            foreach (ITLTrack track in childTracks)
            {
                if (Enabled)
                    track.Resume();
            }
        }

        public override void Stop()
        {
            foreach (ITLTrack track in childTracks)
            {
                track.Stop();
            }
        }

        public override void UpdateActions(int _frame, float _time)
        {
            foreach (ITLTrack track in childTracks)
            {
                if (Enabled)
                    track.UpdateActions(_frame, _time);
            }
        }

        public override void Reset()
        {
            foreach (ITLTrack track in childTracks)
            {
                track.Reset();
            }
        }

        public override void SpeedChanged(float _speed)
        {
            foreach (ITLTrack track in childTracks)
            {
                track.SpeedChanged(_speed);
            }
        }

        public override int GetFrameCount()
        {
            int frameCount = 0;
            foreach (var track in childTracks)
            {
                if (track.Enabled && track.GetFrameCount() > frameCount)
                    frameCount = track.GetFrameCount();
            }
            return frameCount;
        }
    }
}
