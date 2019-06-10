using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreHealthPU : PowerUp
{
    public override void MakeYourMagic() {
        var restore = this.GetAsimov().GetOriginalHitPoints();
        this.GetAsimov().RefillHealth(restore);
    }


}
