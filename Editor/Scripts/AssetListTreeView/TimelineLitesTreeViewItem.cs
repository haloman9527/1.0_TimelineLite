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
using UnityEditor.IMGUI.Controls;

namespace Jiange.TimelineLite.Editors
{
    public class TimelineLiteAssetTreeViewItem : TreeViewItem
    {
        string path;
        TimelineLiteAsset userData;

        public string Path { get { return path; } set { path = value; } }
        public TimelineLiteAsset UserData { get { return userData; } }

        public TimelineLiteAssetTreeViewItem(TimelineLiteAsset _userData)
        {
            userData = _userData;
        }

        public TimelineLiteAssetTreeViewItem(int id, TimelineLiteAsset _userData) : base(id)
        {
            userData = _userData;
        }

        public TimelineLiteAssetTreeViewItem(int id, int depth, TimelineLiteAsset _userData) : base(id, depth)
        {
            userData = _userData;
        }

        public TimelineLiteAssetTreeViewItem(int id, int depth, string displayName, TimelineLiteAsset _userData) : base(id, depth, displayName)
        {
            userData = _userData;
        }
    }
}
