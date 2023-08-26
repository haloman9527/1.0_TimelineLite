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
 *  Blog: https://www.crosshair.top/
 *
 */
#endregion
using System;
using UnityEngine;

namespace CZToolKit.TimelineLite.Example
{
    [Serializable]
    public class TweenRotationTLAction : TLAction<TweenRotationTLActionData>
    {
        /// <summary> 不能删 </summary>
        public TweenRotationTLAction()
        {

        }

        /// <summary> 不能删 </summary>
        public TweenRotationTLAction(ITLTrack _track, TweenRotationTLActionData _actionData) : base(_track, _actionData)
        {

        }

        protected override void OnUpdateAction(float _timeSinceActionStart)
        {
            float t = _timeSinceActionStart / Duration;
            Master.transform.rotation = Quaternion.Euler(new Vector3(
                Easing.Tween(TActionData.from.x, TActionData.to.x, t, TActionData.ease),
                Easing.Tween(TActionData.from.y, TActionData.to.y, t, TActionData.ease),
                Easing.Tween(TActionData.from.z, TActionData.to.z, t, TActionData.ease)
                ));
        }
    }
}