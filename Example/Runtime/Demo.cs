using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CZToolKit.TimelineLite.Example
{
    public class Demo : MonoBehaviour
    {
        public TimelineLiteSO timeline;
        public float maunalDelta = 0.02f;

        private void OnEnable()
        {
            if (timeline == null) return;

            GetComponent<PlayableDirectorLite>().Play(new TimelineLiteObject<TimelineLiteObjectData>(timeline.TimelineLiteObjectData));
        }

        private void Update()
        {
            GetComponent<PlayableDirectorLite>().Evaluate(maunalDelta);
        }
    }
}