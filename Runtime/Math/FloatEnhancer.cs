using System;
using UnityEngine;

namespace Kalkuz.Utility.Math
{
  public enum FloatEnhancerOrder
  {
    FLAT,
    PERCENTAGE,
    FLAT_THEN_PERCENTAGE,
    PERCENTAGE_THEN_FLAT
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
          FloatEnhancerOrder.FLAT => value - flat,
          FloatEnhancerOrder.PERCENTAGE => value * (1f - percentage),
          FloatEnhancerOrder.FLAT_THEN_PERCENTAGE => (value - flat) * (1f - percentage),
          FloatEnhancerOrder.PERCENTAGE_THEN_FLAT => (value * (1f - percentage)) - flat,
          _ => value
        };
      }

      return order switch
      {
        FloatEnhancerOrder.FLAT => value + flat,
        FloatEnhancerOrder.PERCENTAGE => value * (1f + percentage),
        FloatEnhancerOrder.FLAT_THEN_PERCENTAGE => (value + flat) * (1f + percentage),
        FloatEnhancerOrder.PERCENTAGE_THEN_FLAT => (value * (1f + percentage)) + flat,
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