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
using UnityEditor;
using UnityEditor.Timeline;
using System;
using System.Collections.Generic;
using System.Linq;
using Atom.UnityEditors;

namespace Atom.TimelineLite.Editors
{
    public enum SearchMode { StartsWith, Contains, EndsWith }

    [Serializable]
    public partial class TimelineLiteEditorWindow : BaseEditorWindow
    {
        #region 常量
        public const float ToolbarHeight = 50;
        #endregion

        #region Static
        [MenuItem("Tools/Moyo/TimelineLite/Timeline Projects")]
        public static void Open()
        {
            if (Instance == null)
                GetWindow<TimelineLiteEditorWindow>();
            else
                Instance.Focus();
        }

        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
        private static void RenderSelectionGizmo(PlayableDirectorLite _playable, GizmoType _gizmoType)
        {
            if (_playable == playable && Instance != null)
                Instance.OnDrawGizmos();
        }

        static TimelineLiteEditorWindow instance;
        static PlayableDirectorLite playable;

        public static TimelineLiteEditorWindow Instance
        {
            get { return instance; }
            protected set { instance = value; }
        }
        public static PlayableDirectorLite Playable
        {
            get { return playable; }
            protected set { playable = value; }
        }
        #endregion

        [SerializeField] int playableInstanceID;
        int selectedTabIndex = 0;
        string[] toolbarLabels;
        protected Dictionary<string, Action> toolbarTabs = new Dictionary<string, Action>();

        protected Action initializeToolbarTab;
        protected Action onProjectChange;

        #region Unity
        protected virtual void OnEnable()
        {
            instance = this;
            titleContent = new GUIContent("TimelineLites");

            EditorApplication.playModeStateChanged += OnPlayModeChanged;

#if UNITY_2019_1_OR_NEWER
            SceneView.duringSceneGui += OnSceneGUI;
#else
            SceneView.onSceneGUIDelegate += OnSceneGUI;
#endif

            Playable = (EditorUtility.InstanceIDToObject(playableInstanceID) as GameObject)?.GetComponent<PlayableDirectorLite>();
            Initialize();
        }

        protected virtual void Initialize()
        {
            toolbarTabs["Assets List"] = AssetListGUI;
            toolbarTabs["Toolbox"] = ToolboxGUI;
            toolbarTabs["Inspector"] = InspectorGUI;
            toolbarLabels = toolbarTabs.Keys.ToArray();

            initializeToolbarTab += InitializeAssetList;
            initializeToolbarTab += InitializeToolbox;
            initializeToolbarTab += InitializeInspector;
            initializeToolbarTab.Invoke();
        }

        void OnPlayModeChanged(PlayModeStateChange obj)
        {
            switch (obj)
            {
                case PlayModeStateChange.EnteredEditMode:
                    PlayableDirectorLite tempPlayable = (EditorUtility.InstanceIDToObject(playableInstanceID) as GameObject)?.GetComponent<PlayableDirectorLite>();
                    if (tempPlayable != null)
                        playableInstanceID = tempPlayable.gameObject.GetInstanceID();
                    Playable = tempPlayable;
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    Playable = (EditorUtility.InstanceIDToObject(playableInstanceID) as GameObject)?.GetComponent<PlayableDirectorLite>();
                    break;
                default:
                    break;
            }
        }

        protected virtual void OnProjectChange() { onProjectChange?.Invoke(); }

        protected virtual void OnGUI()
        {
            PlayableDirectorLite tempPlayable = EditorGUILayout.ObjectField(Playable, typeof(PlayableDirectorLite), true) as PlayableDirectorLite;
            if (tempPlayable != null)
                playableInstanceID = tempPlayable.gameObject.GetInstanceID();
            Playable = tempPlayable;

            selectedTabIndex = GUILayout.Toolbar(selectedTabIndex, toolbarLabels, GUILayout.Height(30));
            toolbarTabs[toolbarLabels[selectedTabIndex]]();
            Repaint();
        }

        protected virtual void OnDestroy()
        {
#if UNITY_2019_1_OR_NEWER
            SceneView.duringSceneGui -= OnSceneGUI;
#else
            SceneView.onSceneGUIDelegate -= OnSceneGUI;
#endif
        }
        #endregion

        protected virtual void OnDrawGizmos()
        {
#if UNITY_2018_1_OR_NEWER
            TimelineLiteAsset inspectedAsset = TimelineEditor.inspectedAsset as TimelineLiteAsset;
#else
            TimelineLiteAsset inspectedAsset = UnityEditor.Timeline.TimelineEditor.timelineAsset as TimelineLiteAsset;
#endif
            if (inspectedAsset == null) return;

            // 始终调用
            foreach (var trackAsset in inspectedAsset.GetOutputTracks())
            {
                TLBasicTrackAsset basicTrackAsset = trackAsset as TLBasicTrackAsset;
                if (basicTrackAsset == null) continue;
                foreach (var timelineClip in trackAsset.GetClips())
                {
                    IDrawGizmos asset = timelineClip.asset as IDrawGizmos;
                    if (asset != null)
                        asset.DrawGizmos_Lite(Playable, timelineClip, IndicatorFrame);
                }
            }

            foreach (var timelineClip in TimelineEditor.selectedClips)
            {
                IDrawGizmos asset = timelineClip.asset as IDrawGizmos;
                if (asset != null)
                    asset.DrawGizmosSelected_Lite(Playable, timelineClip, IndicatorFrame);
            }
        }

        protected virtual void OnSceneGUI(SceneView _sceneView)
        {
            if (Playable == null) return;

#if UNITY_2018_1_OR_NEWER
            TimelineLiteAsset inspectedAsset = TimelineEditor.inspectedAsset as TimelineLiteAsset;
#else
            TimelineLiteAsset inspectedAsset = UnityEditor.Timeline.TimelineEditor.timelineAsset as TimelineLiteAsset;
#endif
            if (inspectedAsset == null) return;

            // 始终调用
            foreach (var trackAsset in inspectedAsset.GetOutputTracks())
            {
                TLBasicTrackAsset basicTrackAsset = trackAsset as TLBasicTrackAsset;
                if (basicTrackAsset == null) continue;
                foreach (var timelineClip in basicTrackAsset.GetClips())
                {
                    ISceneGUI asset = timelineClip.asset as ISceneGUI;
                    if (asset != null)
                        asset.SceneGUI(Playable, timelineClip, indicatorFrame);
                }
            }

#if UNITY_2019_1_OR_NEWER
            // 只有选中后调用
            foreach (var timelineClip in TimelineEditor.selectedClips)
            {
                ISceneGUI asset = timelineClip.asset as ISceneGUI;
                if (asset != null)
                    asset.SceneGUISelected(Playable, timelineClip, indicatorFrame);
            }
#endif
        }
    }
}
