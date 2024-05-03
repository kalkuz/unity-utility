using System;
using System.Collections;
using System.Collections.Generic;

namespace Kalkuz.Utility.DataStructures.InterdimensionalArray
{
  [Serializable]
  public struct InterdimensionalArray<T> : IEnumerable<T>
  {
    public int Width { get; set; }
    public int Height { get; set; }

    private readonly T[] _array;

    public InterdimensionalArray(int width, int height)
    {
      Width = width;
      Height = height;
      _array = new T[width * height];
    }

    public T this[int x, int y]
    {
      get => _array[y * Width + x];
      set => _array[y * Width + x] = value;
    }
    
    public T this[int index]
    {
      get => _array[index];
      set => _array[index] = value;
    }
    
    public static implicit operator T[](InterdimensionalArray<T> array) => array._array;
    
    public IEnumerator<T> GetEnumerator()
    {
      return ((IEnumerable<T>)_array).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}