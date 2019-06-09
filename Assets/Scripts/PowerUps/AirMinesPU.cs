using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirMinesPU : PowerUp
{ 

    public override void MakeYourMagic() {
        for (int i = 0; i < 6; i++) {
            //this.GetAsimov().ShootAirMines();
            this.ShootAirMines(i);
        }
    }

    private void ShootAirMines(int i) {
        GameObject bomb = this.GetPool().Spawn("AirMine", this.GetAsimov().transform.position, Quaternion.identity);

        bomb.GetComponent<AirMine>().SetTarget(GetRandomTarget(i));
        bomb.GetComponent<AirMine>().SetShooted(true);

    }

    private Vector2 GetRandomTarget(int i) {
        Vector2 target = new Vector2();
        float rnd = Random.Range(-1f, 1f);
        switch (i) {
            case 0:
                // esquina superior izq
                target = new Vector2(-7f + rnd, 4f + rnd);
                break;
            case 1:
                // centro
                target = new Vector2(0f + rnd, 0f + rnd);
                break;
            case 2:
                // esquina superior derecha
                target = new Vector2(7f + rnd, 4f + rnd);
                break;
            case 3:
                // esquina inferior izq
                target = new Vector2(-7f + rnd, -2f + rnd);
                break;
            case 4:
                // esquina inferior der
                target = new Vector2(7f + rnd, -2f + rnd);
                break;
            case 5:
                // centro
                target = new Vector2(0f + rnd, 4f + rnd);
                break;
            default:
                break;
        }

        return target;
    }
}
