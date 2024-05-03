using UnityEngine;

namespace Kalkuz.Utility.TagSystem
{
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