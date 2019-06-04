using UnityEngine;

public interface IAttack
{
    float TimeBetweenBulletShoots { get; set; }
    float TimeBetweenMissileShoots { get; set; }
    float RemainTimeForShootBullet { get; set; }
    float RemainTimeForShootMissile { get; set; }
    bool CanShoot { get; set; }
    bool CanShootMissile { get; set; }
    DamageControl DamageCtrl { get; set; }

    void Shoot();
}
