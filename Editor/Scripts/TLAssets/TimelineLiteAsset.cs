using CZToolKit.Core.Attributes;
using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace CZToolKit.TimelineLite.Editors
{
    [CreateAssetMenu(menuName = "CZToolKit/TimelineLite/New TimelineLiteAsset", fileName = "New TimelineLiteAsset")]
    public class TimelineLiteAsset : TimelineAsset
    {
        [TextArea(minLines: 3, maxLines: 7)]
        public string Description = "";

        /// <summary> 上次导出的存储位置 </summary>
        [ReadOnly]
        public string previousPath = "";
        [Tooltip("是否循环")]
        public bool loop = false;

        public virtual Type TargetSOType { get { return typeof(TimelineLiteSO); } }
        public virtual Type TargetObjectType { get { return typeof(TimelineLiteObject<TimelineLiteObjectData>); } }
        public virtual Type TargetDataType { get { return typeof(TimelineLiteObjectData); } }

        public TimelineLiteObjectData Extract()
        {
            TimelineLiteObjectData timelineData = Activator.CreateInstance(TargetDataType) as TimelineLiteObjectData;
            timelineData.Loop = loop;
            timelineData.FrameRate = this.editorSettings.fps;
            timelineData.FrameCount = (int)this.GetFrameCount();

            foreach (var trackAsset in GetRootTracks())
            {
                if (!(trackAsset is GroupTrack) && trackAsset.isEmpty) continue;
                TLTrackData trackData = GetTrack(trackAsset);
                if (trackData != null)
                    timelineData.Tracks.Add(trackData);
            }

            ExtractPostProcess(timelineData);
            return timelineData;
        }

        protected virtual void ExtractPostProcess(TimelineLiteObjectData _data) { }

        private TLTrackData GetTrack(TrackAsset trackAsset)
        {
            Type trackAssetType = trackAsset.GetType();
            if (trackAssetType == typeof(GroupTrack))
            {
                GroupTrack groupTrack = trackAsset as GroupTrack;
                TLGroupTrackData group = new TLGroupTrackData();
                group.enabled = !trackAsset.muted;
                group.name = trackAsset.name;

                foreach (TrackAsset childTrack in groupTrack.GetChildTracks())
                {
                    TLTrackData track = GetTrack(childTrack);
                    if (track != null)
                        group.ChildTracks.Add(track);
                }

                if (group.ChildTracks.Count == 0)
                    return null;
                return group;
            }
            else if (typeof(TLBasicTrackAsset).IsAssignableFrom(trackAssetType))
            {
                TLBasicTrackAsset basicTrackAsset = trackAsset as TLBasicTrackAsset;

                // 创建Track对象
                TLBasicTrackData basicTrackData = basicTrackAsset.CreateTrackData();
                basicTrackData.enabled = !basicTrackAsset.muted;
                basicTrackData.name = basicTrackAsset.name;
                // 遍历Track的所有片段
                foreach (TimelineClip clip in basicTrackAsset.GetClips())
                {
                    TLBasicClipAsset clipAsset = clip.asset as TLBasicClipAsset;
                    TLActionData actionData = clipAsset.CreateActionData();

                    ActionBaseInfo actionBaseInfo = new ActionBaseInfo();
                    actionBaseInfo.name = clip.displayName;
                    actionBaseInfo.triggerOnSkip = clipAsset.TriggerOnSkip;
                    actionBaseInfo.start = (int)clip.GetStartFrame();
                    actionBaseInfo.end = (int)clip.GetEndFrame();
                    actionBaseInfo.frameCount = (int)clip.GetFrameCount();
                    actionBaseInfo.startTime = (float)clip.start;
                    actionBaseInfo.endTime = (float)clip.end;
                    actionBaseInfo.duration = actionBaseInfo.endTime - actionBaseInfo.startTime;
                    actionData.ActionBaseInfo = actionBaseInfo;

                    // 获取类型片段，并添加到Track对象
                    basicTrackData.Clips.Add(actionData);
                }

                return basicTrackData;
            }
            else
            {
                return CustomCreateTrackData(trackAsset);
            }
        }

        protected virtual TLTrackData CustomCreateTrackData(TrackAsset trackAsset) { return null; }
    }
}