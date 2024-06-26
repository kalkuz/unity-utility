﻿using System;
using System.Collections.Generic;
using Kalkuz.Utility.DataStructures.SerializableDictionary;
using UnityEngine;

namespace Kalkuz.Utility.TagSystem
{
  [Serializable]
  public sealed class TagSet : Dictionary<string, List<string>>, ISerializationCallbackReceiver
  {
    [SerializeField] private List<SerializableKeyValuePair<string, List<string>>> pairs;
    
    public void OnBeforeSerialize()
    {
      pairs = new List<SerializableKeyValuePair<string, List<string>>>();
      foreach (var pair in this)
      {
        pairs.Add(new SerializableKeyValuePair<string, List<string>>(pair.Key, pair.Value));
      }
    }

    public void OnAfterDeserialize()
    {
      Clear();
      foreach (var pair in pairs)
      {
        Add(pair.Key, pair.Value);
      }
    }
  }
}