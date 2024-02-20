using System.Collections;
using UnityEngine;

namespace Kalkuz.Gameplay
{
  public sealed class ProfiledCameraShake : CameraShakerBase
  {
    [SerializeField] private AnimationCurve profileX;
    [SerializeField] private AnimationCurve profileY;
    [SerializeField] private AnimationCurve profileZ;
    [SerializeField] private bool pingPong;

    protected override IEnumerator ShakeCoroutine()
    {
      var elapsed = 0f;
      var originalPos = cam.transform.localPosition;

      while (elapsed < shakeDuration)
      {
        var t = elapsed / shakeDuration;
        if (pingPong) t = Mathf.PingPong(t * 2, 1f);

        var offset = new Vector3(profileX.Evaluate(t), profileY.Evaluate(t), profileZ.Evaluate(t));
        offset.Scale(shakeMagnitude);

        cam.transform.localPosition = originalPos + offset;

        elapsed += Time.deltaTime;
        yield return null;
      }

      cam.transform.localPosition = originalPos;
      shakeCoroutine = null;
    }
  }
}