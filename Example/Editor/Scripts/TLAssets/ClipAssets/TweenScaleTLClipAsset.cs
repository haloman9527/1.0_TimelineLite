using CZToolKit.Core;
using CZToolKit.TimelineLite.Editors;
using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace CZToolKit.TimelineLite.Example
{
    [Serializable]
    public class TweenScaleTLClipAsset : TLBasicClipAsset<TweenScaleTLActionData>
    {
        public Vector3 from, to;
        public EasingType ease = EasingType.Linear;

        /// <summary> 创建<see cref="TweenScaleTLActionData" />对象 </summary>
        public override TLActionData CreateActionData()
        {
            TweenScaleTLActionData actionData = new TweenScaleTLActionData();
            // 在此进行对应的数值设置
            actionData.from = from;
            actionData.to = to;
            actionData.ease = ease;

            return actionData;
        }
    }
}