using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asimov : Ship
{
    #region "Atributos"
    private float Shield;
    #endregion      

    #region "Componentes"
    private Rigidbody2D body;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private Transform bulletShootPoint1;
    [SerializeField] private Transform bulletShootPoint2;
    private SpriteRenderer img;
    private Sprite actualSprite;
    private ObjectPool objectPool;
    private Shield shield;
    #endregion

    #region "Aux"
    private float leftBorder;
    private float rightBorder;
    private float upBorder;
    private float downBorder;
    private float padding;
    private float timeForShoot;
    private Quaternion rotation_asimov;
    private Quaternion rotation_bullet;
    #endregion

    #region "Setters/Getters"
    public float GetShield() {
        return this.Shield;
    }
    public void SetShield(float value) {
        this.Shield = value;
    }
    #endregion

    #region "Constructor"
    public Asimov(float hitpoints, Vector2 velocity, float bulletDamage, float missileDamage, float shield):
                  base(hitpoints, velocity, bulletDamage, missileDamage){
        this.Shield = shield;        
    }
    #endregion

    #region "Metodos"
   
    private void Start() {
        this.SetTimeBetweenShoots(0.2f);
        this.timeForShoot = this.GetTimeBetweenShoots();
        this.objectPool = ObjectPool.Instance;
        this.shield = FindObjectOfType<Shield>();

        body = GetComponent<Rigidbody2D>();
        img = GetComponent<SpriteRenderer>();
        actualSprite = sprites[0];
        img.sprite = actualSprite;
        SetUpBorders();
    }


    private void Update() {
        Move();
        Rotate();
        MoveShield();
        CheckSprite();
        Fire();
    }



    private void CheckSprite() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (actualSprite == sprites[0]) {
                actualSprite = sprites[1];
            }
            else {
                actualSprite = sprites[0];
            }

            img.sprite = actualSprite;
        }
    }

    private void SetUpBorders() {
        Camera mainCamera = Camera.main;
        padding = 0.5f;
        leftBorder = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        rightBorder = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        upBorder = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).y + padding;
        downBorder = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0)).y - padding;
    }

    public override void Move() {
        var deltaX = Input.GetAxis("Horizontal") * GetVelocity().x * Time.deltaTime;
        var deltaY = Input.GetAxis("Vertical") * GetVelocity().y * Time.deltaTime;

        var nextPosX = Mathf.Clamp(transform.position.x + deltaX, leftBorder, rightBorder);
        var nextPosY = Mathf.Clamp(transform.position.y + deltaY, upBorder, downBorder);

        // pos(n) = pos(n-1) + v*t
        transform.position = new Vector2(nextPosX, nextPosY);
    }

    private float AngleWithCompensateRotation(Vector3 direction, int compensation) {
        var angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - compensation;
        return angle;
    }

    

    private void Rotate() {
        // vector_direccion_ataque = vector_posicion_mouse - vector_centro_camara
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        // angulo_rotacion = arcoseno(dy/dx) - 90 grados
        // 90 grados para compensar que el sprite tiene su 0° hacia el Norte y la camara tiene sus 0° hacia el Este
        // var angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 90;
        var angle_asimov = AngleWithCompensateRotation(dir, 90);
        // el sprite del proyectil ya apunta hacia el Este asi que no tiene compensacion
        var angle_bullet = AngleWithCompensateRotation(dir, 0);        
        rotation_asimov = Quaternion.AngleAxis(angle_asimov, Vector3.forward);
        rotation_bullet = Quaternion.AngleAxis(angle_bullet, Vector3.forward);
        // rotar el componente en el angulo calculado y con z como eje de rotacion
        transform.rotation = Quaternion.AngleAxis(angle_asimov, Vector3.forward);
    }

    private void MoveShield() {
        if(Input.GetAxis("Mouse ScrollWheel") != 0f) {
            shield.ShieldControl(Convert.ToInt32(Input.GetAxis("Mouse ScrollWheel") * 10));
        }
        
    }
    
    private void Fire() {
        if(this.timeForShoot <= 0) {
            if (Input.GetButton("Fire1")) {
                //Instantiate(bulletPrefab, bulletShootPoint1.position, rotation_bullet);
                //Instantiate(bulletPrefab, bulletShootPoint2.position, rotation_bullet);

                objectPool.Spawn("PlayerBullet", bulletShootPoint1.position, rotation_bullet);
                objectPool.Spawn("PlayerBullet", bulletShootPoint2.position, rotation_bullet);


                this.timeForShoot = this.GetTimeBetweenShoots();
            }            
        }
        else {
            timeForShoot -= Time.deltaTime;
        }
    }
        
    

    #endregion
}
