using CZToolKit.Core;
using UnityEngine;

namespace CZToolKit.TimelineLite.Example
{
    public class TweenPositionTLAction : TLAction<TweenPositionTLActionData>
    {
        public TweenPositionTLAction() { }

        public TweenPositionTLAction(ITLTrack _track, TweenPositionTLActionData _logicData) : base(_track, _logicData) { }

        protected override void OnUpdateAction(float _timeSinceActionStart)
        {
            float t = _timeSinceActionStart / Duration;
            Playable.transform.position = new Vector3(
                Easing.Tween(TActionData.startPosition.x, TActionData.endPosition.x, t, TActionData.ease),
                Easing.Tween(TActionData.startPosition.y, TActionData.endPosition.y, t, TActionData.ease),
                Easing.Tween(TActionData.startPosition.z, TActionData.endPosition.z, t, TActionData.ease)
                );
        }
    }
}
