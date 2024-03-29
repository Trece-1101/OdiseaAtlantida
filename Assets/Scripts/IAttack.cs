﻿using UnityEngine;

public interface IAttack
{
    float TimeBetweenShoots { get; set; }
    float TimeBetweenMissileShoots { get; set; }
    float RemainTimeForShootBullet { get; set; }
    float RemainTimeForShootMissile { get; set; }
    bool CanShoot { get; set; }

    void Shoot();
}
