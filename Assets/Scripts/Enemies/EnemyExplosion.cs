//// Clase muy sencilla que desactiva (hace desaparecer) la animacion de explosion de los enemigos

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosion : MonoBehaviour
{
    private void End() {
        gameObject.SetActive(false);
    }
}
