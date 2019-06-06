using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostPU : PowerUp
{
    private float SpeedBoost = 2f;

    public override void MakeYourMagic() {
        var vel = this.GetAsimov().GetVelocity();
        this.GetAsimov().SetVelocity(vel * SpeedBoost);
        Invoke("RevertYourMagic", this.GetCoolTime());
    }

    private void RevertYourMagic() {
        this.GetAsimov().SetVelocity(this.GetAsimov().GetOriginalVelocity());
    }
}
