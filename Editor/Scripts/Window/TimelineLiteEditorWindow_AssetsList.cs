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
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Timeline;

namespace CZToolKit.TimelineLite.Editors
{
    partial class TimelineLiteEditorWindow
    {
        string searchText = "";
        SearchField searchField;
        GUIContent createAssetIcon;
        public SearchMode searchMode = SearchMode.Contains;
        TreeViewState treeViewState;
        TimelineLitesTreeView projecListTreeView;

        private TreeViewState TreeViewState { get { return treeViewState == null ? treeViewState = new TreeViewState() : treeViewState; } }
        public virtual Type TimelineLiteAssetType { get { return typeof(TimelineLiteAsset); } }
        public virtual Type TimelineLiteTrackAssetType { get { return typeof(TrackAsset); } }

        void InitializeAssetList()
        {
            onProjectChange += RefreshList;

            createAssetIcon = new GUIContent(EditorGUIUtility.FindTexture("CreateAddNew"), "创建Asset");
            searchField = new SearchField();
            searchField.autoSetFocusOnFindCommand = true;
            projecListTreeView = BuildAssetsListTreeView();
            projecListTreeView.Reload();
        }

        protected virtual TimelineLitesTreeView BuildAssetsListTreeView()
        {
            TimelineLitesTreeView projecListTreeView = new TimelineLitesTreeView(TreeViewState, searchText, this);

            // 查找所有使用此类的ScriptableObjects
            string[] guids = AssetDatabase.FindAssets("t:" + TimelineLiteAssetType);

            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                TimelineLiteAsset asset = AssetDatabase.LoadAssetAtPath<TimelineLiteAsset>(assetPath);
                TimelineLiteAssetTreeViewItem assetTreeViewItem = new TimelineLiteAssetTreeViewItem(asset);
                projecListTreeView.AddItem(asset.GetType().Name, assetTreeViewItem);
            }
            return projecListTreeView;
        }

        private void AssetListGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            searchMode = (SearchMode)EditorGUILayout.EnumPopup(searchMode, GUILayout.Width(80));
            Rect rect = GUILayoutUtility.GetRect(position.width - 130, 25);
            rect.height = 20;
            rect.y += 3;

            searchText = searchField.OnGUI(rect, searchText);
            if (searchField.HasFocus() && Event.current.rawType == EventType.KeyUp && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter))
            {
                projecListTreeView.Filtter(searchText, searchMode);
                searchField.SetFocus();
            }

            if (GUILayout.Button(createAssetIcon, GUILayout.Width(20), GUILayout.Height(17)))
            {
                string path = EditorUtility.SaveFilePanelInProject("创建", "New TimelineLite", "asset", "");
                if (!string.IsNullOrEmpty(path))
                {
                    AssetDatabase.CreateAsset(ScriptableObject.CreateInstance(TimelineLiteAssetType), path);
                    AssetDatabase.Refresh();
                }
            }
            GUILayout.Space(10);
            GUILayout.EndHorizontal();

            rect.x = 10;
            rect.y += 20;
            rect.width = position.width - 20;
            rect.height = position.height - rect.y - 5;
            projecListTreeView.OnGUI(rect);
        }

        public virtual void BuildAssetContextMenu(IList<int> selection, GenericMenu genericMenu)
        {
            if (selection.Count > 0)
            {
                if (selection.Count == 1)
                {
                    TimelineLiteAssetTreeViewItem item = projecListTreeView.FindItem(selection[0]) as TimelineLiteAssetTreeViewItem;
                    if (item != null)
                    {
                        TimelineLiteAsset data = item.UserData;
                        BuildSingleAssetItemMenu(data, genericMenu);
                    }
                }
                else
                {
                    genericMenu.AddItem(new GUIContent("批量另存为SO"), false, () => SaveSOSelectionAs(selection));
                }

                genericMenu.AddItem(new GUIContent("删除所选"), false, () => DeleteSelection(selection));
            }
        }

        public virtual void BuildSingleAssetItemMenu(TimelineLiteAsset data, GenericMenu genericMenu)
        {
            genericMenu.AddItem(new GUIContent("保存SO"), false, () => SaveSO(data));
            genericMenu.AddItem(new GUIContent("另存为SO"), false, () => SaveSOAs(data));
        }

        private void DeleteSelection(IList<int> _selection)
        {
            if (EditorUtility.DisplayDialog("警告", "你确定要删除所选文件吗？", "删除", "取消"))
            {
                List<TimelineLiteAsset> datas = new List<TimelineLiteAsset>();
                foreach (var id in _selection)
                {
                    TimelineLiteAssetTreeViewItem item = projecListTreeView.FindItem(id) as TimelineLiteAssetTreeViewItem;
                    if (item == null) continue;

                    datas.Add(item.UserData);
                }

                foreach (var data in datas)
                {
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(data));
                }
                projecListTreeView.SetSelection(new List<int>());
            }
        }

        #region Save
        #region SO

        private void SaveSO(TimelineLiteAsset data)
        {
            TimelineLiteSO so = AssetDatabase.LoadAssetAtPath<TimelineLiteSO>(data.previousPath + ".asset");
            if (so == null)
                SaveSOAs(data);
            else
            {
                so.TimelineLiteObjectData = data.Extract();
                EditorUtility.SetDirty(so);
                AssetDatabase.SaveAssets();
            }
        }

        private void SaveSOAs(TimelineLiteAsset data)
        {
            string path = EditorUtility.SaveFilePanelInProject("Save", data.name + "_SO", "asset", "Null");
            if (!string.IsNullOrEmpty(path))
            {
                data.previousPath = path.Substring(0, path.LastIndexOf("."));
                EditorUtility.SetDirty(data);

                TimelineLiteSO so = AssetDatabase.LoadAssetAtPath<TimelineLiteSO>(data.previousPath + ".asset");
                if (so != null)
                {
                    so.TimelineLiteObjectData = data.Extract();
                    EditorUtility.SetDirty(so);
                }
                else
                {
                    so = ScriptableObject.CreateInstance(data.TargetSOType) as TimelineLiteSO;
                    so.TimelineLiteObjectData = data.Extract();
                    AssetDatabase.CreateAsset(so, data.previousPath + ".asset");
                }

                AssetDatabase.SaveAssets();
            }
        }

        /// <summary> SO </summary>
        private void SaveSOSelectionAs(IList<int> _selection)
        {
            string path = EditorUtility.OpenFolderPanel("批量另存为", Application.dataPath, "");

            if (!string.IsNullOrEmpty(path))
            {
                path = path.Replace(Application.dataPath, "Assets");
                foreach (var id in _selection)
                {
                    TimelineLiteAssetTreeViewItem item = projecListTreeView.FindItem(id) as TimelineLiteAssetTreeViewItem;
                    if (item == null) continue;
                    TimelineLiteAsset data = item.UserData;

                    data.previousPath = path + "/" + data.name + "_SO";
                    EditorUtility.SetDirty(data);

                    TimelineLiteSO so = AssetDatabase.LoadAssetAtPath<TimelineLiteSO>(data.previousPath + ".asset");
                    if (so != null)
                    {
                        so.TimelineLiteObjectData = data.Extract();
                        EditorUtility.SetDirty(so);
                    }
                    else
                    {
                        so = ScriptableObject.CreateInstance(data.TargetSOType) as TimelineLiteSO;
                        so.TimelineLiteObjectData = data.Extract();
                        AssetDatabase.CreateAsset(so, data.previousPath + ".asset");
                    }
                }

                AssetDatabase.SaveAssets();
            }
        }

        #endregion
        #endregion

        public void RefreshList()
        {
            projecListTreeView.Clear();
            projecListTreeView = BuildAssetsListTreeView();
            projecListTreeView.Reload();
        }

    }
}
