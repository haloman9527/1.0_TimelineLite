// 请将此脚本放在Editor目录下
using CZToolKit.Core;
using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace CZToolKit.TimelineLite
{
    [Serializable]
    public class SetUpdateModeTLClipAsset : TLBasicClipAsset<SetUpdateModeTLActionData>
    {
        public PlayableUpdateMode updateMode;
        /// <summary> 创建<see cref="SetUpdateModeTLActionData" />对象 </summary>
        public override TLActionData CreateActionData()
        {
            SetUpdateModeTLActionData actionData = new SetUpdateModeTLActionData();
            // 在此进行对应的数值设置
            actionData.updateMode = updateMode;

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