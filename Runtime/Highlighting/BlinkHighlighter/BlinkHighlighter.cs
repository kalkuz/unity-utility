using UnityEngine;

namespace KalkuzSystems.Utility.Highlighting.BlinkHighlighter
{
  /// <summary>
  ///   Base class for blink indication of objects.
  /// </summary>
  public abstract class BlinkHighlighter : Highlighter
  {
    [Header("Properties")] [SerializeField]
    protected AnimationCurve highlightCurve;

    [SerializeField] protected float blinkDuration;
    [SerializeField] protected bool isLooping;
    [SerializeField] protected bool pingPong;
    [SerializeField] protected bool playOnAwake;

    private void Awake()
    {
      if (playOnAwake) Highlight();
    }
  }
}