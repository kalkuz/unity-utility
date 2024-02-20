using UnityEngine;

namespace KalkuzSystems.Utility.DataStructures.SerializableDictionary
{
  [System.Serializable]
  public sealed class SerializableKeyValuePair<TKey, TValue>
  {
    [field: SerializeField] public TKey Key { get; set; }
    [field: SerializeField] public TValue Value { get; set; }

    public SerializableKeyValuePair(TKey key, TValue value)
    {
      Key = key;
      Value = value;
    }
  }
}