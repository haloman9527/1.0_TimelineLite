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
using UnityEngine;
using UnityEngine.Timeline;

namespace Atom.TimelineLite.Editors
{
    public interface ITLBasicTrackAsset
    {
        TLBasicTrackData CreateTrackData();
    }

    public interface ITLBasicTrackAssetEditorEnter
    {
        void Enter();
    }

    public abstract class TLBasicTrackAsset : TrackAsset, ITLBasicTrackAsset
    {
        [TextArea]
        public string description;

        public abstract TLBasicTrackData CreateTrackData();
    }
}
