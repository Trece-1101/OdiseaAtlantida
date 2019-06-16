//// Clase derivada/Hija de PowerUp. Un poder que relentiza el tiempo en 1/4

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTimePU : PowerUp
{
    private float BulletTimeFactor = 0.25f; // Factor de relentizacion del tiempo
    private int BulletTimeShootFactor = 2; // Factor para modificar tiempo entre disparos
    private int PlayerFactor = 4; // Factor para modificar movimiento del Player

    public override void MakeYourMagic() {
        // Metodo que controla la "magia" del PowerUp

        //Debug.Log($"Asimov Speed {this.GetAsimov().GetVelocity()}");
        //Debug.Log($"Asimov TimeBetweenBullet {this.GetAsimov().TimeBetweenBulletShoots}");
        //Debug.Log($"Asimov TimeBetweenMissile {this.GetAsimov().TimeBetweenMissileShoots}");

        Time.timeScale = BulletTimeFactor; // Relentizo el tiempo
        // Modifico la velocidad de la nave de manera inversamente proporcional de manera tal que el movimiento percibido es el mismo
        this.GetAsimov().SetVelocity(this.GetAsimov().GetVelocity() * PlayerFactor); 
        this.GetAsimov().TimeBetweenBulletShoots /= BulletTimeShootFactor; // Disminuyo el tiempo en que se pueden disparar balas y misiles
        this.GetAsimov().TimeBetweenMissileShoots /= BulletTimeShootFactor;

        Invoke(this.GetRevertPowerUpMethod(), this.GetCoolTime() / PlayerFactor); // Revierto el PowerUp en CoolTime segundos y modificado por el factor del player para que no cambie el tiempo
    }

    private void RevertYourMagic() {
        // Vuelvo todos los valores a sus originales
        Time.timeScale = 1f;
        this.GetAsimov().SetVelocity(this.GetAsimov().GetVelocity() / PlayerFactor);
        this.GetAsimov().TimeBetweenBulletShoots *= BulletTimeShootFactor;
        this.GetAsimov().TimeBetweenMissileShoots *= BulletTimeShootFactor;

        //Debug.Log($"Asimov Speed {this.GetAsimov().GetVelocity()}");
        //Debug.Log($"Asimov Speed {this.GetAsimov().TimeBetweenBulletShoots}");
        //Debug.Log($"Asimov TimeBetweenMissile {this.GetAsimov().TimeBetweenMissileShoots}");
    }

    
}
