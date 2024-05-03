using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Kalkuz.Utility.Extensions
{
  public static class EnumerableExtensions
  {
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
      foreach (var item in enumerable)
      {
        action(item);
      }
    }

    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
    {
      var index = 0;
      foreach (var item in enumerable)
      {
        action(item, index);
        index++;
      }
    }

    public static void Shuffle<T>(this IList<T> list)
    {
      var n = list.Count;
      while (n > 1)
      {
        n--;
        var k = Random.Range(0, n + 1);
        (list[k], list[n]) = (list[n], list[k]);
      }
    }

    public static T RandomElement<T>(this IList<T> list)
    {
      return list[Random.Range(0, list.Count)];
    }
  }
}