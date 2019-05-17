using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeBandit : Ship
{
    private ObjectPool objectPool;
    [SerializeField] private Transform shootPoint;

    public OrangeBandit(float hitpoints, Vector2 velocity, float bulletDamage, float missileDamage):base(hitpoints, velocity, bulletDamage, missileDamage) {
        this.SetHitPoints(100f);
        this.SetVelocity(new Vector2(2f, 2f));
        this.SetBulletDamage(20f);
        this.SetMissileDamage(0f);
    }

    private void Start() {
        this.objectPool = ObjectPool.Instance;
        this.CanShoot = false;

        this.TimeBetweenShoots = 0.5f;
        this.RemainTimeForShootBullet = this.TimeBetweenShoots;
    }

    private void Update() {
        Shoot();
    }

    public override void Move() {
        throw new System.NotImplementedException();
    }

    public override void Shoot() {
        if (this.RemainTimeForShootBullet <= 0 && this.CanShoot) {
            objectPool.Spawn("EnemyBullet", shootPoint.position, Quaternion.Euler(0f, 0f, -90f));

            this.RemainTimeForShootBullet = this.TimeBetweenShoots;
        }
        else {
            this.RemainTimeForShootBullet -= Time.deltaTime;
        }
    }

    public override void CheckRotation() {
        throw new System.NotImplementedException();
    }
}
