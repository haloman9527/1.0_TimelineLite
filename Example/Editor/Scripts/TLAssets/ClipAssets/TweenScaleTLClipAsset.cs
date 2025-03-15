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
using Atom;
using System;
using Atom.TimelineLite.Editors;
using UnityEngine;
using UnityEngine.Timeline;

namespace Atom.TimelineLite.Example
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