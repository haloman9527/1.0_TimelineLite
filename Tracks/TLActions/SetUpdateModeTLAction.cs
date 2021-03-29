using CZToolKit.Core;
using System;
using UnityEngine;

namespace CZToolKit.TimelineLite
{
    [Serializable]
    public class SetUpdateModeTLAction : TLAction<SetUpdateModeTLActionData>
    {
        PlayableUpdateMode updateMode;

        /// <summary> 不能删 </summary>
        public SetUpdateModeTLAction() { }

        /// <summary> 不能删 </summary>
        public SetUpdateModeTLAction(ITLTrack _track, SetUpdateModeTLActionData _actionData) : base(_track, _actionData) { }

        protected override void OnGraphStart()
        {
            updateMode = Playable.UpdateMode;
        }

        protected override void OnUpdateAction(float _timeSinceActionStart)
        {
            Playable.UpdateMode = TActionData.updateMode;
        }

        protected override void OnActionFinish()
        {
            Playable.UpdateMode = updateMode;
        }
    }
}