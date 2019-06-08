using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontalShieldShootPU : PowerUp
{

    private float TimeForShoot = 0.15f;

    public override void MakeYourMagic() {
        for (int i = 0; i < 25; i++) {
            Invoke("Shoot", this.TimeForShoot * i);
        }
        
        Invoke("RevertYourMagic", this.GetCoolTime());
    }

    private void Shoot() {
        this.GetAsimov().MakeShieldShoot();
    }

    private void RevertYourMagic() {
        this.GetAsimov().GetMyShield().CanShoot = false;
    }
}
