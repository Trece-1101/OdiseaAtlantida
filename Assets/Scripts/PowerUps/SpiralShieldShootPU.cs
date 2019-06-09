using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralShieldShootPU : PowerUp
{
    private float TimeForShoot = 0.05f;
    private float TimeForRound;
    private float RotationLeft = 360;
    private float RotationSpeed = 10;
    private bool MakeRotate = false;

    public override void MakeYourMagic() {

        this.GetAsimov().SetGetCanRotate(false);

        this.MakeRotate = true;

        this.TimeForRound = this.TimeForShoot * 4;
        this.GetAsimov().GetMyShield().GetShieldOnFront();

        for (int j = 0; j < 4; j++) {
            Invoke("ShootRound", this.TimeForRound * j);
        }


        //Invoke("RevertYourMagic", this.GetCoolTime());
    }

    private void Update() {
        if (this.MakeRotate) {
            float rotation = RotationSpeed * Time.deltaTime;

            if(RotationLeft > 0) {
                RotationLeft -= rotation;
            }
            else {
                rotation = RotationLeft;
                RotationLeft = 0;
            }
            this.GetAsimov().transform.Rotate(0f, 0f, rotation);
        }
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
