//// Clase derivada/Hija de Proyectile, un tipo de proyectil que es un misil con MRUA

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Proyectile
{
    #region "Atributos"
    [SerializeField] private Vector2 Aceleration; // Aceleracion del misil
    #endregion

    #region "Setters y Getters"
    public Vector2 GetAceleration() {
        return this.Aceleration;
    }
    public void SetAceleration(Vector2 value) {
        this.Aceleration = value;
    }
    #endregion

    #region "Metodos"
    private void Start() {
        // Primer metodo que se ejecuta cuando el objeto es "visto" en la jerarquia
        // Pasamos al valor de velocidad inicial el de su velocidad al instanciar el objeto
        SetInitialSpeed(this.GetSpeed());        
    }

    public override void Update() {
        // translate mueve el sprite en la direccion y distancia dada (pos = pos + v*t)
        // como el sprite del proyectil esta en sentido Este el vector direccion es el vector unitario (1, 0)

        // vel = vel + a*t
        this.SetSpeed(this.GetSpeed() + this.Aceleration * Time.deltaTime);

        // pos = pos + v*t + 1/2 a*t2
        transform.Translate(Vector2.right * ((this.GetSpeed() * Time.deltaTime) + ((this.Aceleration  * Mathf.Pow(Time.deltaTime, 2)) / 2)) * this.GetGameProg().GetScale());

        //Debug.Log(this.GetSpeed());

        //Invoke("Die", GetLifeTime());       
        
    }
    #endregion

}
