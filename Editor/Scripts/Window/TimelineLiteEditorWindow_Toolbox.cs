using CZToolKit.Core;
using CZToolKit.Core.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

namespace CZToolKit.TimelineLite.Editors
{
    partial class TimelineLiteEditorWindow
    {
        GUIContent createTrackIcon;
        GUIContent csIcon;
        GUIContent pingIcon;
        GUIContent selectIcon;

        TreeViewState toolboxTreeViewState;
        ToolboxTreeView toolBoxTreeView;
        GenericMenu trackTypeMenu;

        int indicatorFrame = 0;
        protected Action<int> onIndicatorChanged;
        public int IndicatorFrame
        {
            set
            {
                if (indicatorFrame != value)
                {
                    indicatorFrame = value;
                    if (onIndicatorChanged != null)
                        onIndicatorChanged(indicatorFrame);
                }
            }
            get { return indicatorFrame; }
        }

        private void InitializeToolbox()
        {
            onIndicatorChanged += (i) => { SceneView.RepaintAll(); };

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
            foreach (Type trackType in TypeCache.GetTypesDerivedFrom(TimelineLiteTrackAssetType))
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
                    IndicatorFrame = TimelineLiteUtility.ConvertToFrameIndex(EditorGUIExtension.ProgressBar(rect, currentFrame, 0, frameCount, currentFrame + "/" + frameCount, true));
                    EditorGUI.EndDisabledGroup();
                }
                else
                {
                    // 播放状态下可用来调整动画进度
                    EditorGUI.BeginDisabledGroup(!hasPlayable || playable.PlayStatus == PlayStatus.Stopped);
                    int frameCount = hasTimeline ? Playable.Timeline.FrameCount : 0;
                    int currentFrame = hasTimeline ? Playable.CurrentFrame : 0;
                    IndicatorFrame = TimelineLiteUtility.ConvertToFrameIndex(EditorGUIExtension.ProgressBar(rect, currentFrame, 0, frameCount, currentFrame + "/" + frameCount, true));
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
    }
}
