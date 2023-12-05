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
 *  Blog: https://www.mindgear.net/
 *
 */
#endregion
using System;
using UnityEngine.Playables;
using UnityEngine;

namespace CZToolKit.TimelineLite
{
    /// <summary> Action基本信息 </summary>
    [Serializable]
    public struct ActionBaseInfo
    {
        public string name;
        public int start;
        public int frameCount;
        public int end;
        public float startTime;
        public float duration;
        public float endTime;
    }

    /// <summary> 逻辑片段数据提供者抽象基类 </summary>
    [Serializable]
    public abstract class TLActionData : PlayableBehaviour
    {
        [SerializeField, HideInInspector]
        ActionBaseInfo actionBaseInfo;

        public bool triggerOnSkip = true;
        public ActionBaseInfo ActionBaseInfo { get { return actionBaseInfo; } set { actionBaseInfo = value; } }
    }
}