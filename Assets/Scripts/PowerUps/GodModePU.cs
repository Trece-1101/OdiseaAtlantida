using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodModePU : PowerUp
{
    private GameObject EffectGodMode;

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
        EffectGodMode = this.GetPool().Spawn("ShieldedAnimation", this.GetAsimov().transform.position, this.GetAsimov().transform.rotation);
        EffectGodMode.transform.parent = this.GetAsimov().transform;

        this.GetAsimov().SetIsVulnerable(false);
    }

    private void RevertYourMagic() {
        this.GetAsimov().SetIsVulnerable(true);
    }
}
