using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ship : MonoBehaviour, IAttack
{
    #region "Atributos"
    private float HitPoints;
    private Vector2 Velocity;
    private float BulletDamage;
    private float MissileDamage;
    public float TimeBetweenShoots { get; set; }
    public float TimeBetweenMissileShoots { get; set; }
    public float RemainTimeForShootBullet { get; set; }
    public float RemainTimeForShootMissile { get; set; }
    public bool CanShoot { get; set; }
    //private float Reward;
    #endregion

    #region "Setters/Getters"
    public float GetHitPoints() {
        return this.HitPoints;
    }
    public void SetHitPoints(float value) {
        this.HitPoints = value;
    }

    public Vector2 GetVelocity() {
        return this.Velocity;
    }
    public void SetVelocity(Vector2 value) {
        this.Velocity = value;
    }

    public float GetBulletDamage() {
        return this.BulletDamage;
    }
    public void SetBulletDamage(float value) {
        this.BulletDamage = value;
    }

    public float GetMissileDamage() {
        return this.MissileDamage;
    }
    public void SetMissileDamage(float value) {
        this.MissileDamage = value;
    }

    public float GetTimeBetweenShoots() {
        return this.TimeBetweenShoots;
    }
    public void SetTimeBetweenShoots(float value) {
        this.TimeBetweenShoots = value;
    }

    public float GetTimeBetweenMissileShoots() {
        return this.TimeBetweenMissileShoots;
    }
    public void SetTimeBetweenMissileShoots(float value) {
        this.TimeBetweenMissileShoots = value;
    }
    #endregion

    #region "Constructor"
    public Ship(float hitpoints, Vector2 velocity, float bulletDamage, float missileDamage) {
        this.HitPoints = hitpoints;
        this.Velocity = velocity;
        this.BulletDamage = bulletDamage;
        this.MissileDamage = missileDamage;
    }
    #endregion

    #region "Metodos"
    public abstract void Move();
    public abstract void Shoot();
    #endregion
}
