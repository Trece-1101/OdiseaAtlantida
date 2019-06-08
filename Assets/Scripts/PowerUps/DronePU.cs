using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronePU : PowerUp
{
    private GameObject drone1;
    private GameObject drone2;

    public override void MakeYourMagic() {  
        drone1 = this.GetPool().Spawn("Drone", this.GetAsimov().transform.position, this.GetAsimov().transform.rotation);
        drone2 = this.GetPool().Spawn("Drone", this.GetAsimov().transform.position, this.GetAsimov().transform.rotation);

        drone1.transform.parent = this.GetAsimov().transform;
        drone2.transform.parent = this.GetAsimov().transform;

        drone1.transform.localPosition = new Vector3(1.5f, 0f, 0f);
        drone2.transform.localPosition = new Vector3(-1.5f, 0f, 0f);

        Invoke("RevertYourMagic", this.GetCoolTime());
    }

    private void RevertYourMagic() {
        drone1.SetActive(false);
        drone2.SetActive(false);
    }
}
