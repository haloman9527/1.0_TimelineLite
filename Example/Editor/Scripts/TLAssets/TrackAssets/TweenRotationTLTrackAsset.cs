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
using Atom.TimelineLite;
using System;
using Atom.TimelineLite.Editors;
using UnityEngine.Timeline;

namespace Atom.TimelineLite.Example
{
    [Serializable]
    [TrackColor(0.96f, 0.44f, 0.47f)]
    [TLTrackMinHeight(20)]
    [TLTrackAssetMenuItem("Example/插值旋转")]
    [TrackClipType(typeof(TweenRotationTLClipAsset))]
    public class TweenRotationTLTrackAsset : TLBasicTrackAsset
    {
        // 若有需求可返回自定义的轨道数据对象
        public override TLBasicTrackData CreateTrackData()
        {
            TLBasicTrackData trackData = new TLBasicTrackData();

            return trackData;
        }
    }
}
