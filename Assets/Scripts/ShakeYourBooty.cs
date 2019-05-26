using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeYourBooty : MonoBehaviour
{
    // Shake shake shake ♪♪♪

    [SerializeField] Animator CameraAnimation;

    public void ShakeShakeShake() {
        CameraAnimation.SetTrigger("shake");
    }

    public void UltraShake() {
        CameraAnimation.SetTrigger("ultraShake");
    }

    //public IEnumerator ShakeShakeShake(float duration, float magnitude) {
    //    Vector3 originalPosition = transform.localPosition;
    //    float elapsed = 0.0f;

    //    while(elapsed < duration) {
    //        float x = Random.Range(-1f, 1f) * magnitude;
    //        float y = Random.Range(-1f, 1f) * magnitude;

    //        transform.localPosition = new Vector3(x, y, originalPosition.z);

    //        elapsed += Time.deltaTime;

    //        yield return null;
    //    }

    //    transform.localPosition = originalPosition;

    //}

}
