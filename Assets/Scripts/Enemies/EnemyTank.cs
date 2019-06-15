//// Clase Derivada/Hija de "Enemy" que describe a a enemigos que no disparan balas comunes
//// sino balas afectadas por la gravedad que producen un tiro oblicuo

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : Enemy
{
    [SerializeField] bool ShouldRotate; // Una variable poder generar una clase de tanque que no rota y suelta balas en caida libre

    public override void Awake() {
        base.Awake();
       // this.SetMyBulletVFX("TankBullet"); // El tipo de bala del enemigo
    }

    public override void CheckRotation() {
        if (!ShouldRotate) { return; }
        // En este caso el enemigo no persigue al jugador sino que tiene una orientacion definida de 45°
        if (this.GetPlayer() == null || !this.GetPlayer().GetIsAlive()) { return; }
        if (this.GetPlayer().transform.position.x > this.transform.position.x) {
            // si el jugador esta a la derecha el angulo es -45°
            this.transform.rotation = Quaternion.Euler(0f, 0f, -45f);
        }
        else {
            // si el jugador esta a la izquierda el angulo es 45°
            this.transform.rotation = Quaternion.Euler(0f, 0f, 45f);
        }
    }
    
}
