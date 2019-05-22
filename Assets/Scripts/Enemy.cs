using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Ship
{
    [SerializeField] private float hitpoints;
    [SerializeField] private Vector2 velocity;
    [SerializeField] private float timeBtwShoots;
    [SerializeField] private AudioClip deathSFX;
    [SerializeField] private AudioClip shootSFX;
    [SerializeField] private AudioClip hurtSFX;


    private ObjectPool objectPool;
    [SerializeField] private Transform shootPoint;
    private Asimov player;
    
    

    #region "Aux"   
    private Quaternion rotation_enemy;
    private Quaternion rotation_bullet;
    #endregion

    public Enemy(float hitpoints, Vector2 velocity) : base(hitpoints, velocity) {
        this.SetHitPoints(this.hitpoints);
        this.SetVelocity(this.velocity);
        
    }

    private void Awake() {
        this.TimeBetweenShoots = timeBtwShoots;
        this.RemainTimeForShootBullet = this.TimeBetweenShoots;
        this.SetHitPoints(this.hitpoints);
    }

    private void Start() {
        this.objectPool = ObjectPool.Instance;
        this.player = FindObjectOfType<Asimov>();
        this.CanShoot = true;

        
    }

    private void Update() {
        //Move();
        CheckRotation();
        Shoot();
    }

    public override void Move() {
        transform.Translate(new Vector3(0.0001f, 0.0001f, 0f));
    }      
    
    public override void CheckRotation() {
        // vector_direccion_ataque = vector_posicion_player - vector_posicion_enemigo
        // 90 grados para compensar que el sprite tiene su 0° hacia el Norte y la camara tiene sus 0° hacia el Este
        // el sprite del proyectil ya apunta hacia el Este asi que no tiene compensacion
        Dictionary<string, Quaternion> rotations;
        if (!player.GetIsAlive()) {
            return;
        }
        rotations = this.Rotate(player.transform.position - this.transform.position, 90, 0);
        
        rotation_enemy = rotations["rotation_ship"];
        rotation_bullet = rotations["rotation_bullet"];
        this.transform.rotation = rotations["transform_rotation"];
    }

    public override void Shoot() {
        if (this.RemainTimeForShootBullet <= 0 && this.CanShoot) {
            objectPool.Spawn("EnemyBullet", shootPoint.position, rotation_bullet);
            AudioSource.PlayClipAtPoint(shootSFX, this.transform.position, 3f);

            this.RemainTimeForShootBullet = this.TimeBetweenShoots;
        }
        else {
            this.RemainTimeForShootBullet -= Time.deltaTime;
        }
    }
       
    
    public override void Die() {
        Destroy(gameObject);
        PlayExplosion();        
    }

    private void PlayExplosion() {
        objectPool.Spawn("EnemyExplosion", this.transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, 0.6f);
    }

    public override void PlayImpact() {
        objectPool.Spawn("ProyectileExplosion", this.transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(hurtSFX, this.transform.position, 3f);
    }

  

}
