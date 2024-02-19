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
using System;
using UnityEngine;

namespace CZToolKit.TimelineLite.Example
{
    [Serializable]
    public class TweenScaleTLAction : TLAction<TweenScaleTLActionData>
    {
        /// <summary> 不能删 </summary>
        public TweenScaleTLAction()
        {

        }

        /// <summary> 不能删 </summary>
        public TweenScaleTLAction(ITLTrack _track, TweenScaleTLActionData _actionData) : base(_track, _actionData)
        {

        }

        protected override void OnUpdateAction(float _timeSinceActionStart)
        {
            float t = _timeSinceActionStart / Duration;
            Master.transform.localScale = new Vector3(
                Easing.Tween(TActionData.from.x, TActionData.to.x, t, TActionData.ease),
                Easing.Tween(TActionData.from.y, TActionData.to.y, t, TActionData.ease),
                Easing.Tween(TActionData.from.z, TActionData.to.z, t, TActionData.ease)
                );
        }
    }
}