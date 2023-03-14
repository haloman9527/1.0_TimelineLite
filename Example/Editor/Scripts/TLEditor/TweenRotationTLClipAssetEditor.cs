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
using CZToolKit.Common.IMGUI;
using CZToolKit.TimelineLite.Editors;
using UnityEditor;
using UnityEngine;

namespace CZToolKit.TimelineLite.Example.Editors
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(TweenRotationTLClipAsset))]
    public class TweenRotationTLClipAssetEditor : BasicEditor
    {
        protected override void RegisterDrawers()
        {
            base.RegisterDrawers();
            RegisterDrawer("from", Draw);
            RegisterDrawer("to", Draw);
        }

        private void Draw(SerializedProperty arg0)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(arg0);
            if (TimelineLiteEditorWindow.Playable != null &&
                GUILayout.Button("Set", GUILayout.Width(50)))
                arg0.vector3Value = TimelineLiteEditorWindow.Playable.transform.rotation.eulerAngles;
            GUILayout.EndHorizontal();
        }
    }
}
