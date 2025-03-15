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

using Atom.TimelineLite.Editors;
using Atom.UnityEditors;
using UnityEditor;
using UnityEngine;

namespace Atom.TimelineLite.Example.Editors
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(TweenPositionTLClipAsset))]
    public class TweenPositionTLClipAssetEditor : BaseEditor
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
                    if (TimelineLiteEditorWindow.Playable != null && GUILayout.Button("Set", GUILayout.Width(50)))
                        property.vector3Value = TimelineLiteEditorWindow.Playable.transform.position;
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