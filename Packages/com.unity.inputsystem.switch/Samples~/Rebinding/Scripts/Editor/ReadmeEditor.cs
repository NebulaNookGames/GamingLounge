﻿#if UNITY_EDITOR && !EDIT_README
using System;
using UnityEditor;
using UnityEngine;

namespace UnityEngine.InputSystem.Switch.Samples.Rebinding.Editor
{
    [CustomEditor(typeof(Readme))]
    public class ReadmeEditor : UnityEditor.Editor
    {
        const float k_Space = 16f;

        protected override void OnHeaderGUI()
        {
            var readme = (Readme)target;
            Init();

            var iconWidth = Mathf.Min(EditorGUIUtility.currentViewWidth / 3f - 20f, 128f);

            GUILayout.BeginHorizontal("In BigTitle");
            {
                if (readme.icon != null)
                {
                    GUILayout.Space(k_Space);
                    GUILayout.Label(readme.icon, GUILayout.Width(iconWidth), GUILayout.Height(iconWidth));
                }

                GUILayout.Space(k_Space);
                GUILayout.BeginVertical();
                {

                    GUILayout.FlexibleSpace();
                    GUILayout.Label(readme.title, TitleStyle);
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }

        public override void OnInspectorGUI()
        {
            var readme = (Readme)target;
            Init();

            foreach (var section in readme.sections)
            {
                if (!string.IsNullOrEmpty(section.heading))
                {
                    GUILayout.Label(section.heading, HeadingStyle);
                }

                if (!string.IsNullOrEmpty(section.text))
                {
                    GUILayout.Label(section.text, BodyStyle);
                }

                if (!string.IsNullOrEmpty(section.linkText))
                {
                    if (LinkLabel(new GUIContent(section.linkText)))
                    {
                        Application.OpenURL(section.url);
                    }
                }

                GUILayout.Space(k_Space);
            }
        }

        bool m_Initialized;

        GUIStyle LinkStyle
        {
            get { return m_LinkStyle; }
        }

        [SerializeField]
        GUIStyle m_LinkStyle;

        GUIStyle TitleStyle
        {
            get { return m_TitleStyle; }
        }

        [SerializeField]
        GUIStyle m_TitleStyle;

        GUIStyle HeadingStyle
        {
            get { return m_HeadingStyle; }
        }

        [SerializeField]
        GUIStyle m_HeadingStyle;

        GUIStyle BodyStyle
        {
            get { return m_BodyStyle; }
        }

        [SerializeField]
        GUIStyle m_BodyStyle;

        GUIStyle ButtonStyle
        {
            get { return m_ButtonStyle; }
        }

        [SerializeField]
        GUIStyle m_ButtonStyle;

        void Init()
        {
            if (m_Initialized)
                return;
            m_BodyStyle = new GUIStyle(EditorStyles.label);
            m_BodyStyle.wordWrap = true;
            m_BodyStyle.fontSize = 14;
            m_BodyStyle.richText = true;

            m_TitleStyle = new GUIStyle(m_BodyStyle);
            m_TitleStyle.fontSize = 26;

            m_HeadingStyle = new GUIStyle(m_BodyStyle);
            m_HeadingStyle.fontStyle = FontStyle.Bold;
            m_HeadingStyle.fontSize = 18;

            m_LinkStyle = new GUIStyle(m_BodyStyle);
            m_LinkStyle.wordWrap = false;

            // Match selection color which works nicely for both light and dark skins
            m_LinkStyle.normal.textColor = new Color(0x00 / 255f, 0x78 / 255f, 0xDA / 255f, 1f);
            m_LinkStyle.stretchWidth = false;

            m_ButtonStyle = new GUIStyle(EditorStyles.miniButton);
            m_ButtonStyle.fontStyle = FontStyle.Bold;

            m_Initialized = true;
        }

        bool LinkLabel(GUIContent label, params GUILayoutOption[] options)
        {
            var position = GUILayoutUtility.GetRect(label, LinkStyle, options);

            Handles.BeginGUI();
            Handles.color = LinkStyle.normal.textColor;
            Handles.DrawLine(new Vector3(position.xMin, position.yMax), new Vector3(position.xMax, position.yMax));
            Handles.color = Color.white;
            Handles.EndGUI();

            EditorGUIUtility.AddCursorRect(position, MouseCursor.Link);

            return GUI.Button(position, label, LinkStyle);
        }
    }
}
#endif