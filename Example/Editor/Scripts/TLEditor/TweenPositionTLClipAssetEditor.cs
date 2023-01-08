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
using CZToolKit.Common.Editors;
using CZToolKit.TimelineLite.Editors;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CZToolKit.TimelineLite.Example.Editors
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(TweenPositionTLClipAsset))]
    public class TweenPositionTLClipAssetEditor : BasicEditor
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
                arg0.vector3Value = TimelineLiteEditorWindow.Playable.transform.position;
            GUILayout.EndHorizontal();
        }
    }
}
