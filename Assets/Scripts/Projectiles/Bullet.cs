﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Proyectile
{
    
    private void Start() {
        SetInitialSpeed(this.GetSpeed());
    }
    

    public override void Update() {
        // translate mueve el sprite en la direccion y distancia dada (pos = pos + v*t)
        // como el sprite del proyectil esta en sentido Este el vector direccion es el vector unitario (1, 0)
        this.transform.Translate(Vector2.right * this.GetSpeed() * Time.deltaTime * this.GetGameProg().GetScale());
        Invoke("Die", GetLifeTime());
        //Debug.Log(this.GetSpeed());
    }


}
