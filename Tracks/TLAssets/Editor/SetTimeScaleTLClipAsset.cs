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
using System;
using UnityEngine;

namespace CZToolKit.TimelineLite.Editors
{
    [Serializable]
    public class SetTimeScaleTLClipAsset : TLBasicClipAsset<SetTimeScaleTLActionData>
    {
        [Range(0, 1)]
        public float timeScale = 1;

        /// <summary> 创建<see cref="SetTimeScaleTLActionData" />对象 </summary>
        public override TLActionData CreateActionData()
        {
            SetTimeScaleTLActionData actionData = new SetTimeScaleTLActionData();
            // 在此进行对应的数值设置
            actionData.timeScale = timeScale;

            return actionData;
        }
    }
}