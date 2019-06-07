﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : Enemy
{

    public override void CoAwake() {
        base.CoAwake();
        this.SetMyBulletVFX("TankBullet");
    }

    public override void Shoot() {
        if (this.RemainTimeForShootBullet <= 0 && this.CanShoot) {
            ShootBullet();
            PlayShootSFX(this.GetShootBulletSFX(), this.transform.position, 3f);

            this.RemainTimeForShootBullet = this.TimeBetweenBulletShoots;
        }
        else {
            this.RemainTimeForShootBullet -= Time.deltaTime;
        }
    }

    public override void ShootBullet() {
        for (int i = 0; i < this.GetBulletShootPoints().Count; i++) {
            this.GetPool().Spawn(this.GetMyBulletVFX(), this.GetBulletShootPoints()[i].position, this.GetMyBulletRotation());
        }
    }
}