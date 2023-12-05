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
 *  Blog: https://www.mindgear.net/
 *
 */
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Timeline;

namespace CZToolKit.TimelineLite
{
    public static class TimelineLiteUtility
    {
        private static bool s_Initialized;
        private static Dictionary<Type, Type> s_ActionDataDict;

        static TimelineLiteUtility()
        {
            Init(true);
        }

        public static void Init(bool force)
        {
            if (!force && s_Initialized)
            {
                return;
            }

            if (s_ActionDataDict == null)
                s_ActionDataDict = new Dictionary<Type, Type>();
            else
                s_ActionDataDict.Clear();
            
            foreach (var actionType in Util_TypeCache.GetTypesDerivedFrom<ITLAction>())
            {
                if (actionType.IsGenericType || actionType.IsAbstract) continue;
                var actionDataType = actionType.GetProperty("TActionData",
                    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).PropertyType;
                s_ActionDataDict[actionDataType] = actionType;
            }

            s_Initialized = true;
        }
        
        public static ITLTrack GetTrack(ITimelineLiteObject timelineLiteObject, TLTrackData trackData)
        {
            if (trackData is TLGroupTrackData groupTrackData)
            {
                var groupTrack = new TLGroupTrack(timelineLiteObject, trackData as TLGroupTrackData);
                foreach (TLTrackData childTrack in groupTrackData.ChildTracks)
                {
                    var track = GetTrack(timelineLiteObject, childTrack);
                    if (track != null)
                        groupTrack.ChildTracks.Add(track);
                }

                return groupTrack;
            }
            else if(trackData is TLBasicTrackData basicTrackData)
            {
                var basicTrack = new TLBasicTrack(timelineLiteObject, basicTrackData);
                foreach (TLActionData actionData in basicTrackData.Clips)
                {
                    Type actionType;
                    if (s_ActionDataDict.TryGetValue(actionData.GetType(), out actionType))
                    {
                        ITLAction action =
                            Activator.CreateInstance(actionType, basicTrack, actionData) as ITLAction;
                        if (action != null)
                            basicTrack.Clips.Add(action);
                    }
                }
                return basicTrack;
            }

            return null;
        }

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
