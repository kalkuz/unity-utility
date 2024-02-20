using JetBrains.Annotations;
using KalkuzSystems.Utility.TagSystem;
using UnityEditor;
using UnityEngine;

namespace Kalkuz.Utility.Editor.TagSystem
{
  [CustomPropertyDrawer(typeof(TagSelectAttribute))]
  public class TagSelectPropertyDrawer : PropertyDrawer
  {
    private bool _dropdownOpen;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      var tagSelectAttribute = (TagSelectAttribute)attribute;
      var allOptions = TagSettings.GetTags(tagSelectAttribute.TagGroup);

      if (allOptions.Count == 0)
      {
        EditorGUILayout.LabelField(label.text, $"No tags found in {tagSelectAttribute.TagGroup}.");
        return;
      }

      switch (property.type)
      {
        case "string":
        {
          var index = Mathf.Max(0, allOptions.IndexOf(property.stringValue));

          var labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
          EditorGUI.LabelField(labelRect, label);

          var popupRect = new Rect(position.x + EditorGUIUtility.labelWidth + 2, position.y,
            position.width - EditorGUIUtility.labelWidth, position.height);
          var newIndex = EditorGUI.Popup(popupRect, index, allOptions.ToArray());

          property.stringValue = allOptions[newIndex];
          break;
        }
        case nameof(MultipleTags):
        {
          var multipleTags = (MultipleTags)fieldInfo.GetValue(property.serializedObject.targetObject);

          var labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
          EditorGUI.LabelField(labelRect, label);

          var buttonRect = new Rect(position.x + EditorGUIUtility.labelWidth + 2, position.y,
            position.width - EditorGUIUtility.labelWidth, position.height);

          var dropdownContent =
            multipleTags.HashSet.Count > 0 ? string.Join(", ", multipleTags.HashSet) : "<Select Tags>";

          if (EditorGUI.DropdownButton(buttonRect, new GUIContent(dropdownContent), FocusType.Passive,
                EditorStyles.popup))
          {
            _dropdownOpen = !_dropdownOpen;
          }

          if (_dropdownOpen)
          {
            var dropdownRect = new Rect(buttonRect.x, 0, buttonRect.width, EditorGUIUtility.singleLineHeight);

            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Remove All"), false, () =>
            {
              multipleTags.HashSet.Clear();
              property.serializedObject.ApplyModifiedProperties();
            });

            foreach (var option in allOptions)
            {
              menu.AddItem(new GUIContent(option), multipleTags.HashSet.Contains(option), () =>
              {
                if (!multipleTags.HashSet.Add(option))
                {
                  multipleTags.HashSet.Remove(option);
                }

                property.serializedObject.ApplyModifiedProperties();
                _dropdownOpen = true;
              });
            }

            menu.DropDown(dropdownRect);

            _dropdownOpen = false;
          }

          break;
        }
        default:
          EditorGUILayout.LabelField(label.text, $"Use TagSelect with string or {nameof(MultipleTags)} fields only.");
          return;
      }

      property.serializedObject.ApplyModifiedProperties();
    }
  }

  public sealed class TagSelectAttribute : PropertyAttribute
  {
    public readonly string TagGroup;
    
    public TagSelectAttribute()
    {
      TagGroup = string.Empty;
    }

    public TagSelectAttribute(string tagGroup)
    {
      TagGroup = tagGroup;
    }
  }
}