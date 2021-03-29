// 请将此脚本放在Editor目录下
using CZToolKit.TimelineLite;
using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace CZToolKit.TimelineLite
{
    [Serializable]
    public class SetTimeScaleTLClipAsset : TLBasicClipAsset<SetTimeScaleTLActionData>
    {
        [Range(0, 1)]
        public float timeScale = 1;

        /// <summary> 创建<see cref="SetTimeScaleTLActionData" />对象 </summary>
        public override TLActionData CreateActionData()
        {
            SetTimeScaleTLActionData actionData = new SetTimeScaleTLActionData();
            // 在此进行对应的数值设置
            actionData.timeScale = timeScale;

            return actionData;
        }

        protected override void OnDrawGizmos_Lite(PlayableDirectorLite _playable, TimelineClip _timelineClip)
        {

        }

        protected override void OnDrawGizmosSelected_Lite(PlayableDirectorLite _playable, TimelineClip _timelineClip)
        {

        }
    }
}