//// Clase que Controla el escudo de la nave que maneja el Player

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, IAttack, IDefense
{

    #region "Atritutos Serializados"
    [Header("Shoot")]
    [SerializeField] private List<Transform> ShootsPositions = null; // Listado de puntos desde donde puede disparar el escudo
    [SerializeField] public float OriginalHitPoints = 100f; // Los hitpoints o "vida" al iniciar el juego/escena
    #endregion

    #region "Atributos"
    private bool IsEnable = true; // Atributo que controla si el escudo esta activo    

    public float TimeBetweenBulletShoots { get; set; } // Implementacion de la interfaz IAttack, tiempo que debe pasar para poder ejecutar disparos de balas consecutivos
    public float TimeBetweenMissileShoots { get; set; } // Implementacion de la interfaz IAttack, tiempo que debe pasar para poder ejecutar disparos de misiles consecutivos
    public float RemainTimeForShootBullet { get; set; } // Implementacion de la interfaz IAttack, tiempo restante para disparar balas
    public float RemainTimeForShootMissile { get; set; } // Implementacion de la interfaz IAttack, tiempo restante para disparar misiles
    public bool CanShoot { get; set; }  // Implementacion de la interfaz IAttack, determina la posibilidad de disparar balas
    public bool CanShootMissile { get; set; } // Implementacion de la interfaz IAttack, determina la posibilidad de disparar misiles
    public bool CanMove { get; set; } // Implementacion de la interfaz IAttack, determina la posibilidad de moverse

    public float HitPoints { get; set; } // Implementacion de la interfaz IDefense, cantidad de puntos de vida restante de la nave
    #endregion

    #region "Auxiliares"
    private int PositionCounter; // Controlador de valor entero de la posicion
    private List<Vector3> Positions = new List<Vector3>(); // Lista de posiciones del escudo
    private List<Quaternion> Rotations = new List<Quaternion>(); // Lista de rotaciones del escudo
    #endregion

    #region "Componentes en Cache"
    private ObjectPool Pool; // Referencia al Pool que contiene los objetos instanciados
    private Asimov Player; // Referencia al Player
    private Animator MyAnimator; // Referencia al componente Animator
    public DamageControl DamageCtrl { get; set; } // Implementacion de la interfaz IDefense
    #endregion

    #region "Setters y Getters"
    public int GetPositionCounter() {
        return this.PositionCounter;
    }
    public void SetPositionCounter(int value) {
        this.PositionCounter = value;
    }

    public List<Vector3> GetPositions() {
        return this.Positions;
    }
    public void SetPositions(List<Vector3> value) {
        this.Positions = value;
    }

    public List<Quaternion> GetRotations() {
        return this.Rotations;
    }
    public void SetRotations(List<Quaternion> value) {
        this.Rotations = value;
    }    

    public bool GetIsEnable() {
        return this.IsEnable;
    }
    public void SetIsEnable(bool value) {
        this.IsEnable = value;
    }

    public float GetOriginalHitPoints() {
        return this.HitPoints;
    }
    public void SetOriginalHitPoints(float value) {
        this.OriginalHitPoints = value;
    }
    #endregion

    #region "Metodos"
    private void Start() {
        // Primer metodo que se ejecuta cuando el objeto es "visto" en la jerarquia si no esta declarado "Awake"
        // Enlazamos los componentes en cache con sus respectivas referencias
        this.Player = FindObjectOfType<Asimov>();
        this.Pool = ObjectPool.Instance;
        this.MyAnimator = GetComponent<Animator>();

        // Asignamos valores de inicio
        TimeBetweenBulletShoots = 0.8f;
        this.OriginalHitPoints = 60;
        this.HitPoints = this.OriginalHitPoints;

        this.RemainTimeForShootBullet = this.TimeBetweenBulletShoots;
        this.RemainTimeForShootMissile = this.TimeBetweenMissileShoots;
        this.CanMove = true;
        this.CanShoot = true;

        // Asignamos las posiciones y rotaciones a las listas
        this.Positions.Add(new Vector3(0f, 0.4f, 0f)); // escudo arriba --> indice 0
        this.Positions.Add(new Vector3(0.4f, 0f, 0f)); // escudo derecha --> indice 1
        this.Positions.Add(new Vector3(0f, -0.4f, 0f)); // escudo abajo --> indice 2
        this.Positions.Add(new Vector3(-0.4f, 0f, 0f)); // escudo izquierda -- indice 3

        this.Rotations.Add(Quaternion.Euler(0f, 0f, 270f)); // escudo arriba --> indice 0
        this.Rotations.Add(Quaternion.Euler(0f, 0f, 180f)); // escudo derecha --> indice 1 
        this.Rotations.Add(Quaternion.Euler(0f, 0f, 90f)); // escudo abajo --> indice 2
        this.Rotations.Add(Quaternion.Euler(0f, 0f, 0f)); // escudo izquierda -- indice 3

        // Ponemos el contador de posicion en 0 y llamamos al metodo que mueve el escudo
        this.PositionCounter = 0; // escudo arriba
        this.ShieldControl(this.PositionCounter); // Metodo que controla posicion y mueve escudo
    }

    public void ShieldControl(int value) {
        // Este metodo controla la posicion y mueve el escudo, recibe un entero (-1, 0, 1)
        // Al valor actual de la posicion le sumamos el entero recibido, si es negativo restara
        this.PositionCounter += value;

        if (this.PositionCounter > 3) {
            // Si el valor es mayor a 3 tiene que pegar la vuelta sentido horario (indice 4 == 0)
            this.PositionCounter = 0;
        }

        if (this.PositionCounter < 0) {
            // Si el valor es menor a 0 tiene que pegar la vuelta sentido anti-horario (indice -1 == 3)
            this.PositionCounter = 3;
        }

        // Ya teniendo el valor del indice chequeamos si el escudo esta en condiciones de moverse y aplicamos la rotacion y posicion
        if (this.CanMove) {
            this.transform.localPosition = this.Positions[this.PositionCounter];
            this.transform.localRotation = this.Rotations[this.PositionCounter];
        }
    }

    public void GetShieldOnFront() {
        // Metodo que coloca el escudo en posicion frotal. Simplemente resta al entero de la posicion a si mismo dejandolo en 0
        this.ShieldControl(-this.PositionCounter);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        // Metodo que controla las colisiones con el escudo. 
        this.DamageCtrl = collision.gameObject.GetComponent<DamageControl>();
        if (this.DamageCtrl != null) {
            // Si colisiona con un objeto que posea un componente DamageControl desactiva a dicho componente
            collision.gameObject.SetActive(false);
            this.ReceiveDamage(this.DamageCtrl); // Controla el daño recibido
        }

    }

    public void ReceiveDamage(DamageControl damageControl) {
        // Metodo que controla el daño recibido. Resta el valor del daño a los puntos de vida del escudo
        this.HitPoints -= this.DamageCtrl.GetDamage();
        this.ControlShieldLife(); // Llamada al metodo que controla la vida actual
    }

    private void ControlShieldLife() {
        // Metodo que controla la vida del escudo.
        var lifePercentage = this.HitPoints / this.OriginalHitPoints; // Variable del porcentual de vida para manejo de animaciones

        if (lifePercentage >= 0.75) {
            // Si el porcentaje es mayor o igual a 75 se activa la animacion de highlife
            this.MyAnimator.SetTrigger("HighLife");
        }
        else if (lifePercentage < 0.75 && lifePercentage >= 0.25) {
            // Si el porcentaje es menor a 75 y mayor o igual a 25 se activa halflife
            this.MyAnimator.SetTrigger("HalfLife");
        }
        else {
            // Si el porcentaje es menor a 25 se activa lowlife
            this.MyAnimator.SetTrigger("LowLife");
        }

        if (this.HitPoints <= 0) {
            // Si los puntos de vida son menor o igual a 0 se desactiva al escudo, se le quita la posibilidad de dispara y se llama al metodo morir.
            this.CanShoot = false;
            this.IsEnable = false;
            this.Die();
        }        
    }

    public void Die() {
        // Metodo que desactiva al objeto
        this.gameObject.SetActive(false);
    }

    public void RestartShield(int position, Vector3 scale, int lifeModifier) {
        // Metodo que reinicia el escudo
        this.gameObject.SetActive(true); // Se activa
        this.IsEnable = true; // Esta disponible 
        this.CanShoot = true; // puede disparar
        this.transform.localPosition = this.Positions[position]; // Coloca al escudo en la posicion deseada
        this.transform.localRotation = this.Rotations[position]; // Con la rotacion deseada
        //this.transform.localScale = new Vector3(1f, 1f, 1f); // Lo devuelve a su escala original por si estaba aumentado al momento de desactivarse
        this.transform.localScale = scale; // Pone al escudo en la escala deseada
        this.HitPoints = this.OriginalHitPoints * lifeModifier; // Regenera sus puntos de vida y los multiplica por el valor deseado
        this.ControlShieldLife(); // Llama a este metodo para recuperar la animacion
        //this.MyAnimator.SetTrigger("HighLife");
    }

    public void BigShield() {
        // Metodo que hace al escudo grande
        // Lo posiciona al frente con una escala de 2.5 y con la vida multiplicada por 2
        this.RestartShield(0, new Vector3(2.5f, 2.5f, 2.5f), 2); 
        //this.transform.localScale = new Vector3(2.5f, 2.5f, 1f);
        //this.HitPoints = this.OriginalHitPoints * 2;
    }

    public void NormalShield() {      
        // Metodo que vuelve al escudo a su estado original
        this.RestartShield(this.PositionCounter, new Vector3(1f, 1f, 1f), 1);
    }

    public void Shoot() {
        // Metodo que hace al escudo disparar, solo es llamado por PowerUps
        if (!this.IsEnable) {
            // Si estaba desactivado el powerUp lo activa en su modo original
            this.RestartShield(0, new Vector3(1f, 1f, 1f), 1);
        }
        
        // Por cada posicion de disparo llama al metodo que redirecciona las balas
        for (int i = 0; i < this.ShootsPositions.Count; i++) {
            this.RedirectShoot(this.ShootsPositions[i]);
        }

    }

    public void RedirectShoot(Transform shootpos) {
        // Como el escudo es hijo de la nave del Player y tiene una rotacion local y global hay que hacer una redireccion
        // Dependiendo de la posicion actual del escudo

        Quaternion rotation; // Variable que va a almacenar la rotacion

        if(this.PositionCounter == 0) {
            // Si esta al frente (posicion 0) la rotacion es la misma que la de cualquier bala de la nave
            rotation = this.Player.GetMyBulletRotation();
        }
        else if(this.PositionCounter == 1) {
            // Si el escudo esta a la derecha (posicion 1) hay que rotar las balas -90°
            rotation = this.Player.GetMyBulletRotation() * Quaternion.Euler(0f, 0f, -90f);
        }
        else if (this.PositionCounter == 2) {
            // Si el escudo esta abajo (posicion 2) hay que rotar las balas 180
            rotation = this.Player.GetMyBulletRotation() * Quaternion.Euler(0f, 0f, 180f);
        }
        else {
            // Si el escudo esta a la derecha (posicion 3) hay que rotar 90°
            rotation = this.Player.GetMyBulletRotation() * Quaternion.Euler(0f, 0f, 90f);
        }

        // Una vez que tenemos la rotacion podemos llamar al pool y pedirle que active la bala
        this.Pool.Spawn("ShieldBullet", shootpos.position, rotation);
    }

    #endregion


}
