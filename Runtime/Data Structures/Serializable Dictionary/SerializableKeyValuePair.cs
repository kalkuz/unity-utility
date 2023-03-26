using UnityEngine;

namespace KalkuzSystems.Utility
{
  [System.Serializable]
  public sealed class SerializableKeyValuePair<TKey, TValue>
  {
    [SerializeField] private TKey key;
    [SerializeField] private TValue value;
    
    public TKey Key => key;
    public TValue Value => value;

    public SerializableKeyValuePair(TKey key, TValue value)
    {
      this.key = key;
      this.value = value;
    }
  }
}