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
using UnityEngine;

namespace CZToolKit.TimelineLite
{
    [SerializeField]
#if ODIN_INSPECTOR
    public class TimelineLiteSO : Sirenix.OdinInspector.SerializedScriptableObject
#else
    public class TimelineLiteSO : ScriptableObject
#endif
    {
#if UNITY_2019_1_OR_NEWER
        [SerializeReference]
#endif
        [SerializeField]
        protected TimelineLiteObjectData timelineLiteObjectData;
        public virtual TimelineLiteObjectData TimelineLiteObjectData { get { return timelineLiteObjectData; } set { timelineLiteObjectData = value; } }
    }
}