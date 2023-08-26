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
// 请将此脚本放在Editor目录下
using CZToolKit;
using System;

namespace CZToolKit.TimelineLite.Editors
{
    [Serializable]
    public class SetUpdateModeTLClipAsset : TLBasicClipAsset<SetUpdateModeTLActionData>
    {
        public UpdateMode updateMode;
        /// <summary> 创建<see cref="SetUpdateModeTLActionData" />对象 </summary>
        public override TLActionData CreateActionData()
        {
            SetUpdateModeTLActionData actionData = new SetUpdateModeTLActionData();
            // 在此进行对应的数值设置
            actionData.updateMode = updateMode;

            return actionData;
        }
    }
}