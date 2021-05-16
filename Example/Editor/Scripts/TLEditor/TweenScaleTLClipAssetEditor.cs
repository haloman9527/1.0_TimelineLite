using CZToolKit.Core.Editors;
using CZToolKit.TimelineLite.Editors;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CZToolKit.TimelineLite.Example.Editors
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(TweenScaleTLClipAsset))]
    public class TweenScaleTLClipAssetEditor : BasicEditor
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
            if (TimelineLiteEditorWindow.Instance != null &&
                TimelineLiteEditorWindow.Playable != null &&
                GUILayout.Button("Set", GUILayout.Width(50)))
                arg0.vector3Value = TimelineLiteEditorWindow.Playable.transform.localScale;
            GUILayout.EndHorizontal();
        }
    }
}
