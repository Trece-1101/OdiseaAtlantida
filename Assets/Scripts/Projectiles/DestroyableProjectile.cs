using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableProjectile : Proyectile
{
    private float HitPoints = 30f;

    private ObjectPool Pool;

    private void Start() {
        SetInitialSpeed(this.GetSpeed());
        Pool = FindObjectOfType<ObjectPool>();
    }


    public override void Update() {
        this.transform.Translate(Vector2.right * this.GetSpeed() * Time.deltaTime * this.GetGameProg().GetScale());
        Invoke("Die", GetLifeTime() * 2);
    }

    private void OnTriggerEnter2D(Collider2D collision) {        
        this.SetDamageCtrl(collision.gameObject.GetComponent<DamageControl>());
        if (this.GetDamageCtrl() != null)  {
            ReceiveDamage(this.GetDamageCtrl());
            PlayImpactSFX();
            collision.gameObject.SetActive(false);
        }        
    }

    private void ReceiveDamage(DamageControl damageCtrl) {
        this.HitPoints = this.HitPoints - damageCtrl.GetDamage();       

        if (this.HitPoints <= 0) {
            this.Die();
        }
    }

    public void PlayImpactSFX() {
        this.Pool.Spawn("ProyectileExplosion", this.transform.position, Quaternion.identity);
    }

}
