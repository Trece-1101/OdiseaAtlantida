using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asimov : Ship
{
    #region "Atributos"
    private float dashCoolTime;
    private float initialDashCoolTime;
    private float dashSpeed;
    private float dashDistance;
    private float dashStep;
    private bool canDash;
    private bool IsAlive;
    private bool godMode;
    #endregion      

    #region "Componentes"
    private Rigidbody2D body;
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private Transform bulletShootPoint1;
    [SerializeField] private Transform bulletShootPoint2;
    [SerializeField] private Transform missileShootPoint1;
    [SerializeField] private Transform missileShootPoint2;
    [SerializeField] private AudioClip deathSFX;
    [SerializeField] private AudioClip shootBulletSFX;
    [SerializeField] private AudioClip shootMissileSFX;
    [SerializeField] private AudioClip hurtSFX;
    private SpriteRenderer img;
    private Sprite actualSprite;
    private ObjectPool objectPool;
    private Shield shield;
    private PolygonCollider2D colliderAtack;
    private PolygonCollider2D colliderDefense;
    private Dictionary<string, List<Vector3>> thrustPosition;
    #endregion

    #region "Referencias en Cache"
    private GameProgram gameProgram;
    #endregion

    #region "Aux"   
    private Quaternion rotation_asimov;
    private Quaternion rotation_bullet;
    #endregion

    #region "Setters/Getters"
    public bool GetIsAlive() {
        return this.IsAlive;
    }
    public void SetIsAlive(bool value) {
        this.IsAlive = value;
    }
    #endregion

    #region "Constructor"
    public Asimov(float hitpoints, Vector2 velocity):
                  base(hitpoints, velocity){     
    }
    #endregion

    #region "Metodos"

    private void Awake() {
        this.SetIsAlive(true);
    }

    private void Start() {
        this.dashDistance = 10f;
        this.dashStep = 0.5f;
        this.godMode = false;
        this.RemainTimeForShootBullet = this.GetTimeBetweenShoots();
        this.RemainTimeForShootMissile = this.GetTimeBetweenMissileShoots();
        this.objectPool = ObjectPool.Instance;
        this.shield = FindObjectOfType<Shield>();
        this.gameProgram = FindObjectOfType<GameProgram>();      


        body = GetComponent<Rigidbody2D>();
        img = GetComponent<SpriteRenderer>();
        PolygonCollider2D[] colliders = GetComponents<PolygonCollider2D>();
        colliderAtack = colliders[0];
        colliderDefense = colliders[1];

        actualSprite = sprites[0];
        img.sprite = actualSprite;

        initialDashCoolTime = 2f;
        dashSpeed = 3f;
        dashCoolTime = initialDashCoolTime;
    }


    private void Update() {
        Move();
        Dash();
        CheckRotation();
        MoveShield();
        CheckSprite();
        Shoot();
    }



    private void CheckSprite() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (actualSprite == sprites[0]) {
                actualSprite = sprites[1];
                colliderAtack.enabled = false;
                colliderDefense.enabled = true;
            }
            else {
                actualSprite = sprites[0];
                colliderAtack.enabled = true;
                colliderDefense.enabled = false;
            }

            img.sprite = actualSprite;

        }
    }    



    public override void Move() {
        var deltaX = Input.GetAxis("Horizontal") * GetVelocity().x * Time.deltaTime;
        var deltaY = Input.GetAxis("Vertical") * GetVelocity().y * Time.deltaTime;

        var nextPosX = Mathf.Clamp(transform.position.x + deltaX, gameProgram.GetLeftBorder(), gameProgram.GetRightBorder());
        var nextPosY = Mathf.Clamp(transform.position.y + deltaY, gameProgram.GetUpBorder(), gameProgram.GetDownBorder());

        // pos(n) = pos(n-1) + v*t
        transform.position = new Vector2(nextPosX, nextPosY);
    }

    public override void CheckRotation() {
        // vector_direccion_ataque = vector_posicion_mouse - vector_centro_camara
        // 90 grados para compensar que el sprite tiene su 0° hacia el Norte y la camara tiene sus 0° hacia el Este
        // el sprite del proyectil ya apunta hacia el Este asi que no tiene compensacion

        var rotations = this.Rotate(Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position), 90, 0);
        rotation_asimov = rotations["rotation_ship"];
        rotation_bullet = rotations["rotation_bullet"];
        this.transform.rotation = rotations["transform_rotation"];
    }

    private void MoveShield() {
        if(Input.GetAxis("Mouse ScrollWheel") != 0f) {
            shield.ShieldControl(Convert.ToInt32(Input.GetAxis("Mouse ScrollWheel") * 10));
        }

        if(Input.GetMouseButtonDown(2)) {
            shield.GetShieldOnFront();
        }
        
    }

    private void Dash() {
        dashCoolTime -= Time.deltaTime;
        if(dashCoolTime <= 0) {
            canDash = true;
        }
        else {
            canDash = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash) {
            if (Input.GetKey(KeyCode.A)) {
                //Debug.Log("Dash IZQ");
                this.transform.position = new Vector3(Mathf.Lerp(this.transform.position.x, this.transform.position.x - dashDistance, dashStep), 
                                                        this.transform.position.y, 
                                                        this.transform.position.z);
            }
            else if (Input.GetKey(KeyCode.D)) {
                //Debug.Log("Dash DER");
                this.transform.position = new Vector3(Mathf.Lerp(this.transform.position.x, this.transform.position.x + dashDistance, dashStep),
                                                        this.transform.position.y,
                                                        this.transform.position.z);
            }

            dashCoolTime = initialDashCoolTime;
        }
        //Debug.Log(canDash);
    }
    
    public override void Shoot() {               
        if(this.RemainTimeForShootBullet <= 0) {
            if (Input.GetButton("Fire1")) {
                //Instantiate(bulletPrefab, bulletShootPoint1.position, rotation_bullet);
                //Instantiate(bulletPrefab, bulletShootPoint2.position, rotation_bullet);

                objectPool.Spawn("PlayerBullet", bulletShootPoint1.position, rotation_bullet);
                objectPool.Spawn("PlayerBullet", bulletShootPoint2.position, rotation_bullet);
                PlayShootSound(shootBulletSFX);


                this.RemainTimeForShootBullet = this.GetTimeBetweenShoots();
            }            
        }
        else {
            this.RemainTimeForShootBullet -= Time.deltaTime;
        }

        if (this.RemainTimeForShootMissile <= 0) {
            if (Input.GetButton("Fire2")) {
                //Instantiate(bulletPrefab, bulletShootPoint1.position, rotation_bullet);
                //Instantiate(bulletPrefab, bulletShootPoint2.position, rotation_bullet);

                objectPool.Spawn("PlayerMissile", missileShootPoint1.position, rotation_bullet);
                objectPool.Spawn("PlayerMissile", missileShootPoint2.position, rotation_bullet);
                PlayShootSound(shootMissileSFX);


                this.RemainTimeForShootMissile = this.GetTimeBetweenMissileShoots();
            }
        }
        else {
            this.RemainTimeForShootMissile -= Time.deltaTime;
        }
    }

    private void PlayShootSound(AudioClip sound) {
        AudioSource.PlayClipAtPoint(sound, Camera.main.transform.position, 0.2f);
    }
     
    public override void Die() {
        this.SetIsAlive(false);
        Destroy(gameObject);
        PlayExplosion();
        FindObjectOfType<LevelManager>().LoadGameOver();
    }

    private void PlayExplosion() {
        objectPool.Spawn("EnemyExplosion", this.transform.position + new Vector3(0.3f, 0.3f, 0f), Quaternion.identity);
        objectPool.Spawn("EnemyExplosion", this.transform.position + new Vector3(-0.3f, 0.3f, 0f), Quaternion.identity);
        objectPool.Spawn("EnemyExplosion", this.transform.position + new Vector3(0.3f, -0.3f, 0f), Quaternion.identity);
        objectPool.Spawn("EnemyExplosion", this.transform.position + new Vector3(-0.3f, -0.3f, 0f), Quaternion.identity);

        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, 0.6f);
    }

    public override void PlayImpact() {
        objectPool.Spawn("ProyectileExplosion", this.transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(hurtSFX, Camera.main.transform.position, 0.2f);
    }


    #endregion
}
