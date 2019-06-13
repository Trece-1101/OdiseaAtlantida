//// Clase derivada/Hija de PowerUp. Un poder que regenera la vida del Player al maximo

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreHealthPU : PowerUp
{
    public override void MakeYourMagic() {
        // Metodo que controla la "magia" del PowerUp
        var restore = this.GetAsimov().GetOriginalHitPoints(); // Toma los puntos originales de la nave
        this.GetAsimov().RefillHealth(restore); // Recarga los puntos de vida
    }


}
