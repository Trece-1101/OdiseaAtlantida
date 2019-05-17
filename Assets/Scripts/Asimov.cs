using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asimov : Ship
{
    #region "Atributos"
    #endregion      

    #region "Componentes"
    private Rigidbody2D body;
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private Transform bulletShootPoint1;
    [SerializeField] private Transform bulletShootPoint2;
    [SerializeField] private Transform missileShootPoint1;
    [SerializeField] private Transform missileShootPoint2;
    private SpriteRenderer img;
    private Sprite actualSprite;
    private ObjectPool objectPool;
    private Shield shield;
    private PolygonCollider2D colliderAtack;
    private PolygonCollider2D colliderDefense;
    #endregion

    #region "Referencias en Cache"
    private GameProgram gameProgram;
    #endregion

    #region "Aux"   
    private Quaternion rotation_asimov;
    private Quaternion rotation_bullet;
    #endregion

    #region "Setters/Getters"    
    #endregion

    #region "Constructor"
    public Asimov(float hitpoints, Vector2 velocity, float bulletDamage, float missileDamage, float shield):
                  base(hitpoints, velocity, bulletDamage, missileDamage){     
    }
    #endregion

    #region "Metodos"
   
    private void Start() {        
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
    }


    private void Update() {
        Move();
        CheckRotation();
        MoveShield();
        CheckSprite();
        Shoot();
        ShieldShoot();
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

    private float AngleWithCompensateRotation(Vector3 direction, int compensation) {
        var angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - compensation;
        return angle;
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
        
    }
    
    public override void Shoot() {               
        if(this.RemainTimeForShootBullet <= 0) {
            if (Input.GetButton("Fire1")) {
                //Instantiate(bulletPrefab, bulletShootPoint1.position, rotation_bullet);
                //Instantiate(bulletPrefab, bulletShootPoint2.position, rotation_bullet);

                objectPool.Spawn("PlayerBullet", bulletShootPoint1.position, rotation_bullet);
                objectPool.Spawn("PlayerBullet", bulletShootPoint2.position, rotation_bullet);


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


                this.RemainTimeForShootMissile = this.GetTimeBetweenMissileShoots();
            }
        }
        else {
            this.RemainTimeForShootMissile -= Time.deltaTime;
        }
    }

    private void ShieldShoot() {
        this.shield.Shoot(rotation_bullet);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        DamageHandler damageHandler = collision.gameObject.GetComponent<DamageHandler>();
        if (!damageHandler) {
            return;
        }
        ReceiveDamage(damageHandler.GetDamage());
        collision.gameObject.SetActive(false);
    }

    public override void Die() {
        Destroy(gameObject);
    }



    #endregion
}
