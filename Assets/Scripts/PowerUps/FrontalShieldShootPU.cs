//// Clase derivada/Hija de PowerUp. Un poder que dispara balas desde el escudo siempre en direccion frontal

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontalShieldShootPU : PowerUp
{

    private float TimeForShoot = 0.15f; // Tiempo entre disparos

    public override void MakeYourMagic() {
        // Metodo que controla la "magia" del PowerUp
        for (int i = 0; i < 25; i++) {            
            Invoke("Shoot", this.TimeForShoot * i); // Disparo 24 veces, incremento el tiempo del Invoke proporcionalmente de manera tal que el deltaTiempo es el mismo
        }
        
        Invoke("RevertYourMagic", this.GetCoolTime()); // Revierto el powerUp en CoolTime segundos
    }

    private void Shoot() {
        // Le pido al escudo de la nave que dispare
        this.GetAsimov().GetMyShield().Shoot();
    }

    private void RevertYourMagic() {
        // Metodo que revierte el PowerUp
        // Desactivo la posibilidad de que siga disparando (no es necesario pero solo para asegurar)
        this.GetAsimov().GetMyShield().CanShoot = false;
    }
}
