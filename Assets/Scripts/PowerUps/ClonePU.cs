using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClonePU : PowerUp
{
    private GameObject Clone;

    public override void MakeYourMagic() {
        Clone = this.GetPool().Spawn("AsimovClone", new Vector3(0f, -3f, 0f), Quaternion.identity);
        this.GetAsimov().SetIsCloned(true);
        Invoke("RevertYourMagic", this.GetCoolTime());
    }

    private void RevertYourMagic() {
        Clone.SetActive(false);
        this.GetAsimov().SetIsCloned(false);
    }
}
