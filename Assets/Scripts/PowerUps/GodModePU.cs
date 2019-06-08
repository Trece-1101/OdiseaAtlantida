using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodModePU : PowerUp
{
    public override void MakeYourMagic() {
        this.SetInVulnerable();
        Invoke("RevertYourMagic", this.GetCoolTime());
    }

    public void MakeYourMagic(bool godMode) {
        if (godMode) {
            this.SetInVulnerable();
        }
        else {
            this.MakeYourMagic();
        }
    }

    private void SetInVulnerable() {
        this.GetAsimov().SetIsVulnerable(false);
    }

    private void RevertYourMagic() {
        this.GetAsimov().SetIsVulnerable(true);
    }
}
