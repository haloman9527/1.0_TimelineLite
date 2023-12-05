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
using CZToolKitEditor;
using CZToolKit.TimelineLite.Editors;
using UnityEditor;
using UnityEngine;

namespace CZToolKit.TimelineLite.Example.Editors
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(TweenScaleTLClipAsset))]
    public class TweenScaleTLClipAssetEditor : BaseEditor
    {
        protected override void OnPropertyGUI(SerializedProperty property)
        {
            switch (property.propertyPath)
            {
                case "from":
                case "to":
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(property);
                    if (TimelineLiteEditorWindow.Instance != null &&
                        TimelineLiteEditorWindow.Playable != null &&
                        GUILayout.Button("Set", GUILayout.Width(50)))
                        property.vector3Value = TimelineLiteEditorWindow.Playable.transform.localScale;
                    GUILayout.EndHorizontal();
                    break;
                }
                default:
                {
                    base.OnPropertyGUI(property);
                    break;
                }
            }
        }
    }
}
