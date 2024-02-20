using System;
using System.Collections;
using UnityEngine;

namespace KalkuzSystems.Utility.Highlighting
{
  /// <summary>
  ///   Base class for indicating objects.
  /// </summary>
  public abstract class Highlighter : MonoBehaviour
  {
    [ColorUsage(true, true)] [SerializeField]
    protected Color highlightColor = Color.white;

    [SerializeField] [Header("Emission")] protected bool useEmissionColor;
    [SerializeField] protected string emissionEnabledKeyword = "_EMISSION";
    [SerializeField] protected string emissionColorName = "_EmissionColor";

    protected Coroutine highlightCoroutine;
    protected Action onStopCoroutine;

    public void Highlight()
    {
      onStopCoroutine?.Invoke();
      if (highlightCoroutine != null) StopCoroutine(highlightCoroutine);
      highlightCoroutine = StartCoroutine(HighlightCoroutine());
    }

    public void StopHighlight()
    {
      onStopCoroutine?.Invoke();
      if (highlightCoroutine != null) StopCoroutine(highlightCoroutine);
      highlightCoroutine = null;
    }

    protected abstract IEnumerator HighlightCoroutine();
  }
}