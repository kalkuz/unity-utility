using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KalkuzSystems.Utility.TagSystem
{
  public class TagSettings : ScriptableObject
  {
    private const string MID_TagSettingsPath = "Assets/TagSettings.asset";

    [SerializeField] private TagSet tagGroups;

    private static TagSettings GetOrCreateSettings()
    {
      var settings = AssetDatabase.LoadAssetAtPath<TagSettings>(MID_TagSettingsPath);
      
      if (settings is not null) return settings;
      
      settings = CreateInstance<TagSettings>();

      settings.tagGroups = new TagSet();
      
      AssetDatabase.CreateAsset(settings, MID_TagSettingsPath);
      AssetDatabase.SaveAssets();
      
      return settings;
    }
    
    public static List<string> GetTags(string tagGroup)
    {
      if (!string.IsNullOrEmpty(tagGroup)) return GetOrCreateSettings().tagGroups[tagGroup];
      
      var allTags = new List<string>();
      foreach (var tagGroupValue in GetOrCreateSettings().tagGroups.Values)
      {
        allTags.AddRange(tagGroupValue);
      }
      return allTags;
    }

    internal static SerializedObject GetSerializedSettings()
    {
      return new SerializedObject(GetOrCreateSettings());
    }
  }
  
  // Register a SettingsProvider using IMGUI for the drawing framework:
  internal static class TagSettingsProvider
  {
    [SettingsProvider]
    public static SettingsProvider CreateMyCustomSettingsProvider()
    {
      var provider = new SettingsProvider("Project/Kalkuz/Tag Settings", SettingsScope.Project)
      {
        guiHandler = (searchContext) =>
        {
          var settings = TagSettings.GetSerializedSettings();
          EditorGUILayout.PropertyField(settings.FindProperty("tagGroups"), true);
          settings.ApplyModifiedProperties();
        },
        keywords = new HashSet<string>(new[] { "Tag", "Settings" })
      };

      return provider;
    }
  }
}