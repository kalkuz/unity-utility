namespace KalkuzSystems.Utility.Math
{
  public struct MathUtility
  {
    public float Map(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
      return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
    }
  }
}