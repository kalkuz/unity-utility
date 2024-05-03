using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kalkuz.Utility.TagSystem
{
  [Serializable]
  public sealed class MultipleTags : ISerializationCallbackReceiver
  {
    [SerializeField] private List<string> tags = new ();

    public HashSet<string> HashSet { get; private set; } = new ();

    public void OnBeforeSerialize()
    {
      tags = new List<string>(HashSet);
    }

    public void OnAfterDeserialize()
    {
      HashSet = new HashSet<string>(tags);
    }

    public bool Contains(string tag)
    {
      return HashSet.Contains(tag);
    }
  }
}