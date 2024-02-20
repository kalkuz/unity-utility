using System;
using UnityEngine;

namespace KalkuzSystems.Utility.Math
{
  public enum FloatEnhancerOrder
  {
    [InspectorName("Flat Only")] Flat,
    [InspectorName("Percentage Only")] Percentage,
    [InspectorName("First Flat, Then Percentage")] FlatThenPercentage,
    [InspectorName("First Percentage, Then Flat")] PercentageThenFlat
  }

  [Serializable]
  public sealed class FloatEnhancer
  {
    public float flat;
    public float percentage;
    public FloatEnhancerOrder order;

    public float Calculate(float value, bool negate = false)
    {
      if (negate)
      {
        return order switch
        {
          FloatEnhancerOrder.Flat => value - flat,
          FloatEnhancerOrder.Percentage => value * (1f - percentage),
          FloatEnhancerOrder.FlatThenPercentage => (value - flat) * (1f - percentage),
          FloatEnhancerOrder.PercentageThenFlat => (value * (1f - percentage)) - flat,
          _ => value
        };
      }

      return order switch
      {
        FloatEnhancerOrder.Flat => value + flat,
        FloatEnhancerOrder.Percentage => value * (1f + percentage),
        FloatEnhancerOrder.FlatThenPercentage => (value + flat) * (1f + percentage),
        FloatEnhancerOrder.PercentageThenFlat => (value * (1f + percentage)) + flat,
        _ => value
      };
    }

    public Vector2 Calculate(Vector2 value, bool negate = false)
    {
      return new Vector2(Calculate(value.x, negate), Calculate(value.y, negate));
    }

    public Vector3 Calculate(Vector3 value, bool negate = false)
    {
      return new Vector3(Calculate(value.x, negate), Calculate(value.y, negate), Calculate(value.z, negate));
    }
  }
}