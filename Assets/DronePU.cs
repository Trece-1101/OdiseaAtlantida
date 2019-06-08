using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronePU : PowerUp
{
    public override void MakeYourMagic() {
        this.GetAsimov().GiveMeMyDrones();
        Invoke("RevertYourMagic", this.GetCoolTime());
    }

    private void RevertYourMagic() {
        this.GetAsimov().DestroyMyDrones();
    }
}
