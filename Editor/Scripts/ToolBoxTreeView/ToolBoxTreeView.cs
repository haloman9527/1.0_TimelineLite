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
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace CZToolKit.TimelineLite.Editors
{
    public class ToolboxTreeView : TreeView
    {
        List<ToolboxTreeViewItem> items = new List<ToolboxTreeViewItem>();

        public ToolboxTreeView(TreeViewState state) : base(state)
        {
            showBorder = true;
            showAlternatingRowBackgrounds = true;
        }

        public ToolboxTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader) : base(state, multiColumnHeader)
        {
            showBorder = true;
            showAlternatingRowBackgrounds = true;
        }

        protected override TreeViewItem BuildRoot()
        {
            TreeViewItem root = new TreeViewItem(-1, -1, "Root");
            root.children = new List<TreeViewItem>();

            int id = 0;
            foreach (var item in items)
            {
                if (string.IsNullOrEmpty(item.Path))
                {
                    item.id = id;
                    id++;
                    item.depth = 0;
                    root.AddChild(item);
                }
                else
                {
                    string[] path = item.Path.Split('/');
                    TreeViewItem currentLayer = root;
                    for (int i = 0; i < path.Length; i++)
                    {
                        TreeViewItem child = currentLayer.children.Find(l => l.displayName == path[i]);
                        if (child == null)
                        {
                            child = new ToolboxTreeViewItem(id, i, path[i]);
                            child.children = new List<TreeViewItem>();
                            id++;
                            currentLayer.AddChild(child);
                        }
                        currentLayer = child;
                    }
                    item.depth = path.Length;
                    item.id = id;
                    id++;
                    currentLayer.AddChild(item);
                }
            }

            SetupDepthsFromParentsAndChildren(root);
            return root;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            base.RowGUI(args);
            (args.item as ToolboxTreeViewItem).OnGUI(args.rowRect);
        }

        public override void OnGUI(Rect rect)
        {
            GUI.Box(rect, "");
            base.OnGUI(rect);
        }

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            return false;
        }

        protected override float GetCustomRowHeight(int row, TreeViewItem item)
        {
            return (item as ToolboxTreeViewItem).Height;
        }

        public void AddItem<T>(T item) where T : ToolboxTreeViewItem
        {
            AddItem("", item);
        }

        public void AddItem<T>(string path, T item) where T : ToolboxTreeViewItem
        {
            item.Path = path;
            items.Add(item);
        }
    }
}
