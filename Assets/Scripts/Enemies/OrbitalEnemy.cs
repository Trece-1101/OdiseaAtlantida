using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalEnemy : Enemy
{
    private float OrbitDistance = 2f; // Distancia de orbita (radio de la circunferencia)
    private float OrbitDegreesPerSecond = 90f; // Cantidad de grados por segundo de orbita
    private Vector3 Zaxis = new Vector3(0f, 0f, 1f); // Eje de rotacion
    private bool FirstDisable = true; // Auxiliar


    private void OnEnable() {
        // Cada vez que se activa un minion recupera sus puntos de vida originales
        this.HitPoints = this.GetOriginalHitPoints();
    }

    public override void Update() {
        base.Update();

        Transform target = transform.parent; // el eje de rotacion es el padre (jerarquico)

        // una vez que esta creado el minion lo posicionamos a una distancia orbitDistance
        // pos = posEje + (pos - posEje).normal * d
        this.transform.position = target.position + (this.transform.position - target.position).normalized * this.OrbitDistance;
        this.transform.RotateAround(target.position, this.Zaxis, this.OrbitDegreesPerSecond * Time.deltaTime);
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
