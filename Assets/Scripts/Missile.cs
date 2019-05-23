using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Proyectile
{
    private float Aceleration;

    public float GetAceleration() {
        return this.Aceleration;
    }
    public void SetAceleration(float value) {
        this.Aceleration = value;
    }

    private void Start() {
        SetSpeed(3f);
        SetInitialSpeed(this.GetSpeed());
        this.Aceleration = 4f;
    }

    public override void Update() {
        // translate mueve el sprite en la direccion y distancia dada (pos = pos + v*t)
        // como el sprite del proyectil esta en sentido Este el vector direccion es el vector unitario (1, 0)

        // vel = vel + a*t
        // pos = pos + v*t + 1/2 a*t2

        this.SetSpeed(this.GetSpeed() + this.Aceleration * Time.deltaTime);
        transform.Translate(Vector2.right * ((this.GetSpeed() * Time.deltaTime) + ((this.Aceleration * Mathf.Pow(Time.deltaTime, 2)) / 2)));

        //transform.Translate(Vector2.right * this.GetSpeed() * Time.deltaTime);
        Invoke("Die", GetLifeTime());
        
    }
}
