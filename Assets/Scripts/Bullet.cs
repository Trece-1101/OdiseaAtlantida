using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Proyectile
{
    private void Start() {
        SetSpeed(8f);
        SetLifeTime(2f);
        Invoke("Die", GetLifeTime());
    }
       
    public override void Die() {
        gameObject.SetActive(false);
    }

    private void OnDisable() {
       CancelInvoke();
    }
}
