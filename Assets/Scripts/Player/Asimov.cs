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
    private float ShieldRestartCoolTime;
    private float InitialRestartCoolTime;
    private bool HasPowerUp;
    private PowerUp PowerUpType;
    private Vector2 OriginalVelocity;
    private GameObject drone1;
    private GameObject drone2;
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

    public bool GetHasPowerUp() {
        return this.HasPowerUp;
    }
    public void SetHasPowerUp(bool value) {
        this.HasPowerUp = value;
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
        this.Img = GetComponent<SpriteRenderer>();
        PolygonCollider2D[] colliders = GetComponents<PolygonCollider2D>();  

        this.AtackCollider = colliders[0];
        this.DefenseCollider = colliders[1];
        //this.ActualSprite = MySprites[0];
        //this.Img.sprite = this.ActualSprite;

        this.OriginalVelocity = this.GetVelocity();
        this.DashDistance = 4f;
        this.DashStep = 0.5f;
        this.InitialDashCoolTime = 2f;
        this.DashSpeed = 3f;
        this.DashCoolTime = this.InitialDashCoolTime;
        this.SetStartHealth(this.GetHitPoints());

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
        RestartShield();
        Shoot();
        UsePowerUp();
    }

    public override void Move() {
        var movementX = Input.GetAxis("Horizontal"); // -1 a 0 => Izquierda -- 0 => Sin Movimiento (No Input) -- 0 a 1 => Derecha
        var movementY = Input.GetAxis("Vertical"); // -1 a 0 => Abajo -- 0 => Sin Movimiento (No Input) -- 0 a 1 => Arriba               


        // d = v*t
        // si el input = 0 --> deltaMov = 0
        var deltaX = movementX * GetVelocity().x * Time.deltaTime;
        var deltaY = movementY * GetVelocity().y * Time.deltaTime;

        //Debug.Log($"MovX {movementX} -- MovY {movementY}");
        //Debug.Log($"DeltaX {deltaX} -- DeltaY {deltaY}");

        // proxima pos = posicion actual + deltaMov -- bloqueo mi proxima posicion para que no pueda avanzar mas alla de los margenes
        // pos(n) = pos(n-1) + v*t -- v*t = deltaMov
        var nextPosX = Mathf.Clamp(transform.position.x + deltaX, this.GetGameProg().GetLeftBorder(), this.GetGameProg().GetRightBorder());
        var nextPosY = Mathf.Clamp(transform.position.y + deltaY, this.GetGameProg().GetUpBorder(), this.GetGameProg().GetDownBorder());

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
            ShootAirMines(); // TODO: quitar esto
        }

    }

    private void RestartShield() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            if (!this.MyShield.GetIsEnable()) {
                this.MyShield.RestartShield();                
            }
        }
    }

    private void CheckSprite() {
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
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
            if (Input.GetButtonDown("Fire2")) {
                ShootMissile();
                PlayShootSFX(this.GetShootMissileSFX(), this.GetMyMainCamera().transform.position, 0.2f);
                GiveMeMyDrones();

                this.RemainTimeForShootMissile = this.GetTimeBetweenMissileShoots();
            }
        }
        else {
            this.RemainTimeForShootMissile -= Time.deltaTime;
        }
    }

    public override void ControlOtherCollision(Collider2D collision) {
        if(collision.gameObject.tag == "Enemy") {
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

    public void BigShield() {
        this.MyShield.BigShield();
    }
    public void OriginalShield() {
        this.MyShield.NormalShield();
    }

    public void MoveShieldOnDemand(int value) {
        this.MyShield.ShieldControl(value);
    }

    public void GiveMeMyDrones() {
        drone1 = this.GetPool().Spawn("Drone", this.transform.position, this.transform.rotation);
        drone2 = this.GetPool().Spawn("Drone", this.transform.position, this.transform.rotation);

        drone1.transform.parent = gameObject.transform;
        drone2.transform.parent = gameObject.transform;

        drone1.transform.localPosition = new Vector3(1.5f, 0f, 0f);
        drone2.transform.localPosition = new Vector3(-1.5f, 0f, 0f);
    }

    public void DestroyMyDrones() {
        drone1.SetActive(false);
        drone2.SetActive(false);
    }

    public void ShootAirMines() {
        GameObject bomb1 = this.GetPool().Spawn("AirMine", this.transform.position, Quaternion.identity);
        GameObject bomb2 = this.GetPool().Spawn("AirMine", this.transform.position, Quaternion.identity);
        GameObject bomb3 = this.GetPool().Spawn("AirMine", this.transform.position, Quaternion.identity);
        GameObject bomb4 = this.GetPool().Spawn("AirMine", this.transform.position, Quaternion.identity);
        GameObject bomb5 = this.GetPool().Spawn("AirMine", this.transform.position, Quaternion.identity);
        GameObject bomb6 = this.GetPool().Spawn("AirMine", this.transform.position, Quaternion.identity);

        List<GameObject> bombs = new List<GameObject>() { bomb1, bomb2, bomb3, bomb4, bomb5, bomb6 };

        foreach (var bomb in bombs) {
            bomb.GetComponent<AirMine>().SetTarget(new Vector2(GetRandomTarget(), GetRandomTarget()));
            bomb.GetComponent<AirMine>().SetShooted(true);
        }

    }

    private float GetRandomTarget() {
        float rnd = 0f;
        while(rnd >= -4 && rnd <= 4f) {
            rnd = UnityEngine.Random.Range(-5f, 5f);
        }
        return rnd;
    }

    #endregion

}
