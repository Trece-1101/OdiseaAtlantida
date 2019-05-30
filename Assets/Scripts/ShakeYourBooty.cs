using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeYourBooty : MonoBehaviour
{
    // Shake shake shake ♪♪♪

    #region "Atributos Serializados"
    [SerializeField] Animator CameraAnimation = null;
    #endregion

    #region "Metodos"
    public void ShakeShakeShake() {
        CameraAnimation.SetTrigger("shake");
    }

    public void UltraShake() {
        CameraAnimation.SetTrigger("ultraShake");
    }
    #endregion


}
