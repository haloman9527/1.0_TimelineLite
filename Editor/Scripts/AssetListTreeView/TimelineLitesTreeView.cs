using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace CZToolKit.TimelineLite.Editors
{
    public class TimelineLitesTreeView : TreeView
    {
        GUIContent playIcon;

        SearchMode filtterMode;
        string filtter;
        public TimelineLiteEditorWindow Window;

        List<TimelineLiteAssetTreeViewItem> items = new List<TimelineLiteAssetTreeViewItem>(16);

        public TimelineLitesTreeView(TreeViewState state, string _filtter, TimelineLiteEditorWindow _window) : base(state)
        {
            rowHeight = 20;
            filtter = _filtter;
            Window = _window;
            showBorder = true;
            showAlternatingRowBackgrounds = true;

            playIcon = new GUIContent(EditorGUIUtility.FindTexture("PlayButton"), "播放");
        }

        public TimelineLitesTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader, string _filtter,
            TimelineLiteEditorWindow _window) : base(state, multiColumnHeader)
        {
            rowHeight = 20;
            filtter = _filtter;
            Window = _window;
            showBorder = true;
            showAlternatingRowBackgrounds = true;

            playIcon = new GUIContent(EditorGUIUtility.FindTexture("PlayButton"), "播放");
        }

        protected override TreeViewItem BuildRoot()
        {
            TreeViewItem root = new TreeViewItem(-1, -1, "Root");
            root.children = new List<TreeViewItem>();
            FiltterMethod filtterMethod = sk => true;
            
            switch (filtterMode)
            {
                case SearchMode.StartsWith:
                    filtterMethod = sk => sk.name.StartsWith(filtter);
                    break;
                case SearchMode.Contains:
                    filtterMethod = sk => sk.name.Contains(filtter);
                    break;
                case SearchMode.EndsWith:
                    filtterMethod = sk => sk.name.EndsWith(filtter);
                    break;
            }


            int id = 0;
            foreach (var item in items)
            {
                if (!string.IsNullOrEmpty(filtter) && !filtterMethod(item.UserData)) continue;
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
                            child = new TreeViewItem(id, i, path[i]);
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
            TimelineLiteAssetTreeViewItem assetItem = args.item as TimelineLiteAssetTreeViewItem;
            if (assetItem == null)
            {
                base.RowGUI(args);
                return;
            }
            if (args.isRenaming)
                return;
            TimelineLiteAsset asset = assetItem.UserData;

            Rect rect = args.rowRect;
            rect.x = 20;
            rect.width = args.rowRect.width - 20;
            GUI.Label(rect, asset.name);

            rect.x = args.rowRect.width - 25;
            rect.width = 25;
            if (!EditorApplication.isPlaying || TimelineLiteEditorWindow.Playable == null)
                EditorGUI.BeginDisabledGroup(true);

            if (GUI.Button(rect, playIcon, (GUIStyle)"toolbarbutton"))
            {
                TimelineLiteEditorWindow.Playable.Play(
                    Activator.CreateInstance(asset.TargetObjectType, asset.Extract()) as ITimelineLiteObject);
                Repaint();
            }

            if (!EditorApplication.isPlaying || TimelineLiteEditorWindow.Playable == null)
                EditorGUI.EndDisabledGroup();
        }

        protected override void ContextClickedItem(int id)
        {
            IList<int> selection = GetSelection();
            GenericMenu genericMenu = new GenericMenu();
            Window.BuildAssetContextMenu(selection, genericMenu);
            genericMenu.ShowAsContext();
        }

        protected override void DoubleClickedItem(int id)
        {
            TimelineLiteAssetTreeViewItem item = FindItem(id, rootItem) as TimelineLiteAssetTreeViewItem;
            if (item == null) return;
            TimelineLiteAsset data = item.UserData;
            Selection.activeObject = data;
        }

        public TreeViewItem FindItem(int id)
        {
            return FindItem(id, rootItem);
        }

        protected override bool CanRename(TreeViewItem item)
        {
            return item as TimelineLiteAssetTreeViewItem != null;
        }

        protected override void RenameEnded(RenameEndedArgs args)
        {
            TimelineLiteAssetTreeViewItem item = FindItem(args.itemID, rootItem) as TimelineLiteAssetTreeViewItem;
            if (item == null) return;
            TimelineLiteAsset data = item.UserData;
            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(data), args.newName);
            item.displayName = data.name;
        }

#if UNITY_2018_1_OR_NEWER
        protected override void SingleClickedItem(int id)
        {
            base.SingleClickedItem(id);
            TimelineLiteAssetTreeViewItem item = FindItem(id, rootItem) as TimelineLiteAssetTreeViewItem;
            if (item == null) return;
            TimelineLiteAsset data = item.UserData;
            if (data != null && UnityEditor.Timeline.TimelineEditor.inspectedAsset != data)
            {
                AssetDatabase.OpenAsset(data);
                Window.Focus();
            }
        }
#endif

#if !UNITY_2018_1_OR_NEWER
        protected override void SelectionChanged(IList<int> selectedIds)
        {
            base.SelectionChanged(selectedIds);
            if (selectedIds.Count <= 0) return;
            TimelineLiteAssetTreeViewItem item = FindItem(selectedIds[0], rootItem) as TimelineLiteAssetTreeViewItem;
            if (item == null) return;
            TimelineLiteAsset data = item.UserData;
            if (data != null && UnityEditor.Timeline.TimelineEditor.timelineAsset != data)
            {
                AssetDatabase.OpenAsset(data);
                Window.Focus();
            }
        }
#endif

        public void Filtter(string _filtter, SearchMode _filtterMode)
        {
            filtter = _filtter;
            filtterMode = _filtterMode;
            Reload();
        }

        public void AddItem<T>(T item) where T : TimelineLiteAssetTreeViewItem
        {
            AddItem("", item);
        }

        public void AddItem<T>(string path, T item) where T : TimelineLiteAssetTreeViewItem
        {
            item.Path = path;
            item.displayName = item.UserData.name;
            items.Add(item);
        }

        public void Clear()
        {
            items.Clear();
        }
    }

    delegate bool FiltterMethod(TimelineLiteAsset _timelineLiteAsset);
}