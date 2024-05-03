using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Kalkuz.Utility.Sampling.WeightedSampling
{
  [Serializable]
  public sealed class WeightedSampler<T>
  {
    [SerializeField] private List<WeightedItem<T>> items;
    
    private float totalWeight;
    private float[] cumulativeWeights;

    public WeightedSampler(List<WeightedItem<T>> items)
    {
      this.items = items;
      
      Initialize();
    }

    public void Initialize()
    {
      totalWeight = items.Sum(item => item.Weight);

      // Precompute the cumulative weight array
      cumulativeWeights = new float[items.Count];
      float sum = 0;
      for (var i = 0; i < items.Count; i++)
      {
        sum += items[i].Weight;
        cumulativeWeights[i] = sum;
      }
    }

    public T Sample()
    {
      var sample = Random.value * totalWeight;

      // Binary search for the item corresponding to the sample value in the cumulative weight array
      var index = Array.BinarySearch(cumulativeWeights, sample);
      if (index < 0)
      {
        index = ~index;
      }

      if (index >= items.Count)
      {
        // If the index is out of range, return the last item in the list
        return items[^1].Item;
      }
      else
      {
        return items[index].Item;
      }
    }
  }
}