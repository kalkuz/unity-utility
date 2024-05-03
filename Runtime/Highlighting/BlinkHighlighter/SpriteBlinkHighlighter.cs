using System.Collections;
using UnityEngine;

namespace Kalkuz.Utility.Highlighting.BlinkHighlighter
{
  public sealed class SpriteBlinkHighlighter : BlinkHighlighter
  {
    [SerializeField] [Header("Renderers")] private SpriteRenderer[] spriteRenderers;

    protected override IEnumerator HighlightCoroutine()
    {
      var elapsed = 0f;
      var originalColors = new Color[spriteRenderers.Length];

      onStopCoroutine = () =>
      {
        for (var i = 0; i < spriteRenderers.Length; i++)
        {
          var sr = spriteRenderers[i];
          if (useEmissionColor)
          {
            sr.material.SetColor(emissionColorName, originalColors[i]);
          }
          else
          {
            sr.color = originalColors[i];
          }
        }
      };

      for (var i = 0; i < spriteRenderers.Length; i++)
      {
        var sr = spriteRenderers[i];
        if (useEmissionColor)
        {
          sr.material.EnableKeyword(emissionEnabledKeyword);
          originalColors[i] = sr.material.GetColor(emissionColorName);
        }
        else
        {
          originalColors[i] = sr.color;
        }
      }

      while (elapsed < blinkDuration)
      {
        var t = elapsed / blinkDuration;
        if (pingPong) t = Mathf.PingPong(t * 2, 1f);

        var color = Color.Lerp(originalColors[0], highlightColor, highlightCurve.Evaluate(t));
        foreach (var sr in spriteRenderers)
        {
          if (useEmissionColor)
          {
            sr.material.SetColor(emissionColorName, color);
          }
          else
          {
            sr.color = color;
          }
        }

        elapsed += Time.deltaTime;
        yield return null;
      }

      
      onStopCoroutine?.Invoke();
      onStopCoroutine = null;
      highlightCoroutine = null;

      if (isLooping)
      {
        Highlight();
      }
    }
  }
}