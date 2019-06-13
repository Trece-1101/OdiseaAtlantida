//// Clase derivada/Hija de PowerUp. Un poder que activa dos drones a los costados de la nave que disparan constantemente

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronePU : PowerUp
{
    private GameObject drone1; // Referencia a los drones
    private GameObject drone2;

    public override void MakeYourMagic() {
        // Metodo que controla la "magia" del PowerUp

        // Le pido al pool activar las instancias de los drones
        drone1 = this.GetPool().Spawn("Drone", this.GetAsimov().transform.position, this.GetAsimov().transform.rotation);
        drone2 = this.GetPool().Spawn("Drone", this.GetAsimov().transform.position, this.GetAsimov().transform.rotation);


        // Los drones son hijos (en la jerarquia) de la nave del player
        // De manera tal que roten y se muevan con ella
        drone1.transform.parent = this.GetAsimov().transform;
        drone2.transform.parent = this.GetAsimov().transform;

        // Muevo los drones a los costados de la nave de manera local (0f, 0f, 0f es el centro de la nave)
        drone1.transform.localPosition = new Vector3(1.5f, 0f, 0f);
        drone2.transform.localPosition = new Vector3(-1.5f, 0f, 0f);

        Invoke("RevertYourMagic", this.GetCoolTime()); // Revierto el powerUp en CoolTime segundos
    }

    private void RevertYourMagic() {
        // Metodo que revierte el PowerUp
        // Desactivo los drones
        drone1.SetActive(false);
        drone2.SetActive(false);
    }
}
