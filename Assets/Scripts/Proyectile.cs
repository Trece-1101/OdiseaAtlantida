using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Proyectile : MonoBehaviour
{
    private float Speed;
    private float LifeTime;

    public float GetSpeed() {
        return this.Speed;
    }
    public void SetSpeed(float value) {
        this.Speed = value;
    }

    public float GetLifeTime() {
        return this.LifeTime;
    }
    public void SetLifeTime(float value) {
        this.LifeTime = value;
    }

    public abstract void Update();


    private void OnDisable() {
        CancelInvoke();
    }

    private void Die() {
        gameObject.SetActive(false);
    }

}
