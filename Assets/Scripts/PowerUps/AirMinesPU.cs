//// Clase derivada/Hija de PowerUp. Un poder que dispara 6 minas aeras hacia 6 areas distintas

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirMinesPU : PowerUp
{ 

    public override void MakeYourMagic() {
        // Metodo que controla la "magia" del PowerUp
        for (int i = 0; i < 6; i++) {
            // Un for de 6 iteraciones (0 a 5)
            this.ShootAirMines(i); // Disparo
        }
    }

    private void ShootAirMines(int i) {
        // Activo el objeto del pool necesario
        GameObject bomb = this.GetPool().Spawn("AirMine", this.GetAsimov().transform.position, Quaternion.identity);

        bomb.GetComponent<AirMine>().SetTarget(GetRandomTarget(i)); // Defino el area al que disparo el objeto AirMine que es un proyectil
        bomb.GetComponent<AirMine>().SetShooted(true); // Lo seteo como disparado para controlar un trigger

    }

    private Vector2 GetRandomTarget(int i) {
        Vector2 target = new Vector2(); // Vector posicion final (target)

        float rnd = Random.Range(-1f, 1f); // Variable para que el target no sea un punto sino un arear

        switch (i) {
            case 0:
                // esquina superior izq (area (-8/-6, 3/5))
                target = new Vector2(-7f + rnd, 4f + rnd);
                break;
            case 1:
                // centro (area (-1/1, -1/1))
                target = new Vector2(0f + rnd, 0f + rnd);
                break;
            case 2:
                // esquina superior derecha (area (8/6, 3/5))
                target = new Vector2(7f + rnd, 4f + rnd);
                break;
            case 3:
                // esquina inferior izq (area (-8/-6, -3/-5))
                target = new Vector2(-7f + rnd, -2f + rnd);
                break;
            case 4:
                // esquina inferior der (area (8/6, -3/-5))
                target = new Vector2(7f + rnd, -2f + rnd);
                break;
            case 5:
                // centro (area (-1/1, 3/5))
                target = new Vector2(0f + rnd, 4f + rnd);
                break;
            default:
                break;
        }

        // devuelvo la posicion target
        return target;
    }
}
