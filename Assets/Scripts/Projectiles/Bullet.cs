//// Clase derivada/Hija de Proyectile, un tipo de proyectil que es una bala con MRU

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Proyectile
{
    
    private void Start() {
        // Primer metodo que se ejecuta cuando el objeto es "visto" en la jerarquia
        // Pasamos al valor de velocidad inicial el de su velocidad al instanciar el objeto
        this.SetInitialSpeed(this.GetSpeed());
    }
    

    public override void Update() {
        // translate mueve el sprite en la direccion y distancia dada (pos = pos + v*t)
        // como el sprite del proyectil esta en sentido 'Este' el vector direccion es el vector unitario (1, 0)
        this.transform.Translate(Vector2.right * this.GetSpeed() * this.GetGameProg().GetScale() * Time.deltaTime );
        
        //Debug.Log(this.GetSpeed());
    }


}
