//// Clase Padre que describe a todos los elementos del tipo Naves del juego.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Ship : MonoBehaviour, IAttack, IDefense
{
    #region "Atributos Serializados"
    [Header("General")]
    [SerializeField] private float OriginalHitPoints; // Los hitpoints o "vida" al iniciar el juego/escena
    [SerializeField] private Vector2 Velocity; // La velocidad (vectorial) de la nave
    [SerializeField] private Image HealthBar; // La imagen de la barra de vida

    [Header("SFX")]
    [SerializeField] private AudioClip DeathSFX; // Efecto de sondio al destruirse
    [SerializeField] private AudioClip ShootBulletSFX; // Efecto de sondio al disparar una bala
    [SerializeField] private AudioClip ShootMissileSFX; // Efecto de sondio al disparar un misil
    [SerializeField] private AudioClip HurtSFX; // Efecto de sondio al se alcanzado por un proyectil

    [Header("Shoot")]
    [SerializeField] private List<Transform> BulletShootsPositions; // Lista que contiene los puntos por donde dispara balas
    [SerializeField] private List<Transform> MissileShootsPositions; // Lista que contiene los puntos por donde dispara misiles
    [SerializeField] List<Transform> MicroBulletShootPosition; // Lista que contiene los puntos por donde dispara balas secundarias rotadas desde la derecha
    [SerializeField] private List<String> ProjectileNames; // Lista de strings que son los tags correspondientes a los proyectiles (balas, misiles, microbalas) que dispara la nave
    #endregion

    #region "Atributos"
    private bool IsAlive; // Atributo que controla si el objeto sigue activo (vivo)
    private bool IsVulnerable; // Atributo que determina si el objeto puede ser dañado o no

    public float TimeBetweenBulletShoots { get; set; } // Implementacion de la interfaz IAttack, tiempo que debe pasar para poder ejecutar disparos de balas consecutivos
    public float TimeBetweenMissileShoots { get; set; } // Implementacion de la interfaz IAttack, tiempo que debe pasar para poder ejecutar disparos de misiles consecutivos
    public float RemainTimeForShootBullet { get; set; } // Implementacion de la interfaz IAttack, tiempo restante para disparar balas
    public float RemainTimeForShootMissile { get; set; } // Implementacion de la interfaz IAttack, tiempo restante para disparar misiles
    public bool CanShoot { get; set; }  // Implementacion de la interfaz IAttack, determina la posibilidad de disparar balas
    public bool CanShootMissile { get; set; } // Implementacion de la interfaz IAttack, determina la posibilidad de disparar misiles
    public bool CanMove { get; set; } // Implementacion de la interfaz IAttack, determina la posibilidad de moverse

    public float HitPoints { get; set; } // Implementacion de la interfaz IDefense, cantidad de puntos de vida restante de la nave    

    private Quaternion MyRotation; // Rotacion actual de la nave (global)
    private Quaternion MyBulletRotation; // Rotacion actual del disparo que efectua la nave     
    #endregion

    #region "Auxiliares"
    private string MyBulletVFX; // Tag de la bala que dispara la nave, se usa en conjunto con ProjectileNames
    private string MyMissileVFX; // Tag del misil que dispara la nave, se usa en conjunto con ProjectileNames
    private string MyMicroBulletsVFX; // Tag de la bala secundaria que dispara la nave, se usa en conjunto con ProjectileNames
    List<string> PowerUpsNames; // Lista que contiene tags de los powerups que se pueden utilizar
    #endregion

    #region "Componentes en Cache"
    private ObjectPool ObjectPool; // "Pool" de objetos ya instanciados en la pre-carga, se llaman (activan) mediante tags (strings) que describen su nombre
    private GameProgram GameProg; // Atributo que enlaza al Game Program que es el encargado de las funciones generales del juego (es un singleton)
    public DamageControl DamageCtrl { set; get; } // Implementacion de la interfaz IDefense, clase auxiliar para el control de daño de los proyectiles y clases que generan daño o no  
    private ShakeYourBooty CameraShake; // Atributo que referencia a la clase encargada de efectos de camara
    private Camera MainCamera; // Atributo que referencia a la camara general del juego
    private float DificultyModifier; // Atributo que modifica a otros atributos dependiendo la dificultad elegida por el usuario
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

    public List<Transform> GettMicroBulletShootPoints() {
        return this.MicroBulletShootPosition;
    }
    public void SetMicroBulletShootPoints(List<Transform> value) {
        this.MicroBulletShootPosition = value;
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

    public List<String> GetProjectileNames() {
        return this.ProjectileNames;
    }
    public void SetProjectileNames(List<string> value) {
        this.ProjectileNames = value;
    }
    #endregion

    #region "Constructor"
    //public Ship(float hitpoints, Vector2 velocity) {
    //    this.HitPoints = hitpoints;
    //    this.Velocity = velocity;

    //}
    #endregion

    #region "Metodos"
    public virtual void Awake() {
        // Primer metodo que se ejecuta cuando el objeto es "visto" en la jerarquia
        // Enlazamos los componentes en cache con sus respectivas referencias
        this.GameProg = FindObjectOfType<GameProgram>();
        this.ObjectPool = ObjectPool.Instance;
        this.MainCamera = Camera.main;
        this.CameraShake = this.MainCamera.GetComponent<ShakeYourBooty>();
        this.DificultyModifier = PlayerPrefController.GetDificultyModifier();
        if(this.ObjectPool != null) {
            // Si existe el Pool de objetos (puede ser opcional en algunos escnarios)
            // Se carga en la lista de powerups los tags de cada uno existente en el pool
            this.PowerUpsNames = this.ObjectPool.GetComponentInChildren<ObjectPool>().GetPrefabsPowerUps();
        }

        // Asigno a cada atributo de visualizacion de disparo sus valores serializados
        this.SetMyBulletVFX(this.GetProjectileNames()[0]);
        this.SetMyMissileVFX(this.GetProjectileNames()[1]);
        this.SetMyMicroBulletsVFX(this.GetProjectileNames()[2]);

        // Atributos base
        this.HitPoints = this.OriginalHitPoints;
        this.SetIsAlive(true);
        this.SetIsVulnerable(true);       

    }

    // Metodos abstractos y virtuales
    public abstract void Shoot();
    public abstract void CheckRotation();
    public virtual void Die() { }
    public virtual void PlayImpactSFX() { }
    public virtual void ControlOtherCollision(Collider2D collision) { }
    public virtual void Move() { }
    

    public Dictionary<string, Quaternion> Rotate(Vector3 dir, int shipAngleCompensation, int bulletAngleCompesation) {
        // Metodo auxilar que toma el vector direccion de la nave OBJETIVO y los angulos de compensacion de la nave y su disparo (la que dispara)
        // Para orientarlos adecuadamente

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
        // Metodo auxiliar utilizado por "Rotate"
        var angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - compensation;
        return angle;
    }
    

    public virtual void ShootBullet() {
        // Metodo que toma todas las posiciones desde donde se pueden disparar balas y activa los objetos (Visualmente DISPARA) en el pool que correspondan
        // Y con la rotacion necesaria (GetMyBulletRotation se asigna en cada objeto gracias al metodo Rotate)
        for (int i = 0; i < this.BulletShootsPositions.Count; i++) {
            this.GetPool().Spawn(this.MyBulletVFX, this.BulletShootsPositions[i].position, this.GetMyBulletRotation());
        }
    }

    public void ShootMicroBullet() {
        // Metodo que toma todas las posiciones desde donde se pueden disparar balas secundarias y activa los objetos (Visualmente DISPARA)
        // En este caso la rotacion esta modificada por la rotacion local de cada bala secundaria (si la tuviera)
        for (int i = 0; i < this.MicroBulletShootPosition.Count; i++) {
            this.GetPool().Spawn(this.MyMicroBulletsVFX, this.MicroBulletShootPosition[i].position,
                                this.GetMyBulletRotation() * this.MicroBulletShootPosition[i].transform.localRotation);
        }
    }

    public void ShootMissile() {
        // Metodo que toma todas las posiciones desde donde se pueden disparar misiles y activa los objetos (Visualmente DISPARA)
        for (int i = 0; i < this.MissileShootsPositions.Count; i++) {
            this.GetPool().Spawn(this.MyMissileVFX, this.MissileShootsPositions[i].position, this.GetMyBulletRotation());
        }
    }

    public void PlayShootSFX(AudioClip sound, Vector3 position, float volumen) {
        // Metodo que efectua el sonido del disparo
        AudioSource.PlayClipAtPoint(sound, position, volumen);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        // Metodo que controla la collision de triggers con el objeto. Para esto nos valemos de la clase DamageControl
        this.DamageCtrl = collision.gameObject.GetComponent<DamageControl>();
        if (this.DamageCtrl != null && this.IsVulnerable) {
            // Si el objeto con el que chocamos posee en sus componentes una clase DamageControl implica que es un objeto "que hace daño"
            // Esto se ejecuta en el caso de que el objeto este vulnerable en el momento dado
            this.ReceiveDamage(DamageCtrl); // Controlamos el daño recibido 
            collision.gameObject.SetActive(false); // Desactivamos el objeto que colisiono contra la nave
            PlayImpactSFX(); // Ejecutamos el sonido correspondiente al impacto
            ShakeThatCamera(); // Llamamos al metodo para comprobar si hay que ejecutar esta animacion
        }
        else if(!this.IsVulnerable) {
            // Si el objeto esta en modo invulnerable podemos hacer algo aca, como ser una animacion
            //Debug.Log("jaja invencible");
        }
        else {
            // Si el objeto con el que colisionamos esta en una capa de colision pero no tiene un componente de DamageControl
            // Implica que el objeto es una nave enemiga. Condicion en la cual tenemos instant-death
            ControlOtherCollision(collision); // Metodo auxiliar que controla este tipo de colisiones "particulares"
        }
    }

    public void ReceiveDamage(DamageControl damageControl) {
        // Metodo que controla el daño recibido por un objeto que realiza daño (parcial)
        // A los puntos actuales le restamos el daño recibido
        this.HitPoints = this.HitPoints - damageControl.GetDamage();

        this.ControlHealthBar(); // Metodo que controla la visualizacion de la barra de vida

        // Si los puntos de vida son 0 (o menor) Ejecutamos el metodo de morir
        if(this.HitPoints <= 0) {
            this.Die();
        }
    }


    public void RefillHealth(float value) {
        // Metodo que permite rellenar (parcial o totalmente) los puntos de vida
        // A los puntos de vida actuales le sumamos el valor de relleno
        this.HitPoints = this.HitPoints + value;
        // Si tras la suma los puntos de vida superan a los puntos originales los capeamos en dichos puntos
        if (this.HitPoints > this.OriginalHitPoints) {
            this.HitPoints = this.OriginalHitPoints;
        }
        
        this.ControlHealthBar(); // Metodo que controla la visualizacion de la barra de vida
    }

    protected void ControlHealthBar() {
        // Metodo que controla la visualizacion de la barra de vida
        // El fill (relleno visual) de la barra es igual a los puntos actuales divido los originales (100%)
        this.HealthBar.fillAmount = this.HitPoints / this.OriginalHitPoints;
    }

    private void ShakeThatCamera() {
        // Metodo que controla si se debe sacudir la camara
        // Si esta instancia es la nave Asimov, ejecutar una animacion en particular
        if(this.gameObject.name == "Asimov") {
            CameraShake.ShakeShakeShake();
        }
    }
    
    #endregion
}
