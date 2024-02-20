using System.Collections;
using UnityEngine;

namespace Kalkuz.Gameplay
{
  public abstract class CameraShakerBase : MonoBehaviour
  {
    [SerializeField] protected Camera cam;
    [SerializeField] protected bool fetchMainCameraOnAwake = true;

    [SerializeField] protected float shakeDuration = 1f;
    [SerializeField] protected Vector3 shakeMagnitude = Vector3.one;

    protected Coroutine shakeCoroutine;

    private void Awake()
    {
      if (fetchMainCameraOnAwake) cam = Camera.main;
    }

    public void Bound(Camera camera)
    {
      cam = camera;
    }

    public void Shake(float duration, Vector3 magnitude)
    {
      shakeDuration = duration;
      shakeMagnitude = magnitude;
      Shake();
    }

    public void Shake()
    {
      if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);
      shakeCoroutine = StartCoroutine(ShakeCoroutine());
    }

    protected abstract IEnumerator ShakeCoroutine();
  }
}