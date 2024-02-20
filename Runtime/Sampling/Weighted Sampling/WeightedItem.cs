using System;
using UnityEngine;

namespace Kalkuz.Utility
{
  [Serializable]
  public sealed class WeightedItem<T>
  {
    [SerializeField] private T item;
    [SerializeField] private float weight;

    public T Item => item;
    public float Weight => weight;

    public WeightedItem(T item, float weight)
    {
      this.item = item;
      this.weight = weight;
    }
  }
}