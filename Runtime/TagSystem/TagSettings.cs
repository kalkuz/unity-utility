using System.Collections.Generic;
using UnityEngine;

namespace Kalkuz.Utility.TagSystem
{
  public class TagSettings : ScriptableObject
  {
    private const string MID_TagSettingsPath = "Assets/TagSettings.asset";

    [SerializeField] private TagSet tagGroups;

    private static TagSettings GetOrCreateSettings()
    {
#if UNITY_EDITOR
      var settings = UnityEditor.AssetDatabase.LoadAssetAtPath<TagSettings>(MID_TagSettingsPath);

      if (settings is not null) return settings;

      settings = CreateInstance<TagSettings>();

      settings.tagGroups = new TagSet();

      UnityEditor.AssetDatabase.CreateAsset(settings, MID_TagSettingsPath);
      UnityEditor.AssetDatabase.SaveAssets();
#else
    var settings = Resources.Load<TagSettings>("TagSettings");

    if (settings is not null) return settings;

    throw new System.Exception("TagSettings not found in Resources folder.");
#endif

      return settings;
    }
    
    public static List<string> GetTags(string tagGroup)
    {
      if (!string.IsNullOrEmpty(tagGroup))
      {
        return GetOrCreateSettings().tagGroups.TryGetValue(tagGroup, out var tags) ? tags : new List<string>();
      }
      
      var allTags = new List<string>();
      foreach (var tagGroupValue in GetOrCreateSettings().tagGroups.Values)
      {
        allTags.AddRange(tagGroupValue);
      }
      return allTags;
    }
    
#if UNITY_EDITOR
    internal static UnityEditor.SerializedObject GetSerializedSettings()
    {
      return new UnityEditor.SerializedObject(GetOrCreateSettings());
    }
    
    public static void OpenTagSettingsWindow()
    {
      UnityEditor.SettingsService.OpenProjectSettings("Project/Kalkuz/Tag Settings");
    }
#endif
  }

#if UNITY_EDITOR
  // Register a SettingsProvider using IMGUI for the drawing framework:
  internal static class TagSettingsProvider
  {
    [UnityEditor.SettingsProvider]
    public static UnityEditor.SettingsProvider CreateMyCustomSettingsProvider()
    {
      var provider = new UnityEditor.SettingsProvider("Project/Kalkuz/Tag Settings", UnityEditor.SettingsScope.Project)
      {
        guiHandler = (searchContext) =>
        {
          var settings = TagSettings.GetSerializedSettings();
          UnityEditor.EditorGUILayout.PropertyField(settings.FindProperty("tagGroups"), true);
          settings.ApplyModifiedProperties();
        },
        keywords = new HashSet<string>(new[] { "Tag", "Settings" })
      };

      return provider;
    }
  }
#endif
}