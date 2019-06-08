﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralShieldShootPU : PowerUp
{
    private float TimeForShoot = 0.05f;
    private float TimeForRound;

    public override void MakeYourMagic() {
        this.TimeForRound = this.TimeForShoot * 4;
        this.GetAsimov().GetMyShield().GetShieldOnFront();

        for (int j = 0; j < 4; j++) {
            Invoke("ShootRound", this.TimeForRound * j);
        }


        //Invoke("RevertYourMagic", this.GetCoolTime());
    }

    private void ShootRound() {
        for (int i = 0; i < 4; i++) {
            Invoke("Shoot", this.TimeForShoot * i);
        }
        Invoke("RevertYourMagic", TimeForRound * 4);
    }

    private void Shoot() {
        this.GetAsimov().GetMyShield().ShieldControl(1);
        this.GetAsimov().MakeShieldShoot();
    }

    private void RevertYourMagic() {
        this.GetAsimov().GetMyShield().CanShoot = false;
    }
}