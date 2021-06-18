﻿using CZToolKit.Core;
using UnityEngine;

namespace CZToolKit.TimelineLite.Example
{
    public class TweenPositionTLAction : TLAction<TweenPositionTLActionData>
    {
        public TweenPositionTLAction() { }

        public TweenPositionTLAction(ITLTrack _track, TweenPositionTLActionData _actionData) : base(_track, _actionData) { }

        protected override void OnUpdateAction(float _timeSinceActionStart)
        {
            float t = _timeSinceActionStart / Duration;
            Master.transform.position = new Vector3(
                Easing.Tween(TActionData.startPosition.x, TActionData.endPosition.x, t, TActionData.ease),
                Easing.Tween(TActionData.startPosition.y, TActionData.endPosition.y, t, TActionData.ease),
                Easing.Tween(TActionData.startPosition.z, TActionData.endPosition.z, t, TActionData.ease)
                );
        }
    }
}