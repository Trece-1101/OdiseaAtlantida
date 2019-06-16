//// Clase derivada/Hija de PowerUp. Un poder que dispara balas desde el escudo en cruz o espiral

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralShieldShootPU : PowerUp
{
    private float TimeForShoot = 0.05f; // Tiempo para realizar cada disparo
    private float TimeForRound; // Tiempo de incio de cada ronda/giro
    private int NumberOfRounds = 4;

    public override void MakeYourMagic() {
        // Metodo que controla la "magia" del PowerUp

        this.TimeForRound = this.TimeForShoot * this.NumberOfRounds; // Cada 4 disparos = 1 ronda
        this.GetAsimov().GetMyShield().GetShieldOnFront(); // Pongo el escudo al frente

        // Cantidad de "vueltas" que va a dar el escudo = 4 vueltas * 4 posiciones (N,E,S,O) = 16

        for (int j = 0; j < this.NumberOfRounds; j++) {
            // 4 giros (Giro 0 => 0 sg, Giro 1 => 0.2 sg, Giro 2 => 0.4sg, Giro 3 => 0.6sg) 
            Invoke("ShootRound", this.TimeForRound * j); // Invoco al metodo de disparar cada Giro
        }


        //Invoke("RevertYourMagic", this.GetCoolTime());
    }

    private void ShootRound() {
        for (int i = 0; i < this.NumberOfRounds; i++) {
            // 4 posiciones (N,S,E,O) por cada giro
            // posicion 0 del giro 0 => 0sg, posicion 1 del giro 0 => 0.05sg, posicion 1 del giro 1 => 0.2sg + 0.05sg, etc
            Invoke("Shoot", this.TimeForShoot * i);
        }

        Invoke(this.GetRevertPowerUpMethod(), TimeForRound * 4); // Revierto el powerUp al dar todos los giros
    }

    private void Shoot() {
        // Metodo que le pide al escudo que dispare
        this.GetAsimov().GetMyShield().ShieldControl(1); // El valor 1 es porque queremos disparar en sentido horario secuencialmente
        this.GetAsimov().GetMyShield().Shoot(); // Le pido al escudo que dispare
    }

    private void RevertYourMagic() {
        // Metodo que revierte el PowerUp
        // Desactivo la posibilidad de que siga disparando (no es necesario pero solo para asegurar)
        this.GetAsimov().GetMyShield().CanShoot = false;
    }
}
