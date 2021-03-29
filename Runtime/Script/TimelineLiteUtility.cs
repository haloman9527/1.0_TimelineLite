using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace CZToolKit.TimelineLite
{
    public static class TimelineLiteUtility
    {

        /// <summary> 帧的算法,向下贴近,超过整数的小数不计 </summary>
        public static int ConvertToFrameIndex(float _frame)
        {
            return Mathf.FloorToInt(_frame);
        }

        /// <summary> 帧数量的算法,四舍五入 </summary>
        public static int ConvertToFrameCount(float _f)
        {
            return Mathf.RoundToInt(_f);
        }

        public static float GetStartFrame(this TimelineClip timelineClip)
        {
            if (timelineClip.parentTrack == null) return 0;
            return ConvertToFrameIndex((float)timelineClip.start * timelineClip.parentTrack.timelineAsset.editorSettings.fps);
        }

        public static float GetEndFrame(this TimelineClip timelineClip)
        {
            if (timelineClip.parentTrack == null) return 0;
            return ConvertToFrameIndex((float)timelineClip.end * timelineClip.parentTrack.timelineAsset.editorSettings.fps);
        }

        public static float GetFrameCount(this TimelineClip timelineClip)
        {
            if (timelineClip.parentTrack == null) return 0;
            return ConvertToFrameCount(timelineClip.parentTrack.timelineAsset.editorSettings.fps * (float)timelineClip.duration);
        }

        public static float GetFrameCount(this TimelineAsset timelineAsset)
        {
            return ConvertToFrameCount(timelineAsset.editorSettings.fps * (float)timelineAsset.duration);
        }
    }
}
