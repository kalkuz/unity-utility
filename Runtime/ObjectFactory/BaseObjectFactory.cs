using UnityEngine;

namespace Kalkuz.Utility.ObjectFactory
{
  public abstract class BaseObjectFactory<T> : ScriptableObject
  {
    public abstract T Get();
  }
}