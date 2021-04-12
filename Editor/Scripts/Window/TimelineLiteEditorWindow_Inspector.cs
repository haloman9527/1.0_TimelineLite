using UnityEditor.Timeline;

namespace CZToolKit.TimelineLite.Editors
{
    partial class TimelineLiteEditorWindow
    {
        TimelineLiteAsset timelineLiteAsset;

#if ODIN_INSPECTOR
        Sirenix.OdinInspector.Editor.PropertyTree propertyTree;
#else
        SerializedObject serializedObject;
#endif
        void InitializeInspector()
        {
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
    }
}
