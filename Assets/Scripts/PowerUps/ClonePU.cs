//// Clase derivada/Hija de PowerUp. Un poder que crea un clon de la nave al cual apuntan los enemigos

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClonePU : PowerUp
{
    private GameObject Clone; // Referencia al objeto clon
    private Vector3 ClonePosition = new Vector3(0f, -3f, 0f);

    public override void MakeYourMagic() {
        // Metodo que controla la "magia" del PowerUp

        // Obtengo del pool la instancia del clone, la activo y lo posiciono
        Clone = this.GetPool().Spawn("AsimovClone", this.ClonePosition, Quaternion.identity);
        this.GetAsimov().SetIsCloned(true); // Le digo al player que esta clonado, esto afectara a los enemigos
        Invoke(this.GetRevertPowerUpMethod(), this.GetCoolTime()); // Revierto el powerUp en CoolTime segundos
    }

    private void RevertYourMagic() {
        // Metodo que revierte el PowerUp
        Clone.SetActive(false); // Desactivo al clon
        this.GetAsimov().SetIsCloned(false); // Le digo a la nave que ya no esta clonada
    }
}
