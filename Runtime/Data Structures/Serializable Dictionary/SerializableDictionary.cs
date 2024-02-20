using System.Collections.Generic;
using UnityEngine;

namespace Kalkuz.Utility
{
  [System.Serializable]
  public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
  {
    [SerializeField] private List<SerializableKeyValuePair<TKey, TValue>> elements;

    public void OnBeforeSerialize()
    {
      elements ??= new List<SerializableKeyValuePair<TKey, TValue>>();
      elements.Clear();

      foreach (var kvp in this)
      {
        elements.Add(new SerializableKeyValuePair<TKey, TValue>(kvp.Key, kvp.Value));
      }
    }

    public void OnAfterDeserialize()
    {
      Clear();

      var prevKeys = new List<TKey>();
      foreach (var kvp in elements)
      {
        if (prevKeys.Contains(kvp.Key))
        {
          Debug.LogWarning($"Duplicate key '{kvp.Key}' found in dictionary. Skipping.");
          continue;
        }

        Add(kvp.Key, kvp.Value);

        prevKeys.Add(kvp.Key);
      }
    }
  }
}