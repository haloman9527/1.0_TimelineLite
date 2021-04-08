using UnityEngine;
using UnityEngine.Timeline;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEditor.IMGUI.Controls;
using System;
using System.Collections.Generic;
using CZToolKit.Core;
using CZToolKit.Core.Editors;
using System.Reflection;
using System.Linq;
using UnityEngine.UIElements;

namespace CZToolKit.TimelineLite.Editors
{
    public enum SearchMode { StartsWith, Contains, EndsWith }

    [Serializable]
    public class TimelineLiteEditorWindow : BasicEditorWindow<TimelineLiteEditorWindow>
    {
        #region 常量
        public const float ToolbarHeight = 50;
        #endregion

        #region Static
        [MenuItem("Tools/CZToolKit/TimelineLite/Timeline Projects")]
        public static void Open()
        {
            if (Instance == null)
                GetWindow<TimelineLiteEditorWindow>();
            else
                Instance.Focus();
        }
        static TimelineLiteEditorWindow instance;

        public static TimelineLiteEditorWindow Instance
        {
            get { return instance; }
            protected set { instance = value; }
        }
        #endregion

        int playableInstanceID;
        PlayableDirectorLite playable;
        int selectedTabIndex = 0;
        string[] toolbarLabels;
        protected Dictionary<string, Action> toolbarTabs = new Dictionary<string, Action>();

        protected Action initializeToolbarTab;
        protected Action onProjectChange;

        public PlayableDirectorLite Playable { get { return playable; } protected set { playable = value; } }

        #region Unity

        protected virtual void OnEnable()
        {
            instance = this;
            EditorApplication.playModeStateChanged += OnPlayModeChanged;

            titleContent = new GUIContent("TimelineLites");
#if UNITY_2019_1_OR_NEWER
            SceneView.duringSceneGui += OnSceneGUI;
#else
            SceneView.onSceneGUIDelegate += OnSceneGUI;
#endif
            if (Playable != null)
                Playable.onDrawGizmos += OnDrawGizmos;

            Initialize();
            initializeToolbarTab.Invoke();
            BuildToolbar();
        }

        private void OnPlayModeChanged(PlayModeStateChange obj)
        {
            switch (obj)
            {
                case PlayModeStateChange.EnteredEditMode:
                    PlayableDirectorLite tempPlayable = (EditorUtility.InstanceIDToObject(playableInstanceID) as GameObject)?.GetComponent<PlayableDirectorLite>();
                    if (Playable != null)
                        Playable.onDrawGizmos -= OnDrawGizmos;
                    if (tempPlayable != null)
                    {
                        playableInstanceID = tempPlayable.gameObject.GetInstanceID();
                        tempPlayable.onDrawGizmos += OnDrawGizmos;
                    }
                    Playable = tempPlayable;
                    Repaint();
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    Playable = (EditorUtility.InstanceIDToObject(playableInstanceID) as GameObject)?.GetComponent<PlayableDirectorLite>();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    break;
                default:
                    break;
            }
        }

        protected virtual void OnDestroy()
        {
#if UNITY_2019_1_OR_NEWER
            SceneView.duringSceneGui -= OnSceneGUI;
#else
            SceneView.onSceneGUIDelegate -= OnSceneGUI;
#endif
            if (Playable != null)
                Playable.onDrawGizmos -= OnDrawGizmos;
        }

        protected virtual void OnProjectChange()
        {
            onProjectChange.Invoke();
        }

        protected virtual void OnGUI()
        {
            PlayableDirectorLite tempPlayable = EditorGUILayout.ObjectField(Playable, typeof(PlayableDirectorLite), true) as PlayableDirectorLite;
            if (Playable == null || Playable != tempPlayable)
            {
                if (Playable != null)
                    Playable.onDrawGizmos -= OnDrawGizmos;
                if (tempPlayable != null)
                {
                    playableInstanceID = tempPlayable.gameObject.GetInstanceID();
                    tempPlayable.onDrawGizmos += OnDrawGizmos;
                }
                Playable = tempPlayable;
            }

            selectedTabIndex = GUILayout.Toolbar(selectedTabIndex, toolbarLabels, GUILayout.Height(30));
            toolbarTabs[toolbarLabels[selectedTabIndex]]();
            Repaint();
        }

        #endregion

        protected virtual void Initialize()
        {
            initializeToolbarTab += InitializeAssetList;
            initializeToolbarTab += InitializeToolbox;
            initializeToolbarTab += InitializeInspector;
        }

        void BuildToolbar()
        {
            toolbarLabels = new string[toolbarTabs.Count];
            int i = 0;
            foreach (string key in toolbarTabs.Keys)
            {
                toolbarLabels[i++] = key;
            }
            selectedTabIndex = Mathf.Min(toolbarLabels.Length - 1, selectedTabIndex);
        }

        protected virtual void OnDrawGizmos()
        {
#if UNITY_2019_1_OR_NEWER
            UnityEngine.Timeline.TimelineAsset inspectedAsset = UnityEditor.Timeline.TimelineEditor.inspectedAsset;
            if (inspectedAsset == null) return;

            // 任何时间调用，建议关闭
            //foreach (var trackAsset in inspectedAsset.GetOutputTracks())
            //{
            //    TLBasicTrackAsset basicTrackAsset = trackAsset as TLBasicTrackAsset;
            //    if (basicTrackAsset == null) continue;
            //    foreach (var timelineClip in trackAsset.GetClips())
            //    {
            //        ITLBasicClipAsset asset = timelineClip.asset as ITLBasicClipAsset;
            //        if (asset != null)
            //        {
            //            int startFrame = PlayableDirectorLite.TakeFrame(timelineClip.start);
            //            int endFrame = PlayableDirectorLite.TakeFrame(timelineClip.end);
            //            int frameSince = Mathf.Clamp(indicator - startFrame, 0, endFrame - startFrame);
            //            asset.DrawGizmos_Lite(Playable, frameSince);
            //        }
            //    }
            //}

            foreach (var timelineClip in UnityEditor.Timeline.TimelineEditor.selectedClips)
            {
                TLBasicClipAsset asset = timelineClip.asset as TLBasicClipAsset;
                if (asset != null)
                    asset.DrawGizmosSelected_Lite(Playable, timelineClip);
            }
#endif
        }

        protected virtual void OnSceneGUI(SceneView _sceneView)
        {
            if (Playable == null) return;

#if UNITY_2018_1_OR_NEWER
            TimelineLiteAsset inspectedAsset = UnityEditor.Timeline.TimelineEditor.inspectedAsset as TimelineLiteAsset;
#else
            TimelineLiteAsset inspectedAsset = UnityEditor.Timeline.TimelineEditor.timelineAsset as TimelineLiteAsset;
#endif
            if (inspectedAsset == null) return;

            // 任何时间调用，建议关闭
            //foreach (var trackAsset in inspectedAsset.GetOutputTracks())
            //{
            //    TLBasicTrackAsset basicTrackAsset = trackAsset as TLBasicTrackAsset;
            //    if (basicTrackAsset == null) continue;
            //    foreach (var timelineClip in basicTrackAsset.GetClips())
            //    {
            //        ITLBasicClipAsset asset = timelineClip.asset as ITLBasicClipAsset;
            //        if (asset != null)
            //        {
            //            int startFrame = PlayableDirectorLite.TakeFrame(timelineClip.start);
            //            int endFrame = PlayableDirectorLite.TakeFrame(timelineClip.end);
            //            int frameSince = Mathf.Clamp(indicator - startFrame, 0, endFrame - startFrame);
            //            asset.SceneGUI(Playable, frameSince);
            //        }
            //    }
            //}

#if UNITY_2019_1_OR_NEWER
            // 只有选中后调用
            foreach (var timelineClip in TimelineEditor.selectedClips)
            {
                TLBasicClipAsset asset = timelineClip.asset as TLBasicClipAsset;
                if (asset != null)
                    asset.SceneGUISelected(Playable, timelineClip);
            }
            SceneView.RepaintAll();
#endif
        }

        #region Draw Asset List

        string searchText = "";
        SearchField searchField;
        GUIContent createAssetIcon;
        public SearchMode searchMode = SearchMode.Contains;
        TreeViewState treeViewState;
        TimelineLitesTreeView projecListTreeView;

        private TreeViewState TreeViewState { get { return treeViewState == null ? treeViewState = new TreeViewState() : treeViewState; } }
        public virtual Type TimelineLiteAssetType { get { return typeof(TimelineLiteAsset); } }
        public virtual Type TimelineLiteTrackAssetType { get { return typeof(TrackAsset); } }

        private void InitializeAssetList()
        {
            toolbarTabs["Asset List"] = AssetListGUI;
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
        #endregion

        #region Draw Toolbox

        GUIContent createTrackIcon;
        GUIContent csIcon;
        GUIContent pingIcon;
        GUIContent selectIcon;

        TreeViewState toolboxTreeViewState;
        ToolboxTreeView toolBoxTreeView;
        GenericMenu trackTypeMenu;

        int indicatorFrame = 0;
        public int IndicatorFrame { get { return indicatorFrame; } }

        private void InitializeToolbox()
        {
            toolbarTabs["Toolbox"] = ToolboxGUI;
            createTrackIcon = new GUIContent(EditorGUIUtility.FindTexture("CreateAddNew"), "创建轨道");
            pingIcon = EditorGUIUtility.IconContent("d_SelectionListItem Icon");
            pingIcon.tooltip = "PingAsset";
            csIcon = new GUIContent(EditorGUIUtility.FindTexture("cs Script Icon"), "编辑Asset脚本");
            selectIcon = EditorGUIUtility.IconContent("Selectable Icon");
            selectIcon.tooltip = "选中Asset文件";

            if (toolboxTreeViewState == null)
                toolboxTreeViewState = new TreeViewState();
            toolBoxTreeView = new ToolboxTreeView(toolboxTreeViewState);
            BuildToolboxTreeView(toolBoxTreeView);
            toolBoxTreeView.Reload();

            List<KeyValuePair<string, Type>> menu = new List<KeyValuePair<string, Type>>();
            foreach (Type trackType in ChildrenTypeCache.GetChildrenTypes(TimelineLiteTrackAssetType))
            {
                TLTrackAssetMenuItemAttribute attrib;
                if (AttributeCache.TryGetTypeAttribute(trackType, out attrib))
                {
                    if (!string.IsNullOrEmpty(attrib.defaultTrackName))
                        menu.Add(new KeyValuePair<string, Type>(attrib.defaultTrackName, trackType));
                    else
                        menu.Add(new KeyValuePair<string, Type>(trackType.Name, trackType));
                }
            }
            menu = menu.OrderBy(a => a.Key).ToList();

            trackTypeMenu = new GenericMenu();
            foreach (var kv in menu)
            {
                trackTypeMenu.AddItem(new GUIContent(kv.Key), false, () =>
                {
                    CreateTrack(kv.Value, kv.Key);
                });
            }
        }

        protected virtual void BuildToolboxTreeView(ToolboxTreeView _treeView)
        {

            _treeView.AddItem(ToolboxTreeViewItem.GetSeparator(2));

            // 打开脚本，ping和选中保存按钮
            ToolboxTreeViewItem helpControl = ToolboxTreeViewItem.GetItem((rect, item) =>
            {
#if UNITY_2018_1_OR_NEWER
                TimelineLiteAsset timelineLiteAsset = TimelineEditor.inspectedAsset as TimelineLiteAsset;
#else
                TimelineLiteAsset timelineLiteAsset = TimelineEditor.timelineAsset as TimelineLiteAsset;
#endif
                EditorGUI.BeginDisabledGroup(timelineLiteAsset == null);
                rect.width = 30;
                if (GUI.Button(rect, csIcon))
                    AssetDatabase.OpenAsset(MonoScript.FromScriptableObject(timelineLiteAsset));
                rect.x += rect.width;

                if (GUI.Button(rect, pingIcon))
                {
                    Assembly unityEditorAssembly = Assembly.Load("UnityEditor");
                    Type type = new List<Type>(unityEditorAssembly.GetTypes()).Find(_type => _type.Name == "ProjectBrowser");
                    EditorWindow window = GetWindow(type);
                    EditorGUIUtility.PingObject(timelineLiteAsset);
                }
                rect.x += rect.width;
                if (GUI.Button(rect, selectIcon))
                    Selection.activeObject = timelineLiteAsset;
                rect.x += rect.width;
                rect.width = 50;
                if (GUI.Button(rect, "Save"))
                {
                    GenericMenu genericMenu = new GenericMenu();
                    BuildSingleAssetItemMenu(timelineLiteAsset, genericMenu);
                    genericMenu.ShowAsContext();

                }
                EditorGUI.EndDisabledGroup();
            });
            helpControl.Height = 30;
            _treeView.AddItem(helpControl);

            _treeView.AddItem(ToolboxTreeViewItem.GetSeparator(2));

            // 创建轨道按钮
            ToolboxTreeViewItem editorControl = ToolboxTreeViewItem.GetItem((rect, item) =>
            {
#if UNITY_2018_1_OR_NEWER
                TimelineLiteAsset timelineLiteAsset = TimelineEditor.inspectedAsset as TimelineLiteAsset;
#else
                TimelineLiteAsset timelineLiteAsset = TimelineEditor.timelineAsset as TimelineLiteAsset;
#endif

                rect.width = 30;
                EditorGUI.BeginDisabledGroup(timelineLiteAsset == null);
                if (GUI.Button(rect, createTrackIcon))
                    trackTypeMenu.ShowAsContext();
                EditorGUI.EndDisabledGroup();
            });
            editorControl.Height = 30;
            _treeView.AddItem(editorControl);

            // 播放进度控制条和播放按钮
            ToolboxTreeViewItem progressControl = ToolboxTreeViewItem.GetItem((rect, item) =>
            {
#if UNITY_2018_1_OR_NEWER
                TimelineLiteAsset timelineLiteAsset = TimelineEditor.inspectedAsset as TimelineLiteAsset;
#else
                TimelineLiteAsset timelineLiteAsset = TimelineEditor.timelineAsset as TimelineLiteAsset;
#endif
                GUI.Box(rect, "");
                rect.height = 25;

                bool hasPlayable = Playable != null;
                bool hasTimeline = hasPlayable && Playable.Timeline != null;
                bool disable = !hasPlayable || !hasTimeline;
                bool inspectorTimelineLite = timelineLiteAsset != null;

                if (!EditorApplication.isPlaying)
                {
                    // 非播放状态下只用来调整指针
                    EditorGUI.BeginDisabledGroup(!inspectorTimelineLite);
                    int frameCount = inspectorTimelineLite ? (int)timelineLiteAsset.GetFrameCount() : 0;
                    int currentFrame = Mathf.Clamp(inspectorTimelineLite ? indicatorFrame : 0, 0, frameCount);
                    indicatorFrame = TimelineLiteUtility.ConvertToFrameIndex(EditorGUIExtension.ProgressBar(rect, currentFrame, 0, frameCount, currentFrame + "/" + frameCount, true));
                    EditorGUI.EndDisabledGroup();
                }
                else
                {
                    // 播放状态下可用来调整动画进度
                    EditorGUI.BeginDisabledGroup(!hasPlayable || playable.PlayStatus == PlayStatus.Stopped);
                    int frameCount = hasTimeline ? Playable.Timeline.FrameCount : 0;
                    int currentFrame = hasTimeline ? Playable.CurrentFrame : 0;
                    indicatorFrame = TimelineLiteUtility.ConvertToFrameIndex(EditorGUIExtension.ProgressBar(rect, currentFrame, 0, frameCount, currentFrame + "/" + frameCount, true));
                    if (indicatorFrame != currentFrame)
                    {
                        Playable.Pause();
                        Playable.SetFrame(indicatorFrame);
                    }
                    EditorGUI.EndDisabledGroup();
                }

                rect.y += 30;
                rect.height = 15;
                rect.x = 0;
                rect.width = rect.width / 4;
                if (EditorApplication.isPlaying)
                {
                    // 上一帧按钮
                    EditorGUI.BeginDisabledGroup(!hasPlayable || playable.PlayStatus != PlayStatus.Pausing);
                    if (GUI.Button(rect, EditorGUIUtility.IconContent("Animation.PrevKey"), (GUIStyle)"MiniToolbarButton"))
                        playable.SetFrame(playable.CurrentFrame - 1);
                    EditorGUI.EndDisabledGroup();
                }
                else
                {
                    // 非播放状态下
                    EditorGUI.BeginDisabledGroup(!inspectorTimelineLite);
                    if (GUI.Button(rect, EditorGUIUtility.IconContent("Animation.PrevKey"), (GUIStyle)"MiniToolbarButton"))
                    {
                        int frameCount = inspectorTimelineLite ? (int)timelineLiteAsset.GetFrameCount() : 0;
                        indicatorFrame = Mathf.Clamp(--indicatorFrame, 0, frameCount);
                    }
                    EditorGUI.EndDisabledGroup();
                }

                EditorGUI.BeginDisabledGroup(!EditorApplication.isPlaying);
                rect.x += rect.width;
                if (hasPlayable)
                {
                    switch (playable.PlayStatus)
                    {
                        case PlayStatus.Playing:
                            if (GUI.Button(rect, EditorGUIUtility.IconContent("PauseButton"), (GUIStyle)"MiniToolbarButton"))
                                playable.Pause();
                            break;
                        case PlayStatus.Pausing:
                            if (GUI.Button(rect, EditorGUIUtility.IconContent("PlayButton"), (GUIStyle)"MiniToolbarButton"))
                                playable.Resume();
                            break;
                        case PlayStatus.Stopped:
                            if (playable.Timeline != null)
                            {
                                if (GUI.Button(rect, EditorGUIUtility.IconContent("PlayButton"), (GUIStyle)"MiniToolbarButton"))
                                    playable.Play();
                            }
                            else
                            {
                                EditorGUI.BeginDisabledGroup(!inspectorTimelineLite);
                                if (GUI.Button(rect, EditorGUIUtility.IconContent("PlayButton"), (GUIStyle)"MiniToolbarButton"))
                                {
                                    TimelineLiteObjectData data = timelineLiteAsset.Extract();
                                    ITimelineLiteObject timelineLiteObject = Activator.CreateInstance(timelineLiteAsset.TargetObjectType, data) as ITimelineLiteObject;
                                    Playable.Play(timelineLiteObject);
                                }
                                EditorGUI.EndDisabledGroup();
                            }
                            break;
                    }
                }
                else
                {
                    GUI.Button(rect, EditorGUIUtility.IconContent("PlayButton"), (GUIStyle)"MiniToolbarButton");
                }
                EditorGUI.EndDisabledGroup();

                // 下一帧按钮
                rect.x += rect.width;

                if (EditorApplication.isPlaying)
                {
                    EditorGUI.BeginDisabledGroup(!hasPlayable || playable.PlayStatus != PlayStatus.Pausing);
                    if (GUI.Button(rect, EditorGUIUtility.IconContent("Animation.NextKey"), (GUIStyle)"MiniToolbarButton"))
                        playable.SetFrame(playable.CurrentFrame + 1);
                    EditorGUI.EndDisabledGroup();
                }
                else
                {
                    EditorGUI.BeginDisabledGroup(!inspectorTimelineLite);
                    if (GUI.Button(rect, EditorGUIUtility.IconContent("Animation.NextKey"), (GUIStyle)"MiniToolbarButton"))
                    {
                        int currentFrame = inspectorTimelineLite ? indicatorFrame : 0;
                        int frameCount = inspectorTimelineLite ? (int)timelineLiteAsset.GetFrameCount() : 0;
                        indicatorFrame = Mathf.Clamp(++indicatorFrame, currentFrame, frameCount);
                    }
                    EditorGUI.EndDisabledGroup();
                }

                rect.x += rect.width;
                EditorGUI.BeginDisabledGroup(!hasPlayable || (playable.PlayStatus == PlayStatus.Stopped && playable.Timeline == null));
                if (GUI.Button(rect, EditorGUIUtility.IconContent("PreMatQuad"), (GUIStyle)"MiniToolbarButton"))
                    playable.Play<ITimelineLiteObject>(null);
                EditorGUI.EndDisabledGroup();
            });

            progressControl.Height = 50;
            _treeView.AddItem(progressControl);

        }

        protected void ToolboxGUI()
        {
            Rect lastRect = GUILayoutUtility.GetLastRect();
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            Rect toolboxGUI = GUILayoutUtility.GetRect(position.width, position.height - lastRect.y - lastRect.height - ToolbarHeight - 10);
            toolboxGUI.width -= 20;
            toolBoxTreeView.OnGUI(toolboxGUI);

            GUILayout.EndHorizontal();
        }

        public T CreateTrack<T>(string _path) where T : TrackAsset
        {
            return CreateTrack(typeof(T), _path) as T;

        }

        public TrackAsset CreateTrack(Type _trackAssetType, string _path)
        {
#if UNITY_2018_1_OR_NEWER
            TimelineLiteAsset timelineLiteAsset = TimelineEditor.inspectedAsset as TimelineLiteAsset;
#else
            TimelineLiteAsset timelineLiteAsset = TimelineEditor.timelineAsset as TimelineLiteAsset;
#endif
            TrackAsset trackAsset = null;
            if (timelineLiteAsset != null)
            {
                string name = _path.Substring(_path.LastIndexOf("/") + 1);

                if (Selection.activeObject is UnityEngine.Timeline.GroupTrack)
                    trackAsset = timelineLiteAsset.CreateTrack(_trackAssetType, Selection.activeObject as UnityEngine.Timeline.GroupTrack, name);
                else
                    trackAsset = timelineLiteAsset.CreateTrack(_trackAssetType, null, name);
#if UNITY_2018_1_OR_NEWER
                TimelineEditor.Refresh(RefreshReason.ContentsAddedOrRemoved);
#endif
            }
            return trackAsset;
        }
        #endregion

        #region Draw Inspector
        TimelineLiteAsset timelineLiteAsset;

#if ODIN_INSPECTOR
        Sirenix.OdinInspector.Editor.PropertyTree propertyTree;
#else
        SerializedObject serializedObject;
#endif

        private void InitializeInspector()
        {
            toolbarTabs["Inspector"] = InspectorGUI;
        }

        private void InspectorGUI()
        {
#if UNITY_2018_1_OR_NEWER
            TimelineLiteAsset currentTimelineLiteAsset = TimelineEditor.inspectedAsset as TimelineLiteAsset;
#else
            TimelineLiteAsset currentTimelineLiteAsset = TimelineEditor.timelineAsset as TimelineLiteAsset;
#endif
#if ODIN_INSPECTOR

            if (timelineLiteAsset != currentTimelineLiteAsset)
            {
                timelineLiteAsset = currentTimelineLiteAsset;
                if (timelineLiteAsset == null)
                    propertyTree = null;
                else
                {
                    propertyTree = Sirenix.OdinInspector.Editor.PropertyTree.Create(timelineLiteAsset);
                    propertyTree.DrawMonoScriptObjectField = true;
                }
            }
            else
            {
                if (propertyTree == null && timelineLiteAsset != null)
                {
                    propertyTree = Sirenix.OdinInspector.Editor.PropertyTree.Create(timelineLiteAsset);
                    propertyTree.DrawMonoScriptObjectField = true;
                }
            }
            if (propertyTree != null)
            {
#if ODIN_INSPECTOR_3
                propertyTree.BeginDraw(true);
                propertyTree.Draw();
                propertyTree.EndDraw();
#else
                Sirenix.OdinInspector.Editor.InspectorUtilities.BeginDrawPropertyTree(propertyTree, true);
                propertyTree.Draw();
                Sirenix.OdinInspector.Editor.InspectorUtilities.EndDrawPropertyTree(propertyTree);
#endif
            }

#else
            if (timelineLiteAsset != currentTimelineLiteAsset)
            {
                timelineLiteAsset = currentTimelineLiteAsset;
                if (timelineLiteAsset == null)
                    serializedObject = null;
                else
                    serializedObject = new SerializedObject(timelineLiteAsset);
            }
            else
            {
                if (serializedObject == null && timelineLiteAsset != null)
                    serializedObject = new SerializedObject(timelineLiteAsset);
            }

            if (serializedObject != null)
            {
                EditorGUI.BeginChangeCheck();
                SerializedProperty iterator = serializedObject.GetIterator();
                iterator.NextVisible(true);
                do
                {
                    EditorGUI.BeginDisabledGroup(iterator.name == "m_Script");
                    EditorGUILayout.PropertyField(iterator);
                    EditorGUI.EndDisabledGroup();
                } while (iterator.NextVisible(false));

                if (EditorGUI.EndChangeCheck())
                    serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }
#endif
        }
        #endregion
    }

    public struct IndicatorClipInfo
    {
        public int startFrame;
        public int endFrame;
        public int indicator;
        public int indicatorFrameSinceStart;
    }
}
