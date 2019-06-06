using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreHealthPU : PowerUp
{
    public override void MakeYourMagic() {
        var restore = this.GetAsimov().GetStartHealth();
        this.GetAsimov().RefillHealth(restore);
    }


}
