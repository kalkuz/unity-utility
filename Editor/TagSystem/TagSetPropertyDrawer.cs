using System.Collections.Generic;
using System.Linq;
using Kalkuz.Utility.TagSystem;
using UnityEditor;
using UnityEngine;

namespace Kalkuz.Utility.Editor.TagSystem
{
  [CustomPropertyDrawer(typeof(TagSet))]
  public sealed class TagSetPropertyDrawer : PropertyDrawer
  {
    private static TagSet m_tagSet;

    private static bool m_foldout;
    private static List<int> m_foldoutTagGroupIndices = new ();

    private static string m_key;
    private static string m_value;

    private static bool m_shouldSave;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      m_tagSet = (TagSet)fieldInfo.GetValue(property.serializedObject.targetObject);
      var keys = m_tagSet.Keys.ToList();

      EditorGUILayout.BeginVertical(GUI.skin.box);

      EditorGUILayout.BeginHorizontal(GUI.skin.box);
      m_foldout = EditorGUILayout.BeginFoldoutHeaderGroup(m_foldout, "Tag Groups");
      var newCount = EditorGUILayout.DelayedIntField("", keys.Count);

      if (newCount != keys.Count)
      {
        if (newCount > keys.Count)
        {
          for (var i = keys.Count; i < newCount; i++)
          {
            m_tagSet.Add($"Tag Group {i}", new List<string>());
            m_shouldSave = true;
          }
        }
        else
        {
          for (var i = keys.Count - 1; i >= newCount; i--)
          {
            m_tagSet.Remove(keys[i]);
            m_shouldSave = true;
          }
        }
      }

      EditorGUILayout.EndFoldoutHeaderGroup();
      EditorGUILayout.EndHorizontal();

      if (m_foldout)
      {
        EditorGUI.indentLevel++;

        EditorGUILayout.BeginVertical(GUI.skin.box);
        if (keys.Count == 0)
        {
          EditorGUILayout.LabelField("No Tag Groups", EditorStyles.centeredGreyMiniLabel);
        }

        for (var i = 0; i < keys.Count; i++)
        {
          EditorGUI.indentLevel++;

          var key = keys[i];

          var lastIndent = EditorGUI.indentLevel;
          EditorGUI.indentLevel = 0;
          EditorGUILayout.BeginHorizontal(GUI.skin.box);
          var opened = EditorGUILayout.BeginFoldoutHeaderGroup(m_foldoutTagGroupIndices.Contains(i), "Group:");
          var newKey = EditorGUILayout.DelayedTextField(key);

          if (GUILayout.Button("Remove Tag Group", GUILayout.Width(150)))
          {
            m_tagSet.Remove(key);
            opened = false;
            m_shouldSave = true;
          }

          if (newKey != key)
          {
            if (m_tagSet.TryAdd(newKey, m_tagSet[key]))
            {
              m_tagSet.Remove(key);
              opened = false;
              m_shouldSave = true;
            }
            else
            {
              Debug.LogWarning($"Tag Group with key {newKey} already exists.");
            }
          }

          EditorGUILayout.EndFoldoutHeaderGroup();
          EditorGUILayout.EndHorizontal();
          EditorGUI.indentLevel = lastIndent;

          if (!opened)
          {
            if (m_foldoutTagGroupIndices.Contains(i))
            {
              m_foldoutTagGroupIndices.Remove(i);
            }
          }
          else
          {
            if (!m_foldoutTagGroupIndices.Contains(i))
            {
              m_foldoutTagGroupIndices.Add(i);
            }

            var tags = m_tagSet[key];

            if (tags.Count == 0)
            {
              EditorGUILayout.LabelField("No Tags", EditorStyles.centeredGreyMiniLabel);
            }

            for (var j = 0; j < tags.Count; j++)
            {
              EditorGUILayout.BeginHorizontal();
              var newTag = EditorGUILayout.TextField(tags[j]);

              if (newTag != tags[j])
              {
                tags[j] = newTag;
                m_shouldSave = true;
              }

              if (GUILayout.Button("-", GUILayout.Width(20)))
              {
                tags.RemoveAt(j);
                m_shouldSave = true;
              }

              EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            EditorGUILayout.BeginHorizontal();
            GUI.SetNextControlName("Tag Input" + i);
            m_value = EditorGUILayout.TextField(m_value);
            if (GUILayout.Button("+", GUILayout.Width(20)) ||
                (Event.current.isKey && Event.current.keyCode == KeyCode.Return &&
                 GUI.GetNameOfFocusedControl() == "Tag Input" + i))
            {
              tags.Add(m_value);
              m_value = "";
              m_shouldSave = true;

              GUI.FocusControl("Tag Input" + i);
            }

            EditorGUILayout.EndHorizontal();
          }

          EditorGUI.indentLevel--;
        }

        EditorGUI.indentLevel--;

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.BeginHorizontal();
        GUI.SetNextControlName("Tag Group Key Input");
        m_key = EditorGUILayout.TextField(m_key);
        // If button is pressed or enter is pressed while focused on the _key text field
        if (GUILayout.Button("Add Tag Group", GUILayout.Width(120)) ||
            (Event.current.isKey && Event.current.keyCode == KeyCode.Return &&
             GUI.GetNameOfFocusedControl() == "Tag Group Key Input"))
        {
          if (m_tagSet.TryAdd(m_key, new List<string>()))
          {
            m_key = "";
            m_shouldSave = true;
          }
          else
          {
            Debug.LogWarning($"Tag Group with key {m_key} already exists.");
          }

          GUI.FocusControl("Tag Group Key Input");
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
      }

      EditorGUILayout.EndVertical();

      if (!m_shouldSave) return;

      Save(property);
    }

    private void Save(SerializedProperty property)
    {
      // Todo: Add Undo
      // Undo.RecordObject(property.serializedObject.targetObject, "Tags Changed");
      EditorUtility.SetDirty(property.serializedObject.targetObject);
      AssetDatabase.SaveAssets();
      m_shouldSave = false;
    }
  }
}