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
 *  Blog: https://www.crosshair.top/
 *
 */
#endregion
using UnityEditor;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector.Editor;
#endif

namespace CZToolKit.TimelineLite.Editors
{
    [CustomEditor(typeof(TLBasicTrackAsset), true)]
    public class TLBasicTrackAssetEditor : Editor
    {
#if ODIN_INSPECTOR
        PropertyTree propertyTree;
#endif
        protected virtual void OnEnable()
        {
#if ODIN_INSPECTOR
            propertyTree = PropertyTree.Create(target);
            propertyTree.DrawMonoScriptObjectField = true;
#endif
            if (target is ITLBasicTrackAssetEditorEnter enter)
                enter.Enter();
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            EditorGUI.EndDisabledGroup();
#if ODIN_INSPECTOR
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
            EditorGUI.BeginChangeCheck();
            SerializedProperty iterator = serializedObject.GetIterator();
            iterator.NextVisible(true);
            do
            {
                if (iterator.name == "m_Script") continue;
                EditorGUILayout.PropertyField(iterator);
            } while (iterator.NextVisible(false));

            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
#endif
        }

    }
}
