//// Clase derivada/Hija de PowerUp. Un poder que aumenta la velocidad de la nave un 75%

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostPU : PowerUp
{
    private float SpeedBoost = 1.75f; // Modificacion de velocidad

    public override void MakeYourMagic() {
        // Metodo que controla la "magia" del PowerUp
        var vel = this.GetAsimov().GetVelocity(); // Tomo la velocidad actual del Player
        this.GetAsimov().SetVelocity(vel * SpeedBoost); // La multiplico por el SpeedBoost
        Invoke("RevertYourMagic", this.GetCoolTime());  // Revierto el powerUp en CoolTime segundos
    }

    private void RevertYourMagic() {
        // Devuelvo la velocidad a su original
        this.GetAsimov().SetVelocity(this.GetAsimov().GetOriginalVelocity());
    }
}
