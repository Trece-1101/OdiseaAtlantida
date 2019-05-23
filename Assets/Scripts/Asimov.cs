using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asimov : Ship
{
    #region "Atributos"
    private float DashCoolTime;
    private float InitialDashCoolTime;
    private float DashSpeed;
    private float DashDistance;
    private float DashStep;
    private bool CanDash;
    private bool GodMode;
    #endregion      

    #region "Referencias en Cache"    
    [SerializeField] private List<Sprite> MySprites;       
    private SpriteRenderer Img;
    private Sprite ActualSprite;    
    private Shield MyShield;
    private PolygonCollider2D AtackCollider;
    private PolygonCollider2D DefenseCollider;
    #endregion

    #region "Setters/Getters"
    public float GetDashCoolTime() {
        return this.DashCoolTime;
    }
    public void SetDashCoolTime(float value) {
        this.DashCoolTime = value;
    }

    public float GetInitialDashCoolTime() {
        return this.InitialDashCoolTime;
    }
    public void SetInitialDashCoolTime(float value) {
        this.InitialDashCoolTime = value;
    }

    public float GetDashSpeed() {
        return this.DashSpeed;
    }
    public void SetDashSpeed(float value) {
        this.DashSpeed = value;
    }

    public float GetDashDistance() {
        return this.DashDistance;
    }
    public void SetDashDistance(float value) {
        this.DashDistance = value;
    }

    public float GetDashStep() {
        return this.DashStep;
    }
    public void SetDashStep(float value) {
        this.DashStep = value;
    }

    public bool GetCanDash() {
        return this.CanDash;
    }
    public void SetCanDash(bool value) {
        this.CanDash = value;
    }

    public bool GetGodMode() {
        return this.GodMode;
    }
    public void SetGodMode(bool value) {
        this.GodMode = value;
    }

    public List<Sprite> GetMySprites() {
        return this.MySprites;
    }
    public void SetMySprites(List<Sprite> value) {
        this.MySprites = value;
    }

    public SpriteRenderer GetImg() {
        return this.Img;
    }
    public void SetImg(SpriteRenderer value) {
        this.Img = value;
    }

    public Sprite GetActualSprite() {
        return this.ActualSprite;
    }
    public void SetActualSprite(Sprite value) {
        this.ActualSprite = value;
    }

    public Shield GetMyShield() {
        return this.MyShield;
    }
    public void SetMyShield(Shield value) {
        this.MyShield = value;
    }

    public PolygonCollider2D GetAtackCollider() {
        return this.AtackCollider;
    }
    public void SetAtackColiider(PolygonCollider2D value) {
        this.AtackCollider = value;
    }

    public PolygonCollider2D GetDefenseCollider() {
        return this.DefenseCollider;
    }
    public void SetDefenseCollider(PolygonCollider2D value) {
        this.DefenseCollider = value;
    }

    #endregion
    
    #region "Metodos"

    public override void CoAwake() {
        this.SetIsAlive(true);

        this.MyShield = FindObjectOfType<Shield>();
        this.Img = GetComponent<SpriteRenderer>();
        PolygonCollider2D[] colliders = GetComponents<PolygonCollider2D>();
        this.AtackCollider = colliders[0];
        this.DefenseCollider = colliders[1];
        this.ActualSprite = MySprites[0];
        this.Img.sprite = this.ActualSprite;

        this.DashDistance = 10f;
        this.DashStep = 0.5f;
        this.GodMode = false;
        this.InitialDashCoolTime = 2f;
        this.DashSpeed = 3f;
        this.DashCoolTime = this.InitialDashCoolTime;

        this.SetTimeBetweenBulletShoots(0.2f);
        this.SetTimeBetweenMissileShoots(1f);
        this.RemainTimeForShootBullet = this.GetTimeBetweenBulletShoots();
        this.RemainTimeForShootMissile = this.GetTimeBetweenMissileShoots();
        this.CanShootMissile = true;
        this.CanShoot = true;
        this.SetMyBulletVFX("PlayerBullet");
        this.SetMyMissileVFX("PlayerMissile");

    }

   
    private void Update() {
        Move();
        Dash();
        CheckRotation();
        MoveShield();
        CheckSprite();
        Shoot();
    }

    public override void Move() {
        var deltaX = Input.GetAxis("Horizontal") * GetVelocity().x * Time.deltaTime;
        var deltaY = Input.GetAxis("Vertical") * GetVelocity().y * Time.deltaTime;

        var nextPosX = Mathf.Clamp(transform.position.x + deltaX, this.GetGameProg().GetLeftBorder(), this.GetGameProg().GetRightBorder());
        var nextPosY = Mathf.Clamp(transform.position.y + deltaY, this.GetGameProg().GetUpBorder(), this.GetGameProg().GetDownBorder());

        // pos(n) = pos(n-1) + v*t
        this.transform.position = new Vector2(nextPosX, nextPosY);
    }

    private void Dash() {
        this.DashCoolTime -= Time.deltaTime;
        if (this.DashCoolTime <= 0) {
            this.CanDash = true;
        }
        else {
            this.CanDash = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && this.CanDash) {
            if (Input.GetKey(KeyCode.A)) {
                //Debug.Log("Dash IZQ");
                this.transform.position = new Vector3(Mathf.Lerp(this.transform.position.x, this.transform.position.x - this.DashDistance, this.DashStep),
                                                        this.transform.position.y,
                                                        this.transform.position.z);
            }
            else if (Input.GetKey(KeyCode.D)) {
                //Debug.Log("Dash DER");
                this.transform.position = new Vector3(Mathf.Lerp(this.transform.position.x, this.transform.position.x + this.DashDistance, this.DashStep),
                                                        this.transform.position.y,
                                                        this.transform.position.z);
            }

            this.DashCoolTime = this.InitialDashCoolTime;
        }
        //Debug.Log(canDash);
    }

    public override void CheckRotation() {
        // vector_direccion_ataque = vector_posicion_mouse - vector_centro_camara
        // 90 grados para compensar que el sprite tiene su 0° hacia el Norte y la camara tiene sus 0° hacia el Este
        // el sprite del proyectil ya apunta hacia el Este asi que no tiene compensacion

        var rotations = this.Rotate(Input.mousePosition - this.GetMyMainCamera().WorldToScreenPoint(this.transform.position), 90, 0);

        this.SetMyRotation(rotations["rotation_ship"]);
        this.SetMyBulletRotation(rotations["rotation_bullet"]);
        this.transform.rotation = rotations["transform_rotation"];
    }

    private void MoveShield() {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f) {
            this.MyShield.ShieldControl(Convert.ToInt32(Input.GetAxis("Mouse ScrollWheel") * 10));
        }

        if (Input.GetMouseButtonDown(2)) {
            this.MyShield.GetShieldOnFront();
        }

    }

    private void CheckSprite() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (this.ActualSprite == this.MySprites[0]) {
                this.ActualSprite = this.MySprites[1];
                ChangeMode(true, 1);
            }
            else {
                this.ActualSprite = this.MySprites[0];
                ChangeMode(false, 0);
            }

            this.Img.sprite = this.ActualSprite;

        }
    }

    private void ChangeMode(bool value, int sprite) {
        this.ActualSprite = this.MySprites[sprite];
        this.AtackCollider.enabled = !value;
        this.DefenseCollider.enabled = value;
        this.CanShootMissile = !value;
    }   

    
    public override void Shoot() {
        if (this.RemainTimeForShootBullet <= 0) {
            if (Input.GetButton("Fire1")) {
                ShootBullet();
                PlayShootSFX(this.GetShootBulletSFX(), this.GetMyMainCamera().transform.position, 0.2f);

                this.RemainTimeForShootBullet = this.GetTimeBetweenBulletShoots();
            }
        }
        else {
            this.RemainTimeForShootBullet -= Time.deltaTime;
        }

        if (this.RemainTimeForShootMissile <= 0 && this.CanShootMissile) {
            if (Input.GetButton("Fire2")) {
                ShootMissile();
                PlayShootSFX(this.GetShootMissileSFX(), this.GetMyMainCamera().transform.position, 0.2f);


                this.RemainTimeForShootMissile = this.GetTimeBetweenMissileShoots();
            }
        }
        else {
            this.RemainTimeForShootMissile -= Time.deltaTime;
        }
    }       


    private void PlayExplosion() {
        this.GetPool().Spawn("EnemyExplosion", this.transform.position + new Vector3(0.3f, 0.3f, 0f), Quaternion.identity);
        this.GetPool().Spawn("EnemyExplosion", this.transform.position + new Vector3(-0.3f, 0.3f, 0f), Quaternion.identity);
        this.GetPool().Spawn("EnemyExplosion", this.transform.position + new Vector3(0.3f, -0.3f, 0f), Quaternion.identity);
        this.GetPool().Spawn("EnemyExplosion", this.transform.position + new Vector3(-0.3f, -0.3f, 0f), Quaternion.identity);

        AudioSource.PlayClipAtPoint(this.GetDeathSFX(), this.GetMyMainCamera().transform.position, 0.6f);
    }

    public override void PlayImpactSFX() {
        this.GetPool().Spawn("ProyectileExplosion", this.transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(this.GetHurtSFX(), this.GetMyMainCamera().transform.position, 0.2f);
    }

    public override void Die() {
        this.SetIsAlive(false);
        Destroy(gameObject);
        PlayExplosion();
        FindObjectOfType<LevelManager>().LoadGameOver();
    }


    #endregion
}
