using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalEnemy : Enemy
{
    private float OrbitDistance = 2f;
    private float OrbitDegreesPerSecond = 90f;
    private Vector3 Zaxis = new Vector3(0f, 0f, 1f);
    private bool FirstDisable = true;


    private void OnEnable() {
        this.HitPoints = this.GetOriginalHitPoints();
    }

    public override void Update() {
        base.Update();

        Transform target = transform.parent; // el eje de rotacion es el padre (jerarquico)

        // pos = posEje + (pos - posEje).normal * d
        transform.position = target.position + (transform.position - target.position).normalized * this.OrbitDistance;
        transform.RotateAround(target.position, this.Zaxis, this.OrbitDegreesPerSecond * Time.deltaTime);
    }

    public override void Die() {
        // Metodo de muerte del enemigo

        this.gameObject.SetActive(false);

        this.PlayExplosion(); // Metodo que muestra la explosion al destruirse        
    }

    private void OnDisable() {
        if (!FirstDisable) {
            //this.GetGameProg().AddScore(this.GetReward()); // Le agregamos al Score el Reward del enemigo
            this.PowerUpRoulette(); // Metodo para controlar spawn de PowerUps            
        }
        else {
            this.FirstDisable = false;
        }
    }


}
