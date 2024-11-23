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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jiange.TimelineLite.Example
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