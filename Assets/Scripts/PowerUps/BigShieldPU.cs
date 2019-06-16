//// Clase derivada/Hija de PowerUp. Un poder que aumenta el tamaño del escudo

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigShieldPU : PowerUp
{
    public override void MakeYourMagic() {
        // Metodo que controla la "magia" del PowerUp
        // Le pide al escudo de la nave del player que ejecute su metodo de BigShield
        this.GetAsimov().GetMyShield().BigShield();
        Invoke(this.GetRevertPowerUpMethod(), this.GetCoolTime()); // Revierte este proceso en CoolTime segundos
    }

    private void RevertYourMagic() {
        // Le pide al escudo de la nave del player que ejecute el metodo de volver a valores normales
        this.GetAsimov().GetMyShield().NormalShield();
    }
}
