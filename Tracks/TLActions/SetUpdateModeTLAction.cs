using CZToolKit.Core;
using System;
using UnityEngine;

namespace CZToolKit.TimelineLite
{
    [Serializable]
    public class SetUpdateModeTLAction : TLAction<SetUpdateModeTLActionData>
    {
        UpdateMode updateMode;

        /// <summary> 不能删 </summary>
        public SetUpdateModeTLAction() { }

        /// <summary> 不能删 </summary>
        public SetUpdateModeTLAction(ITLTrack _track, SetUpdateModeTLActionData _actionData) : base(_track, _actionData) { }

        protected override void OnGraphStart()
        {
            updateMode = Master.UpdateMode;
        }

        protected override void OnUpdateAction(float _timeSinceActionStart)
        {
            Master.UpdateMode = TActionData.updateMode;
        }

        protected override void OnActionFinish()
        {
            Master.UpdateMode = updateMode;
        }

        protected override void OnActionStop()
        {
            Master.UpdateMode = updateMode;
        }
    }
}