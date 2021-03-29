using UnityEngine;

namespace CZToolKit.TimelineLite
{
    [SerializeField]
    public class TimelineLiteSO : ScriptableObject
    {
#if UNITY_2019_1_OR_NEWER
        [SerializeReference]
#endif
        [SerializeField]
        protected TimelineLiteObjectData timelineLiteObjectData;
        public virtual TimelineLiteObjectData TimelineLiteObjectData { get { return timelineLiteObjectData; } set { timelineLiteObjectData = value; } }
    }
}