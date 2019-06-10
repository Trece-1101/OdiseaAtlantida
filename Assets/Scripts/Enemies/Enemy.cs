using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Ship
{
    #region "Atributos Serializados"
    [Header("Especificos")]
    [SerializeField] private int Reward;
    [SerializeField] [Range (0f, 1f)] private float PowerUpChance = 0f;
    #endregion        

    #region "Referencias en Cache"
    private Asimov Player;    
    #endregion

    #region "Auxiliares"
    [SerializeField] private float timeBtwBulletS = 1f;
    [SerializeField] private float timeBtwMissileS = 1f;
    [SerializeField] private bool canShootMissile = false;
    List<string> Explodes = new List<string>() { "EnemyExplosion", "EnemyImplosion" };
    #endregion

    #region "Setters/Getters"
    public Asimov GetPlayer() {
        return this.Player;
    }
    public void SetPlayer(Asimov value) {
        this.Player = value;
    }

    public int GetReward() {
        return this.Reward;
    }
    public void SetReward(int value) {
        this.Reward = value;
    }
    #endregion

    #region "Metodos"
    public override void CoAwake() {
        this.SetIsAlive(true);
        this.SetIsVulnerable(true);

        this.Player = FindObjectOfType<Asimov>();
 
        this.SetTimeBetweenBulletShoots(timeBtwBulletS);
        this.SetTimeBetweenMissileShoots(timeBtwMissileS);
        this.RemainTimeForShootBullet = this.GetTimeBetweenBulletShoots();
        this.RemainTimeForShootMissile = this.GetTimeBetweenMissileShoots();
        this.CanShootMissile = canShootMissile;
        this.SetMyBulletVFX("EnemyBullet");
        this.SetMyMissileVFX("EnemyMissile");
        this.GetGameProg().AddEnemyToCount();
    }       

    private void Update() {
        CheckRotation();
        Shoot();
    }

    //public override void Move() {
    //    transform.Translate(new Vector3(0.0001f, 0.0001f, 0f));
    //}

    public override void CheckRotation() {
        // vector_direccion_ataque = vector_posicion_player - vector_posicion_enemigo
        // 90 grados para compensar que el sprite tiene su 0° hacia el Norte y la camara tiene sus 0° hacia el Este
        // el sprite del proyectil ya apunta hacia el Este asi que no tiene compensacion

        Dictionary<string, Quaternion> rotations;

        if (!this.Player.GetIsAlive()) {
            return;
        }

        if (this.Player.GetIsCloned()) {
            rotations = this.Rotate(new Vector3(0f, -3f, 0f) - this.transform.position, 90, 0);
        }
        else {
            rotations = this.Rotate(this.Player.transform.position - this.transform.position, 90, 0);
        }

        this.SetMyRotation(rotations["rotation_ship"]);
        this.SetMyBulletRotation(rotations["rotation_bullet"]);
        this.transform.rotation = rotations["transform_rotation"];
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

        if (this.RemainTimeForShootMissile <= 0 && this.CanShootMissile && this.GetMissileShootPoints().Count > 0) {
            ShootMissile();
            PlayShootSFX(this.GetShootMissileSFX(), this.transform.position, 3f);


            this.RemainTimeForShootMissile = this.GetTimeBetweenMissileShoots();
        }
        else {
            this.RemainTimeForShootMissile -= Time.deltaTime;
        }

    }

    public override void ControlOtherCollision(Collider2D collision) {
        if(collision.gameObject.tag == "Player") {
            Die();
        }
    }

    private void PlayExplosion() {
        int expIndex = Random.Range(0, 2);
        string exp = Explodes[expIndex];
        this.GetPool().Spawn(exp, this.transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(this.GetDeathSFX(), this.GetMyMainCamera().transform.position, 0.6f);
    }

    public override void PlayImpactSFX() {
        this.GetPool().Spawn("ProyectileExplosion", this.transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(this.GetHurtSFX(), this.transform.position, 3f);
    }

    public override void Die() {
        Destroy(gameObject);
                      
        PlayExplosion();        
    }

    private void OnDestroy() {
        PowerUpRoulette();
        this.GetGameProg().AddScore(this.GetReward());
    }

    private void PowerUpRoulette() {
        var powerUp = PickRandomPowerUp();
        var number = Random.Range(0f, 1f);

        if (powerUp == null) { return; }
        if(number <= this.PowerUpChance) {
            this.GetPool().Spawn(powerUp, this.transform.position, Quaternion.identity);
        }
        
    }

    private string PickRandomPowerUp() {
        if(!(GetPowerUpsNames().Count > 0)) { return null; }
            int powerUpIndex = Random.Range(0, this.GetPowerUpsNames().Count);
            return this.GetPowerUpsNames()[powerUpIndex];
    }

    #endregion


}
