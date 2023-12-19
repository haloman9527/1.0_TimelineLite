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
 *  Blog: https://www.mindgear.net/
 *
 */
#endregion
using System;
using CZToolKitEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace CZToolKit.TimelineLite.Editors
{
    public class ToolboxTreeViewItem : TreeViewItem
    {
        #region 静态私有变量
        static GUIStyle separatorStyle;
        #endregion

        #region 静态公共属性
        public static GUIStyle SeparatorStyle
        {
            get
            {
                if (separatorStyle == null)
                {
                    separatorStyle = new GUIStyle();
                    separatorStyle.normal.background = EditorStylesExtension.MakeTex(1, 1, Color.green);
                }
                return separatorStyle;
            }
        }
        #endregion

        #region 静态方法
        public static ToolboxTreeViewItem GetSeparator(int _height = 10)
        {
            ToolboxTreeViewItem separator = GetItem((rect, item) => { GUI.Box(rect, "", ToolboxTreeViewItem.SeparatorStyle); });
            separator.Height = _height;
            return separator;
        }

        public static ToolboxTreeViewItem GetItem(Action<Rect, ToolboxTreeViewItem> _drawer)
        {
            return GetItem("", _drawer);
        }

        public static ToolboxTreeViewItem GetItem(string path, Action<Rect, ToolboxTreeViewItem> _drawer)
        {
            ToolboxTreeViewItem toolBoxItem = new ToolboxTreeViewItem();
            toolBoxItem.drawer = _drawer;
            toolBoxItem.depth = 0;
            toolBoxItem.Path = path;
            return toolBoxItem;
        }
        #endregion

        #region 私有变量
        string path;
        Action<Rect, ToolboxTreeViewItem> drawer;
        float height = 20;
        #endregion

        #region 公共属性
        public string Path { get { return path; } set { path = value; } }
        public virtual float Height { get { return height; } set { height = value; } }
        #endregion

        public ToolboxTreeViewItem() { }

        public ToolboxTreeViewItem(int id) : base(id) { }

        public ToolboxTreeViewItem(int id, int depth) : base(id, depth) { }

        public ToolboxTreeViewItem(int id, int depth, string displayName) : base(id, depth, displayName) { }

        public virtual void OnGUI(Rect _position)
        {
            if (drawer != null)
                drawer.Invoke(_position, this);
        }
    }
}
