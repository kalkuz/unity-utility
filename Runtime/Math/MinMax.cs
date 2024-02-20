using System;

namespace Kalkuz.Utility.Math
{
  [Serializable]
  public sealed class MinMaxFloat
  {
    public float min;
    public float max;

    public float RandomValue => UnityEngine.Random.Range(min, max);

    public float Clamp(float value) => UnityEngine.Mathf.Clamp(value, min, max);
  }

  [Serializable]
  public sealed class MinMaxInt
  {
    public int min;
    public int max;

    public int RandomValue => UnityEngine.Random.Range(min, max);

    public int Clamp(int value) => UnityEngine.Mathf.Clamp(value, min, max);
  }

  [Serializable]
  public sealed class MinMaxVector3
  {
    public UnityEngine.Vector3 min;
    public UnityEngine.Vector3 max;

    public UnityEngine.Vector3 RandomValue => new(UnityEngine.Random.Range(min.x, max.x),
      UnityEngine.Random.Range(min.y, max.y), UnityEngine.Random.Range(min.z, max.z));

    public UnityEngine.Vector3 Clamp(UnityEngine.Vector3 value) => new(
      UnityEngine.Mathf.Clamp(value.x, min.x, max.x), UnityEngine.Mathf.Clamp(value.y, min.y, max.y),
      UnityEngine.Mathf.Clamp(value.z, min.z, max.z));
  }

  [Serializable]
  public sealed class MinMaxMagnitudeVector3
  {
    public float min;
    public float max;

    public UnityEngine.Vector3 RandomValue => UnityEngine.Random.onUnitSphere * UnityEngine.Random.Range(min, max);

    public UnityEngine.Vector3 Clamp(UnityEngine.Vector3 value, UnityEngine.Vector3 fallbackDirection = default)
    {
      var magnitude = value.magnitude;
      if (magnitude < 0.0001f) return fallbackDirection.normalized * min;
      
      if (magnitude < min) return value.normalized * min;
      if (magnitude > max) return value.normalized * max;
      return value;
    }
  }
}