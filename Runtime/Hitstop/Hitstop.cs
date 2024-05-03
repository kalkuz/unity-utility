using System.Collections;
using UnityEngine;

namespace Kalkuz.Utility.Hitstop
{
  public sealed class Hitstop : MonoBehaviour
  {
    private static Coroutine m_doCoroutine;
    private static float m_timePreviously;
    private static Hitstop Instance { get; set; }

    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(this);
    }

    public static void Do(float duration, float timeScale)
    {
      if (Instance is null) throw new System.Exception("Hitstop instance not found.");
      
      if (m_doCoroutine != null)
      {
        Instance.StopCoroutine(m_doCoroutine);
        Time.timeScale = m_timePreviously;
      }

      m_doCoroutine = Instance.StartCoroutine(DoCoroutine(duration, timeScale));
    }

    private static IEnumerator DoCoroutine(float duration, float timeScale)
    {
      m_timePreviously = Time.timeScale;

      Time.timeScale = timeScale;
      yield return new WaitForSeconds(duration);
      Time.timeScale = m_timePreviously;

      m_doCoroutine = null;
    }
  }
}