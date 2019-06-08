using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigShieldPU : PowerUp
{
    public override void MakeYourMagic() {
        this.GetAsimov().GetMyShield().BigShield();
        Invoke("RevertYourMagic", this.GetCoolTime());
    }

    private void RevertYourMagic() {
        this.GetAsimov().GetMyShield().NormalShield();
    }
}
