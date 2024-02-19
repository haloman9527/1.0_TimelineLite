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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace CZToolKit.TimelineLite.Editors
{
    public class ActionCreator : EditorWindow
    {
        const string ClipAssetTAPath = "TLClipAsset.cs";
        const string TrackAssetTAPath = "TLTrackAsset.cs";
        const string ActionTAPath = "TLAction.cs";
        const string ActionDataTAPath = "TLActionData.cs";

        static ActionCreator instance;


        [MenuItem("Assets/Create/CZToolKit/TimelineLite/Create TLAction Classes")]
        public static void Open()
        {
            instance = GetWindow<ActionCreator>(true, "Class Name");
            instance.position = new Rect(Screen.width / 2, Screen.width / 2, instance.minSize.x, instance.minSize.y);
        }

        string menuItem;
        string className;
        Color color;

        private void OnEnable()
        {
            color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
            minSize = new Vector2(300, 180);
            maxSize = new Vector2(300, 180);
        }

        private void OnGUI()
        {
            GUILayout.Label("TrackAssetColor");
            color = EditorGUILayout.ColorField(color);
            GUILayout.Label("MenuItem");
            menuItem = GUILayout.TextField(menuItem);
            GUILayout.Label("ClassName");
            className = GUILayout.TextField(className);

            GUILayout.Space(20);
            if (GUILayout.Button("Create",GUILayout.Height(40)))
            {
                //获取当前路径
                string path = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);

                TextAsset clipAssetTA = Resources.Load<TextAsset>(ClipAssetTAPath);
                TextAsset trackAssetTA = Resources.Load<TextAsset>(TrackAssetTAPath);
                TextAsset actionTA = Resources.Load<TextAsset>(ActionTAPath);
                TextAsset actionDataTA = Resources.Load<TextAsset>(ActionDataTAPath);


                // ClipAsset
                string clipAssetClassPath = path + "/" + className + ClipAssetTAPath;
                if (!File.Exists(path))
                {
                    string code = clipAssetTA.text;
                    code = code.Replace("#ClassName#", className);
                    using (FileStream fs = new FileStream(clipAssetClassPath, FileMode.OpenOrCreate))
                    {
                        using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                        {
                            sw.Write(code);
                        }
                    }
                }

                // TrackAsset
                string trackAssetClassPath = path + "/" + className + TrackAssetTAPath;
                if (!File.Exists(path))
                {
                    string code = trackAssetTA.text;
                    code = code.Replace("#ClassName#", className).Replace("#MenuItem#", menuItem);
                    code = code.Replace("#r#", color.r.ToString("0.00") + "f");
                    code = code.Replace("#g#", color.g.ToString("0.00") + "f");
                    code = code.Replace("#b#", color.b.ToString("0.00") + "f");
                    using (FileStream fs = new FileStream(trackAssetClassPath, FileMode.OpenOrCreate))
                    {
                        using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                        {
                            sw.Write(code);
                        }
                    }
                }

                // Action
                string actionClassPath = path + "/" + className + ActionTAPath;
                if (!File.Exists(path))
                {
                    string code = actionTA.text;
                    code = code.Replace("#ClassName#", className);
                    using (FileStream fs = new FileStream(actionClassPath, FileMode.OpenOrCreate))
                    {
                        using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                        {
                            sw.Write(code);
                        }
                    }
                }

                // ActionData
                string actionDataClassPath = path + "/" + className + ActionDataTAPath;
                if (!File.Exists(path))
                {
                    string code = actionDataTA.text;
                    code = code.Replace("#ClassName#", className);
                    using (FileStream fs = new FileStream(actionDataClassPath, FileMode.OpenOrCreate))
                    {
                        using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                        {
                            sw.Write(code);
                        }
                    }
                }

                AssetDatabase.Refresh();
                Close();
            }
        }
    }
}