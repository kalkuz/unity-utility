using System;
using UnityEngine;

namespace Kalkuz.Utility.Math
{
  public enum FloatEnhancerOrder
  {
    [InspectorName("Flat Only")] Flat,
    [InspectorName("Percentage Only")] Percentage,

    [InspectorName("First Flat, Then Percentage")]
    FlatThenPercentage,

    [InspectorName("First Percentage, Then Flat")]
    PercentageThenFlat
  }

  public static class FloatEnhancerOrderExtensions
  {
    public static FloatEnhancerOrder Merge(this FloatEnhancerOrder a, FloatEnhancerOrder b)
    {
      return a switch
      {
        FloatEnhancerOrder.Flat => b switch
        {
          FloatEnhancerOrder.Flat => FloatEnhancerOrder.Flat,
          FloatEnhancerOrder.Percentage => FloatEnhancerOrder.FlatThenPercentage,
          FloatEnhancerOrder.FlatThenPercentage => FloatEnhancerOrder.FlatThenPercentage,
          FloatEnhancerOrder.PercentageThenFlat => FloatEnhancerOrder.PercentageThenFlat,
          _ => FloatEnhancerOrder.Flat
        },
        FloatEnhancerOrder.Percentage => b switch
        {
          FloatEnhancerOrder.Flat => FloatEnhancerOrder.PercentageThenFlat,
          FloatEnhancerOrder.Percentage => FloatEnhancerOrder.Percentage,
          FloatEnhancerOrder.FlatThenPercentage => FloatEnhancerOrder.FlatThenPercentage,
          FloatEnhancerOrder.PercentageThenFlat => FloatEnhancerOrder.PercentageThenFlat,
          _ => FloatEnhancerOrder.Percentage
        },
        FloatEnhancerOrder.FlatThenPercentage => b switch
        {
          FloatEnhancerOrder.Flat => FloatEnhancerOrder.FlatThenPercentage,
          FloatEnhancerOrder.Percentage => FloatEnhancerOrder.FlatThenPercentage,
          FloatEnhancerOrder.FlatThenPercentage => FloatEnhancerOrder.FlatThenPercentage,
          FloatEnhancerOrder.PercentageThenFlat => FloatEnhancerOrder.PercentageThenFlat,
          _ => FloatEnhancerOrder.FlatThenPercentage
        },
        FloatEnhancerOrder.PercentageThenFlat => b switch
        {
          FloatEnhancerOrder.Flat => FloatEnhancerOrder.PercentageThenFlat,
          FloatEnhancerOrder.Percentage => FloatEnhancerOrder.PercentageThenFlat,
          FloatEnhancerOrder.FlatThenPercentage => FloatEnhancerOrder.PercentageThenFlat,
          FloatEnhancerOrder.PercentageThenFlat => FloatEnhancerOrder.PercentageThenFlat,
          _ => FloatEnhancerOrder.PercentageThenFlat
        },
        _ => FloatEnhancerOrder.Flat
      };
    }
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

    public static FloatEnhancer operator +(FloatEnhancer a, FloatEnhancer b)
    {
      return new FloatEnhancer
      {
        flat = a.flat + b.flat,
        percentage = a.percentage + b.percentage,
        order = a.order.Merge(b.order)
      };
    }

    public static FloatEnhancer operator -(FloatEnhancer a, FloatEnhancer b)
    {
      return new FloatEnhancer
      {
        flat = a.flat - b.flat,
        percentage = a.percentage - b.percentage,
        order = a.order.Merge(b.order)
      };
    }
    
    public static FloatEnhancer operator *(FloatEnhancer a, FloatEnhancer b)
    {
      return new FloatEnhancer
      {
        flat = a.flat * b.flat,
        percentage = a.percentage * b.percentage,
        order = a.order.Merge(b.order)
      };
    }
    
    public static FloatEnhancer operator /(FloatEnhancer a, FloatEnhancer b)
    {
      return new FloatEnhancer
      {
        flat = a.flat / b.flat,
        percentage = a.percentage / b.percentage,
        order = a.order.Merge(b.order)
      };
    }
  }
}