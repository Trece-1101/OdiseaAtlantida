using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Asimov : Ship
{
    #region "Atributos"
    private bool CanDash;
    private float DashCoolTime;
    private float InitialDashCoolTime;
    private float DashSpeed;
    private float DashDistance;
    private float DashStep;

    private bool CanRotate;
    private string Mode = "Attack";
    private float TransitionDelay;
    private bool InTransition;
    private float TransitionValueModifier = 1.5f;

    private bool HasPowerUp;
    private bool IsCloned;
    private PowerUp PowerUpType;
    private Vector2 OriginalVelocity;
    #endregion      

    #region "Referencias en Cache"     
    private Shield MyShield;
    private PolygonCollider2D AtackCollider;
    private PolygonCollider2D DefenseCollider;
    private Animator MyAnimator;
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

    public bool GetCanRotate() {
        return this.CanRotate;
    }
    public void SetGetCanRotate(bool value) {
        this.CanRotate = value;
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

    public bool GetHasPowerUp() {
        return this.HasPowerUp;
    }
    public void SetHasPowerUp(bool value) {
        this.HasPowerUp = value;
    }

    public bool GetIsCloned() {
        return this.IsCloned;
    }
    public void SetIsCloned(bool value) {
        this.IsCloned = value;
    }

    public Vector2 GetOriginalVelocity() {
        return this.OriginalVelocity;
    }
    public void SetOriginalVelocity(Vector2 value) {
        this.OriginalVelocity = value;
    }

    public PowerUp GetPowerUpType() {
        return this.PowerUpType;
    }
    public void SetPowerUpType(PowerUp value) {
        this.PowerUpType = value;
    }
    #endregion

    #region "Metodos"

    public override void CoAwake() {
        this.SetIsAlive(true);
        this.SetIsVulnerable(true);
        // GodMode up, up, down, down, left, right, left, right, B, A.

        this.MyShield = FindObjectOfType<Shield>();
        this.MyAnimator = GetComponent<Animator>();
        PolygonCollider2D[] colliders = GetComponents<PolygonCollider2D>();  

        this.AtackCollider = colliders[0];
        this.DefenseCollider = colliders[1];

        this.OriginalVelocity = this.GetVelocity();
        this.DashDistance = 4f;
        this.DashStep = 0.5f;
        this.InitialDashCoolTime = 2f;
        this.DashSpeed = 3f;
        this.DashCoolTime = this.InitialDashCoolTime;
        this.SetOriginalHitPoints(this.GetHitPoints());
        this.TransitionDelay = 3f;
        this.InTransition = false;
        

        this.SetTimeBetweenBulletShoots(0.15f);
        this.SetTimeBetweenMissileShoots(1.5f);
        this.RemainTimeForShootBullet = this.GetTimeBetweenBulletShoots();
        this.RemainTimeForShootMissile = this.GetTimeBetweenMissileShoots();
        this.CanShootMissile = true;
        this.CanShoot = true;
        this.IsCloned = false;
        this.CanRotate = true;
        //this.SetMyBulletVFX("PlayerBullet");
        //this.SetMyMissileVFX("PlayerMissile");
        //this.SetMyMicroBulletsVFX("MicroBulletPlayer");
        this.SetMyBulletVFX(this.GetProjectileNames()[0]);
        this.SetMyMissileVFX(this.GetProjectileNames()[1]);
        this.SetMyMicroBulletsVFX(this.GetProjectileNames()[2]);
    }

   
    private void Update() {
        Move();
        Dash();
        CheckRotation();
        MoveShield();
        RestartShield();
        Shoot();
        UsePowerUp();
        CheckMode();
    }

    public override void Move() {
        var movementX = Input.GetAxis("Horizontal"); // -1 a 0 => Izquierda -- 0 => Sin Movimiento (No Input) -- 0 a 1 => Derecha
        var movementY = Input.GetAxis("Vertical"); // -1 a 0 => Abajo -- 0 => Sin Movimiento (No Input) -- 0 a 1 => Arriba               

        // d = v*t
        // si el input = 0 --> deltaMov = 0
        var deltaX = movementX * GetVelocity().x * Time.deltaTime;
        var deltaY = movementY * GetVelocity().y * Time.deltaTime;

        //Debug.Log($"MovX {movementX} m/s -- MovY {movementY} m/s");
        //Debug.Log($"DeltaX {deltaX} m -- DeltaY {deltaY} m");

        // proxima pos = posicion actual + deltaMov -- bloqueo mi proxima posicion para que no pueda avanzar mas alla de los margenes
        // pos(n) = pos(n-1) + v*t -- v*t = deltaMov
        var nextPosX = Mathf.Clamp(transform.position.x + deltaX * this.GetGameProg().GetScale().x, this.GetGameProg().GetLeftBorder(), this.GetGameProg().GetRightBorder());
        var nextPosY = Mathf.Clamp(transform.position.y + deltaY * this.GetGameProg().GetScale().y, this.GetGameProg().GetUpBorder(), this.GetGameProg().GetDownBorder());

        Vector2 nextPosition = new Vector2(nextPosX, nextPosY);

        // mi vector posicion ahora vale la posicion x e y calculadas
        
        this.transform.position = nextPosition;
    }

    private void Dash() {
        this.DashCoolTime -= Time.deltaTime;
        if (this.DashCoolTime <= 0) {
            this.CanDash = true;
        }
        else {
            this.CanDash = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && this.CanDash) {
            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)) {
                //Debug.Log("Dash IZQ");
                DashVFX("Left");
                this.transform.position = new Vector3(Mathf.Lerp(this.transform.position.x, this.transform.position.x - this.DashDistance, this.DashStep),
                                                        this.transform.position.y,
                                                        this.transform.position.z);
            }
            else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)) {
                //Debug.Log("Dash DER");
                DashVFX("Right");
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

        if (!CanRotate) {
            rotations = this.Rotate(new Vector3(0f, 0f, 0f), 0, 0);
        }

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

    private void RestartShield() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            if (!this.MyShield.GetIsEnable()) {
                this.MyShield.RestartShield();                
            }
        }
    }

    private void CheckMode() {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !this.InTransition) {
            GameObject transition = this.GetPool().Spawn("TransitionAnimation", this.transform.position, this.transform.rotation);
            transition.transform.parent = this.transform;
            if(Mode == "Attack") {
                this.CanShoot = false;
                this.CanShootMissile = false;
                Invoke("ChangeToDefenseMode", this.TransitionDelay);
            }
            else {
                this.CanShoot = false;
                this.CanShootMissile = false;
                Invoke("ChangeToAttackMode", this.TransitionDelay);
            }            
        }
    }

    private void ChangeToDefenseMode() {
        this.MyAnimator.SetTrigger("DefenseState");
        this.Mode = "Defense";
        ModifyValuesWithTransitions(this.Mode);
        this.ChangeCollidersAndEndTransition(true);
    }

    private void ChangeToAttackMode() {
        this.MyAnimator.SetTrigger("AttackState");
        this.Mode = "Attack";
        ModifyValuesWithTransitions(this.Mode);
        this.ChangeCollidersAndEndTransition(false);
    }

    private void ModifyValuesWithTransitions(string mode) {
        if(mode == "Attack") {
            this.TimeBetweenBulletShoots /= (TransitionValueModifier * 2);
            this.SetVelocity(this.GetVelocity() / TransitionValueModifier);
        }
        else {
            this.TimeBetweenBulletShoots *= (TransitionValueModifier * 2);
            this.RefillHealth(this.HitPoints * 2);
            this.SetVelocity(this.GetVelocity() * TransitionValueModifier);
        }
    }

    private void ChangeCollidersAndEndTransition(bool value) {        
        this.DefenseCollider.enabled = value;
        this.AtackCollider.enabled = !value;
        this.CanShootMissile = !value;
        this.InTransition = false;
        this.CanShoot = true;
        //Debug.Log($"HitPoints {this.HitPoints} -- Vel {this.GetVelocity()} -- TimeBtwBullets {this.TimeBetweenBulletShoots}");
    }

    
    public override void Shoot() {
        if (this.RemainTimeForShootBullet <= 0 && this.CanShoot) {
            if (Input.GetButton("Fire1")) {
                ShootBullet();
                ShootMicroBullet();
                PlayShootSFX(this.GetShootBulletSFX(), this.GetMyMainCamera().transform.position, 0.2f);

                this.RemainTimeForShootBullet = this.GetTimeBetweenBulletShoots();
            }
        }
        else {
            this.RemainTimeForShootBullet -= Time.deltaTime;
        }

        if (this.RemainTimeForShootMissile <= 0 && this.CanShootMissile) {
            if (Input.GetButtonDown("Fire2")) {
                ShootMissile();
                PlayShootSFX(this.GetShootMissileSFX(), this.GetMyMainCamera().transform.position, 0.2f);                

                this.RemainTimeForShootMissile = this.GetTimeBetweenMissileShoots();
            }
        }
        else {
            this.RemainTimeForShootMissile -= Time.deltaTime;
        }
    }

    public override void ControlOtherCollision(Collider2D collision) {
        if(collision.gameObject.tag == "Enemy") {
            this.HitPoints = 0;
            Die();            
        }
    }


    private void PlayExplosion() {
        this.GetPool().Spawn("EnemyExplosion", this.transform.position + new Vector3(0.3f, 0.3f, 0f), Quaternion.identity);
        this.GetPool().Spawn("EnemyExplosion", this.transform.position + new Vector3(-0.3f, 0.3f, 0f), Quaternion.identity);
        this.GetPool().Spawn("EnemyExplosion", this.transform.position + new Vector3(0.3f, -0.3f, 0f), Quaternion.identity);
        this.GetPool().Spawn("EnemyExplosion", this.transform.position + new Vector3(-0.3f, -0.3f, 0f), Quaternion.identity);

        AudioSource.PlayClipAtPoint(this.GetDeathSFX(), this.GetMyMainCamera().transform.position, 0.6f);
    }

    private void DashVFX(string dir) {
        Vector3 repos = new Vector3(1.5f, 0f, 0f);
        if(dir == "Left") {
            this.GetPool().Spawn("PlayerDash", this.transform.position - repos, Quaternion.identity * Quaternion.Euler(0f, 0f, 180f));
        }
        else {
            this.GetPool().Spawn("PlayerDash", this.transform.position + repos, Quaternion.identity);
        }
    }

    public override void PlayImpactSFX() {
        this.GetPool().Spawn("ProyectileExplosion", this.transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(this.GetHurtSFX(), this.GetMyMainCamera().transform.position, 0.2f);
    }

    public override void Die() {
        this.ControlHealthBar();
        this.SetIsAlive(false);
        PlayExplosion();
        this.GetCameraShake().UltraShake();        
        Destroy(gameObject);
    }
    #endregion

    #region "Comportamientos PowerUps"
    private void UsePowerUp() {
        if (this.HasPowerUp) {
            if (Input.GetKeyDown(KeyCode.E)) {              
                this.PowerUpType.MakeYourMagic();
                this.HasPowerUp = false;
            }
            
        }
    }
    
    public void MakeShieldShoot() {
        if (!this.MyShield.GetIsEnable()) {
            this.MyShield.RestartShield();
        }
        this.MyShield.CanShoot = true;
        this.MyShield.Shoot();
    }
    
    #endregion

}
