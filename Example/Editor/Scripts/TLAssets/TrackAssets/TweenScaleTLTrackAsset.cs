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
using Moyo.TimelineLite;
using System;
using Moyo.TimelineLite.Editors;
using UnityEngine.Timeline;

namespace Moyo.TimelineLite.Example
{
    [Serializable]
    [TrackColor(0f, 0f, 0.5f)]
    [TLTrackMinHeight(20)]
    [TLTrackAssetMenuItem("Example/插值缩放")]
    [TrackClipType(typeof(TweenScaleTLClipAsset))]
    public class TweenScaleTLTrackAsset : TLBasicTrackAsset
    {
        // 若有需求可返回自定义的轨道数据对象
        public override TLBasicTrackData CreateTrackData()
        {
            TLBasicTrackData trackData = new TLBasicTrackData();

            return trackData;
        }
    }
}
