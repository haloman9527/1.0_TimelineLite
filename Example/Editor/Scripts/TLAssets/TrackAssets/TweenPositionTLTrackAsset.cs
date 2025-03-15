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

using System;
using Atom.TimelineLite.Editors;
using UnityEngine.Timeline;

namespace Atom.TimelineLite.Example
{
    [Serializable]
    [TrackColor(0f, 0.5f, 0f)]
    [TLTrackMinHeight(20)]
    [TLTrackAssetMenuItem("Example/插值位移")]
    [TrackClipType(typeof(TweenPositionTLClipAsset))]
    public class TweenPositionTLTrackAsset : TLBasicTrackAsset
    {
        public override TLBasicTrackData CreateTrackData()
        {
            TLBasicTrackData trackData = new TLBasicTrackData();
            return trackData;
        }
    }
}
