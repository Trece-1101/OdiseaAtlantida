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

    public abstract void Die();

    private void Update() {
        // translate mueve el sprite en la direccion y distancia dada (pos = pos + v*t)
        // como el sprite del proyectil esta en sentido Este el vector direccion es el vector unitario (1, 0)
        transform.Translate(Vector2.right * this.Speed * Time.deltaTime);
    }

}
