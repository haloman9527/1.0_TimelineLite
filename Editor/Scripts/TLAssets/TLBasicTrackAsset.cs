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
using UnityEngine.Timeline;

namespace CZToolKit.TimelineLite.Editors
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
