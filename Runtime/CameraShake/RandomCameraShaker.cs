using System.Collections;
using UnityEngine;

namespace KalkuzSystems.Utility.CameraShake
{
  public sealed class RandomCameraShaker : CameraShakerBase
  {
    protected override IEnumerator ShakeCoroutine()
    {
      var elapsed = 0f;
      var originalPos = cam.transform.localPosition;

      while (elapsed < shakeDuration)
      {
        var offset = Random.insideUnitSphere;
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