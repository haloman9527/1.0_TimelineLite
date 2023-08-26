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
using CZToolKitEditor;

namespace CZToolKit.TimelineLite.Editors
{
    [CustomEditor(typeof(PlayableDirectorLite))]
    public class PlayableDirectorLiteEditor : BaseEditor
    {
        PlayableDirectorLite playableDirectorLite;

        bool HasTarget { get { return playableDirectorLite != null; } }
        bool HasTimeline { get { return HasTarget && playableDirectorLite.Timeline != null; } } 
        
        void OnEnable()
        {
            playableDirectorLite = target as PlayableDirectorLite;
            EditorApplication.update += CustomRepaint;
        }

        protected override void OnPropertyGUI(SerializedProperty property)
        {
            switch (property.propertyPath)
            {
                case "speed":
                {
                    playableDirectorLite.Speed = EditorGUILayout.FloatField(new GUIContent("Speed"), playableDirectorLite.Speed);
                    break;
                }
                default:
                {
                    base.OnPropertyGUI(property);
                    break;
                }
            }
        }

        void CustomRepaint()
        {

        }

        private void OnDisable()
        {
            EditorApplication.update -= CustomRepaint;
        }
    }
}
