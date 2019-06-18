//// Clase derivada/Hija de Proyectile, un tipo de proyectil puede ser destruido
///

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableProjectile : Proyectile
{

    #region "Atributos"
    private float HitPoints = 25f; // Puntos de vida del proyectil
    #endregion

    #region "Componentes en Cache"
    private ObjectPool Pool; // Referencia al Pool de objetos
    #endregion

    #region "Setters y Getters"
    public ObjectPool GetPool() {
        return this.Pool;
    }
    public void SetPool(ObjectPool value) {
        this.Pool = value;
    }
    #endregion

    #region "Metodos"
    public override void Awake() {
        // Primer metodo que se ejecuta cuando el objeto es "visto" en la jerarquia
        // Enlazamos los componentes en cache con sus respectivas referencias
        base.Awake();
        this.Pool = ObjectPool.Instance;

        // Pasamos al valor de velocidad inicial el de su velocidad al instanciar el objeto
        this.SetInitialSpeed(this.GetSpeed() * this.GetGameSessionControl().GetScale());
    }    

    public override void Update() {
        // pos = pos + v * t
        this.transform.Translate(Vector2.right * this.GetSpeed() * Time.deltaTime * this.GetGameSessionControl().GetScale());        
    }

    public override void OnEnable() {
        // Aumentamos un poco el tiempo de muerte
        Invoke("Die", GetLifeTime() * 2f);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        // Metodo que controla la collision de triggers con el objeto. Para esto nos valemos de la clase DamageControl
        this.SetDamageCtrl(collision.gameObject.GetComponent<DamageControl>());
        if (this.GetDamageCtrl() != null)  {
            // Si el objeto con el que chocamos posee en sus componentes una clase DamageControl implica que 
            // es un objeto "que hace daño"
            this.ReceiveDamage(this.GetDamageCtrl()); // Controlamos el daño recibido 
            collision.gameObject.SetActive(false); // Desactivamos el objeto que colisiono contra el proyectil
            this.PlayImpactVFX(); // Ejecutamos la animacion de impacto
        }        
    }

    private void ReceiveDamage(DamageControl damageCtrl) {
        // Metodo que controla el daño recibido por un objeto que realiza daño (parcial)
        // A los puntos actuales le restamos el daño recibido
        this.HitPoints = this.HitPoints - damageCtrl.GetDamage();

        // Si los puntos de vida son 0 (o menor) Ejecutamos el metodo de morir
        if (this.HitPoints <= 0) {
            this.Die();
        }
    }

    public void PlayImpactVFX() {
        this.Pool.Spawn("ProyectileExplosion", this.transform.position, Quaternion.identity);
    }
    #endregion
}
