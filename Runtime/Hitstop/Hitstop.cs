using System.Collections;
using UnityEngine;

namespace Kalkuz.Gameplay
{
  public sealed class Hitstop : MonoBehaviour
  {
    private Coroutine doCoroutine;
    private float timePreviously;
    public static Hitstop Instance { get; private set; }

    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(this);
    }

    public void Do(float duration, float timeScale)
    {
      if (doCoroutine != null)
      {
        StopCoroutine(doCoroutine);
        Time.timeScale = timePreviously;
      }

      doCoroutine = StartCoroutine(DoCoroutine(duration, timeScale));
    }

    private IEnumerator DoCoroutine(float duration, float timeScale)
    {
      timePreviously = Time.timeScale;

      Time.timeScale = timeScale;
      yield return new WaitForSeconds(duration);
      Time.timeScale = timePreviously;

      doCoroutine = null;
    }
  }
}