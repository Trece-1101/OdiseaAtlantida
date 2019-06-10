using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Ship : MonoBehaviour, IAttack, IDefense
{
    #region "Atributos Serializados"
    [Header("General")]
    [SerializeField] private float OriginalHitPoints;
    [SerializeField] private Vector2 Velocity;
    [SerializeField] private Image HealthBar;

    [Header("SFX")]
    [SerializeField] private AudioClip DeathSFX;
    [SerializeField] private AudioClip ShootBulletSFX;
    [SerializeField] private AudioClip ShootMissileSFX;
    [SerializeField] private AudioClip HurtSFX;

    [Header("Shoot")]
    [SerializeField] private List<Transform> BulletShootsPositions;
    [SerializeField] private List<Transform> MissileShootsPositions;
    [SerializeField] private List<Transform> LeftMicroBulletsShootsPositions;
    [SerializeField] private List<Transform> RightMicroBulletsShootsPositions;
    
    #endregion

    #region "Atributos"
    private bool IsAlive;
    private bool IsVulnerable;

    public float TimeBetweenBulletShoots { get; set; }
    public float TimeBetweenMissileShoots { get; set; }
    public float RemainTimeForShootBullet { get; set; }
    public float RemainTimeForShootMissile { get; set; }
    public bool CanShoot { get; set; }
    public bool CanShootMissile { get; set; }
    public bool CanMove { get; set; }
    public float HitPoints { get; set; }
    

    private Quaternion MyRotation;
    private Quaternion MyBulletRotation;

    private string MyBulletVFX;
    private string MyMissileVFX;
    private string MyMicroBulletsVFX;
    List<string> PowerUpsNames;
    #endregion

    #region "Componentes en Cache"
    private Rigidbody2D Body;
    private ObjectPool ObjectPool;
    private GameProgram GameProg;
    public DamageControl DamageCtrl { set; get; }
    private Camera MainCamera;
    private ShakeYourBooty CameraShake;
    private float DificultyModifier;
    #endregion

    #region "Setters/Getters"
    public float GetHitPoints() {
        return this.HitPoints;
    }
    public void SetHitPoints(float value) {
        this.HitPoints = value;
    }

    public float GetOriginalHitPoints() {
        return this.OriginalHitPoints;
    }
    public void SetOriginalHitPoints(float value) {
        this.OriginalHitPoints = value;
    }

    public Vector2 GetVelocity() {
        return this.Velocity;
    }
    public void SetVelocity(Vector2 value) {
        this.Velocity = value;
    }

    public Image GetHealthBar() {
        return this.HealthBar;
    }
    public void SetHealthBar(Image value) {
        this.HealthBar = value;
    }

    public Rigidbody2D GetBody() {
        return this.Body;
    }
    public void SetBody(Rigidbody2D value) {
        this.Body = value;
    }

    public ObjectPool GetPool() {
        return this.ObjectPool;
    }
    public void SetPool(ObjectPool value) {
        this.ObjectPool = value;
    }

    public GameProgram GetGameProg() {
        return this.GameProg;
    }
    public void SetGameProg(GameProgram value) {
        this.GameProg = value;
    }

    public AudioClip GetDeathSFX() {
        return this.DeathSFX;
    }
    public void SetDeathSFX(AudioClip value) {
        this.DeathSFX = value;
    }

    public AudioClip GetShootBulletSFX() {
        return this.ShootBulletSFX;
    }
    public void SetShootBulletSFX(AudioClip value) {
        this.ShootBulletSFX = value;
    }

    public AudioClip GetShootMissileSFX() {
        return this.ShootMissileSFX;
    }
    public void SetShootMissileSFX(AudioClip value) {
        this.ShootMissileSFX = value;
    }

    public AudioClip GetHurtSFX() {
        return this.HurtSFX;
    }
    public void SetHurtSFX(AudioClip value) {
        this.HurtSFX = value;
    }

    public List<Transform> GetBulletShootPoints() {
        return this.BulletShootsPositions;
    }
    public void SetBulletShootPoints(List<Transform> value) {
        this.BulletShootsPositions = value;
    }

    public List<Transform> GetLeftMicroBulletShootPoints() {
        return this.LeftMicroBulletsShootsPositions;
    }
    public void SetLeftMicroBulletsShootsPositions(List<Transform> value) {
        this.LeftMicroBulletsShootsPositions = value;
    }

    public List<Transform> GetRightMicroBulletShootPoints() {
        return this.RightMicroBulletsShootsPositions;
    }
    public void SetRightMicroBulletShootPoints(List<Transform> value) {
        this.RightMicroBulletsShootsPositions = value;
    }


    public List<Transform> GetMissileShootPoints() {
        return this.MissileShootsPositions;
    }
    public void SetMissileShootPoint1(List<Transform> value) {
        this.MissileShootsPositions = value;
    }
   
    public Quaternion GetMyRotation() {
        return this.MyRotation;
    }
    public void SetMyRotation(Quaternion value) {
        this.MyRotation = value;
    }

    public Quaternion GetMyBulletRotation() {
        return this.MyBulletRotation;
    }
    public void SetMyBulletRotation(Quaternion value) {
        this.MyBulletRotation = value;
    }

    public string GetMyBulletVFX() {
        return this.MyBulletVFX;
    }
    public void SetMyBulletVFX(string value) {
        this.MyBulletVFX = value;
    }

    public string GetMyMicroBulletVFX() {
        return this.MyMicroBulletsVFX;
    }
    public void SetMyMicroBulletsVFX(string value) {
        this.MyMicroBulletsVFX = value;
    }

    public string GetMyMissileVFX() {
        return this.MyMissileVFX;
    }
    public void SetMyMissileVFX(string value) {
        this.MyMissileVFX = value;
    }

    public Camera GetMyMainCamera() {
        return this.MainCamera;
    }
    public void SetMyMainCamera(Camera value) {
        this.MainCamera = value;
    }

    public float GetTimeBetweenBulletShoots() {
        return this.TimeBetweenBulletShoots;
    }
    public void SetTimeBetweenBulletShoots(float value) {
        this.TimeBetweenBulletShoots = value;
    }

    public float GetTimeBetweenMissileShoots() {
        return this.TimeBetweenMissileShoots;
    }
    public void SetTimeBetweenMissileShoots(float value) {
        this.TimeBetweenMissileShoots = value;
    }

    public bool GetIsAlive() {
        return this.IsAlive;
    }
    public void SetIsAlive(bool value) {
        this.IsAlive = value;
    }

    public bool GetIsVulnerable() {
        return this.IsVulnerable;
    }
    public void SetIsVulnerable(bool value) {
        this.IsVulnerable = value;
    }

    public ShakeYourBooty GetCameraShake() {
        return this.CameraShake;
    }
    public void SetCameraShake(ShakeYourBooty value) {
        this.CameraShake = value;
    }

    public List<String> GetPowerUpsNames() {
        return this.PowerUpsNames;
    }

    public float GetDificultyModifier() {
        return this.DificultyModifier;
    }
    #endregion

    #region "Constructor"
    //public Ship(float hitpoints, Vector2 velocity) {
    //    this.HitPoints = hitpoints;
    //    this.Velocity = velocity;

    //}
    #endregion

    #region "Metodos"
    private void Awake() {
        this.GameProg = FindObjectOfType<GameProgram>();
        this.Body = GetComponent<Rigidbody2D>();
        this.ObjectPool = ObjectPool.Instance;

        this.MainCamera = Camera.main;
        this.CameraShake = this.MainCamera.GetComponent<ShakeYourBooty>();

        this.DificultyModifier = PlayerPrefController.GetDificultyModifier();

        this.HitPoints = this.OriginalHitPoints;

        if(this.ObjectPool != null) {
            this.PowerUpsNames = this.ObjectPool.GetComponentInChildren<ObjectPool>().GetPrefabsPowerUps();

        }
        
        CoAwake();
    }

    public abstract void CoAwake();
    public abstract void Shoot();
    public abstract void CheckRotation();
    public virtual void Die() { }
    public virtual void PlayImpactSFX() { }
    public virtual void ControlOtherCollision(Collider2D collision) { }
    public virtual void Move() { }
    
    public Dictionary<string, Quaternion> Rotate(Vector3 dir, int shipAngleCompensation, int bulletAngleCompesation) {
        // vector_direccion_ataque = vector_posicion_mouse - vector_centro_camara // en el caso de nuestra nave
        // vector_direccion_ataque = vector_posicion_asimov // en el caso de los enemigos
 
        // angulo_rotacion = arcoseno(dy/dx) - X grados
        // 90 grados para compensar que el sprite tiene su 0° hacia el Norte y la camara tiene sus 0° hacia el Este en el caso de la asimov
        // var angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 90;
        var angle_ship = this.AngleWithCompensateRotation(dir, shipAngleCompensation);
        // el sprite del proyectil ya apunta hacia el Este asi que no tiene compensacion
        var angle_bullet = this.AngleWithCompensateRotation(dir, bulletAngleCompesation);
        var rotation_ship = Quaternion.AngleAxis(angle_ship, Vector3.forward);
        var rotation_bullet = Quaternion.AngleAxis(angle_bullet, Vector3.forward);
        // rotar el componente en el angulo calculado y con z como eje de rotacion
        transform.rotation = Quaternion.AngleAxis(angle_ship, Vector3.forward);
        Dictionary<string, Quaternion> rotations = new Dictionary<string, Quaternion>() {
            {"rotation_ship", rotation_ship },
            {"rotation_bullet", rotation_bullet },
            {"transform_rotation", transform.rotation }
        };

        return rotations;
    }

    public float AngleWithCompensateRotation(Vector3 direction, int compensation) {
        var angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - compensation;
        return angle;
    }
    

    public virtual void ShootBullet() {
        for (int i = 0; i < this.BulletShootsPositions.Count; i++) {
            this.GetPool().Spawn(this.MyBulletVFX, this.BulletShootsPositions[i].position, this.GetMyBulletRotation());
        }
    }

    public void ShootMicroBullet() {
        for (int i = 0; i < this.LeftMicroBulletsShootsPositions.Count; i++) {
            this.GetPool().Spawn(this.MyMicroBulletsVFX, this.LeftMicroBulletsShootsPositions[i].position,
                                this.GetMyBulletRotation() * this.LeftMicroBulletsShootsPositions[i].transform.localRotation);
        }
        for (int i = 0; i < this.RightMicroBulletsShootsPositions.Count; i++) {
            this.GetPool().Spawn(this.MyMicroBulletsVFX, this.RightMicroBulletsShootsPositions[i].position,
                                this.GetMyBulletRotation() * this.RightMicroBulletsShootsPositions[i].transform.localRotation);
        }
    }

    public void ShootMissile() {
        for (int i = 0; i < this.MissileShootsPositions.Count; i++) {
            this.GetPool().Spawn(this.MyMissileVFX, this.MissileShootsPositions[i].position, this.GetMyBulletRotation());
        }
    }

    public void PlayShootSFX(AudioClip sound, Vector3 position, float volumen) {
        AudioSource.PlayClipAtPoint(sound, position, volumen);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        this.DamageCtrl = collision.gameObject.GetComponent<DamageControl>();
        if (this.DamageCtrl != null && this.IsVulnerable) {
            ReceiveDamage(DamageCtrl);
            collision.gameObject.SetActive(false);
            PlayImpactSFX();
            ShakeThatCamera();
        }
        else if(!this.IsVulnerable) {
            //Debug.Log("jaja invencible");
        }
        else {
            ControlOtherCollision(collision);
        }
    }

    public void ReceiveDamage(DamageControl damageControl) {
        this.HitPoints = this.HitPoints - damageControl.GetDamage();

        this.ControlHealthBar();

        if(this.HitPoints <= 0) {
            this.Die();
        }
    }

    public void RefillHealth(float value) {
        this.HitPoints = this.HitPoints + value;
        if (this.HitPoints > this.OriginalHitPoints) {
            this.HitPoints = this.OriginalHitPoints;
        }
        this.ControlHealthBar();
    }

    protected void ControlHealthBar() {
        this.HealthBar.fillAmount = this.HitPoints / this.OriginalHitPoints;
    }

    private void ShakeThatCamera() {
        if(this.gameObject.name == "Asimov") {
            CameraShake.ShakeShakeShake();
        }
    }

    
    #endregion
}
