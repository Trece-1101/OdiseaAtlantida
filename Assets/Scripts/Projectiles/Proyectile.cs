using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Proyectile : MonoBehaviour
{
    #region "Atributos Serializados"
    [SerializeField] private Vector2 Speed;
    private Vector2 InitialSpeed;
    private float Damage;
    #endregion

    #region "Atributos"
    private float LifeTime;
    #endregion

    #region "Referencias en Cache"
    private DamageControl DamageCtrl;
    private Enemy Shooter;
    #endregion

    #region "Setters/Getters"
    public Vector2 GetSpeed() {
        return this.Speed;
    }
    public void SetSpeed(Vector2 value) {
        this.Speed = value;
    }

    public float GetLifeTime() {
        return this.LifeTime;
    }
    public void SetLifeTime(float value) {
        this.LifeTime = value;
    }

    public float GetDamage() {
        return this.Damage;
    }
    public void SetDamage(float value) {
        this.Damage = value;
    }

    public Vector2 GetInitalSpeed() {
        return this.InitialSpeed;
    }
    public void SetInitialSpeed(Vector2 value) {
        this.InitialSpeed = value;
    }

    public DamageControl GetDamageCtrl() {
        return this.DamageCtrl;
    }
    public void SetDamageCtrl(DamageControl value) {
        this.DamageCtrl = value;
    }

    public Enemy GetShooter() {
        return this.Shooter;
    }
    public void SetShooter(Enemy value) {
        this.Shooter = value;
    }

    #endregion

    #region "Metodos"
    private void Awake() {
        this.DamageCtrl = GetComponent<DamageControl>();
        //this.DamageCtrl.SetDamage(this.Damage);

        this.LifeTime = 5f;
    }

    public abstract void Update();
    
    private void OnDisable() {
        CancelInvoke();
    }

    public virtual void Die() {        
        gameObject.SetActive(false);
        this.Speed = this.InitialSpeed;        
    }
    #endregion



}
