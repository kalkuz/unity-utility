using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Utility
{
  [System.Serializable]
  public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
  {
    [SerializeField] private TKey defaultKey;
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

      // Create a temporary dictionary for previous keys to check duplication
      var prevKeys = new List<TKey>();
      foreach (var kvp in elements)
      {
        if (prevKeys.Contains(kvp.Key))
        {
          if (defaultKey is not null)
          {
            try
            {
              Add(defaultKey, kvp.Value);
            }
            catch (UnityException e)
            {
              throw e.Message.Contains("ToString")
                ? new UnityException("There seem to be a problem with keys when tried to add a new one. Check the default key and those are in the dictionary already, they should be different.")
                : e;
            }
          }
          else
          {
            Debug.LogWarning($"The defaultKey property seemed to be a null value. Try a value other than null.");
          }
        }
        else
        {
          Add(kvp.Key, kvp.Value);
        }

        prevKeys.Add(kvp.Key);
      }
    }
  }
}