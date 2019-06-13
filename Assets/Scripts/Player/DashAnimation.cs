//// Clase muy sencilla que solo se encarga de desactivar (hacer desaparecer) la animacion del Dash, ya que esta es llamada desde el pool

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAnimation : MonoBehaviour
{
    private void End() {
        gameObject.SetActive(false);
    }
}
