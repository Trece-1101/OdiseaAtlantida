using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTimePU : PowerUp
{
    private float BulletTimeFactor = 0.25f;
    private int PlayerFactor = 4;

    public override void MakeYourMagic() {
        Debug.Log($"Asimov Speed {this.GetAsimov().GetVelocity()}");
        Debug.Log($"Asimov TimeBetweenBullet {this.GetAsimov().TimeBetweenBulletShoots}");
        Debug.Log($"Asimov TimeBetweenMissile {this.GetAsimov().TimeBetweenMissileShoots}");
        Time.timeScale = BulletTimeFactor;
        this.GetAsimov().SetVelocity(this.GetAsimov().GetVelocity() * PlayerFactor);
        this.GetAsimov().TimeBetweenBulletShoots /= PlayerFactor;
        this.GetAsimov().TimeBetweenMissileShoots /= PlayerFactor;
        Invoke("RevertYourMagic", this.GetCoolTime() / PlayerFactor);
    }

    private void RevertYourMagic() {
        this.GetAsimov().SetVelocity(this.GetAsimov().GetVelocity() / PlayerFactor);
        this.GetAsimov().TimeBetweenBulletShoots *= PlayerFactor;
        this.GetAsimov().TimeBetweenMissileShoots *= PlayerFactor;
        Time.timeScale = 1f;
        Debug.Log($"Asimov Speed {this.GetAsimov().GetVelocity()}");
        Debug.Log($"Asimov Speed {this.GetAsimov().TimeBetweenBulletShoots}");
        Debug.Log($"Asimov TimeBetweenMissile {this.GetAsimov().TimeBetweenMissileShoots}");
    }

    
}
