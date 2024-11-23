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
using Jiange;
using System;
using UnityEngine;

namespace Jiange.TimelineLite
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