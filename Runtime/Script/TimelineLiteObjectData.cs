using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace CZToolKit.TimelineLite
{
    [Serializable]
#if ODIN_INSPECTOR
    [HideReferenceObjectPicker]
#endif
    public class TimelineLiteObjectData
    {
        public const int DEFAULT_FRAME_RATE = 60;
        public const int DEFAULT_DURATION = 10;

        [SerializeField] bool loop;
        /// <summary> 帧数 </summary>
        [SerializeField] int frameCount = DEFAULT_FRAME_RATE * DEFAULT_DURATION;
        /// <summary> 帧率 </summary>
        [SerializeField] float frameRate = DEFAULT_FRAME_RATE;
        /// <summary> 帧耗时 </summary>
        [SerializeField] float frameRateSecond = 1f / DEFAULT_FRAME_RATE;
        /// <summary> 轨道 </summary>
        [SerializeField]
#if UNITY_2019_1_OR_NEWER
        [SerializeReference]
# endif
        List<TLTrackData> tracks = new List<TLTrackData>();

        public bool Loop { get { return loop; } set { loop = value; } }
        public int FrameCount { get { return frameCount; } set { frameCount = value; } }
        public float FrameRate
        {
            get { return frameRate; }
            set
            {
                frameRate = value;
                frameRateSecond = 1f / frameRate;
            }
        }
        public float FrameRateSecond { get { return frameRateSecond; } }
        public List<TLTrackData> Tracks { get { return tracks; } }

        public TimelineLiteObjectData() { }
    }
}