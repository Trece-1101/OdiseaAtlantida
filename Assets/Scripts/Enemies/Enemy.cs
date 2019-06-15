//// Clase Derivada/Hija de "SHIP" que describe a los objetos del tipo "Naves Enemigas".
/// A su vez clase Padre de enemigos con distintas particularidades.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Ship
{
    #region "Atributos Serializados"
    [Header("Especificos")]
    [SerializeField] private int Reward; // Recompensa que genera destruir al enemigo
    [SerializeField] [Range (0f, 1f)] private float PowerUpChance = 0f; // Chance que da el enemigo de spawnear un powerUp
    #endregion        

    #region "Referencias en Cache"
    private Asimov Player; // Referencia al Player
    #endregion

    #region "Auxiliares"
    [SerializeField] private float timeBtwBulletS = 1f; // Tiempo entre disparo de balas, serializado porque para cada instancia/tipo de nave cambia
    [SerializeField] private float timeBtwMissileS = 1f; // Tiempo entre disparo de misiles, serializado porque para cada instancia/tipo de nave cambia
    [SerializeField] private bool canShootMissile = false; // Si este enemigo particular puede disparar misiles
    List<string> Explodes = new List<string>() { "EnemyExplosion", "EnemyImplosion" }; // Lista que contiene tags de los tipos de explosiones que puede generar el enemigo al morir
    #endregion

    #region "Setters/Getters"
    public Asimov GetPlayer() {
        return this.Player;
    }
    public void SetPlayer(Asimov value) {
        this.Player = value;
    }

    public int GetReward() {
        return this.Reward;
    }
    public void SetReward(int value) {
        this.Reward = value;
    }
    #endregion

    #region "Metodos"
    public override void Awake() {
        base.Awake();

        // Enlazamos los componentes en cache con sus respectivas referencias
        this.Player = FindObjectOfType<Asimov>();

        // Usamos los atributos axuiliares serializados para establecer los atributos particulares de esta instancia
        this.CanShootMissile = canShootMissile;
        this.SetTimeBetweenBulletShoots(timeBtwBulletS);
        this.SetTimeBetweenMissileShoots(timeBtwMissileS);

        this.RemainTimeForShootBullet = this.GetTimeBetweenBulletShoots();
        this.RemainTimeForShootMissile = this.GetTimeBetweenMissileShoots();

        this.GetGameProg().AddEnemyToCount(); // Metodo del GameProgram que añade un enemigo al conteo de enemigos
    }      

    public virtual void Update() {
        // Metodo que se ejecuta frame a frame
        CheckRotation(); // Metodo para controlar la rotacion
        Shoot(); // Metodo para disparar
    }

    public override void CheckRotation() {
        // Metodo que controla la rotacion dependiendo de distintos factores

        Dictionary<string, Quaternion> rotations; // Diccionario que contiene rotaciones (aca se almacena los valores devueltos por el metodo Rotate)

        // Si el jugador es destruido no tiene sentido rotar "hacia", genera error, se hace un return
        if (!this.Player.GetIsAlive() && this.Player != null) {
            return;
        }

        // Si el jugador esta clonado (es un tipo de powerUp que genera un clon en un punto dado de la pantalla) el enemigo va a apuntar
        // a un sector delimitado random (no al player real)
        if (this.Player.GetIsCloned()) {
            var targetClone = new Vector3(Random.Range(-1f, 1f), Random.Range(-3.5f, -2.5f), 0f); // vector random
            rotations = this.Rotate(targetClone - this.transform.position, 90, 0); // pedimos al metodo rotate nuestra rotacion para apuntar
        }
        else {
            // si el jugador no esta clonado pedimos a Rotate nuestra rotacion pasandole el vector target = Player
            // la compensacion del sprite del enemigo es 90° (mira al Norte) y la de la bala es 0° (mira el Este)
            rotations = this.Rotate(this.Player.transform.position - this.transform.position, 90, 0);
        }

        // Asignamos las rotaciones correspondientes
        this.SetMyRotation(rotations["rotation_ship"]);
        this.SetMyBulletRotation(rotations["rotation_bullet"]);
        this.transform.rotation = rotations["transform_rotation"];
    }

    public override void Shoot() {
        // Metodo que controla el disparo del enemigo
        // Si el enemigo puede disparar y el contador de tiempo llego a 0 => dispara
        // Tambien se controla si el enemigo tiene puntos de disparo. Si no los tuviera en el metodo "ShootBullet" no pasaria nada
        // Porque iteraria en un for de 0 a 0. Pero se controla aca para evitar tener que entrar al IF y ser mas eficiente
        if (this.RemainTimeForShootBullet <= 0 && this.CanShoot && this.GetBulletShootPoints().Count > 0) {
            this.ShootBullet(); // Metodo en la clase padre
            this.ShootMicroBullet();
            this.PlayShootSFX(this.GetShootBulletSFX(), this.transform.position, 3f); // Metodo en la clase padre

            this.RemainTimeForShootBullet = this.TimeBetweenBulletShoots; // Regresamos el contador a su valor original
        }
        else {
            this.RemainTimeForShootBullet -= Time.deltaTime; // Si aun no es 0 descontamos un deltaTime
        }

        // Igual que con el disparo de balas pero para misiles
        if (this.RemainTimeForShootMissile <= 0 && this.CanShootMissile && this.GetMissileShootPoints().Count > 0) {
            ShootMissile();
            PlayShootSFX(this.GetShootMissileSFX(), this.transform.position, 3f);


            this.RemainTimeForShootMissile = this.GetTimeBetweenMissileShoots();
        }
        else {
            this.RemainTimeForShootMissile -= Time.deltaTime;
        }

    }

    public override void ControlOtherCollision(Collider2D collision) {
        // Metodo que controla collisiones con objetos que no poseen un componenete DamageControl
        // Si el objeto con el que se colisiono es el Player => se destruye el enemigo
        if(collision.gameObject.tag == "Player" && this.tag != "FinalEnemy") {
            Die();
        }
    }

    public override void Die() {
        // Metodo de muerte del enemigo

        Destroy(this.gameObject); // Destruye el objeto
                      
        this.PlayExplosion(); // Metodo que muestra la explosion al destruirse        
    }

    protected void PlayExplosion() {
        // Metodo que muestra la explosion        
        int expIndex = Random.Range(0, this.Explodes.Count); // se elige random entre los tipos de explosion de la lista
        string exp = Explodes[expIndex]; // se asigna a una variable string (el tag) el nombre con ese index
        this.GetPool().Spawn(exp, this.transform.position, Quaternion.identity); // se le pide al pool que muestre la explosion
        AudioSource.PlayClipAtPoint(this.GetDeathSFX(), this.GetMyMainCamera().transform.position, 0.6f); // Se utiliza el sonido de muerte del enemigo
    }

    public override void PlayImpactSFX() {
        // Metodo que controla el sonido y explosion al ser impactado por un proyectil
        this.GetPool().Spawn("ProyectileExplosion", this.transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(this.GetHurtSFX(), this.transform.position, 3f);
    }
    #endregion

    #region "Comportamientos PowerUp"

    private void OnDestroy() {
        // OnDestroy es un metodo de unity que se llama cuando un objeto es destruido
        this.GetGameProg().AddScore(this.GetReward()); // Le agregamos al Score el Reward del enemigo
        this.PowerUpRoulette(); // Metodo para controlar spawn de PowerUps
    }

    protected void PowerUpRoulette() {
        // Metodo para controlar spawn de PowerUps

        var powerUp = this.PickRandomPowerUp(); // Llamamos a un metodo que devuelve un string con el tag de un powerUp
        var number = Random.Range(0f, 1f); // Numero Random entre 0 y 1

        if (powerUp == null) { return; } // Si no hay powerUps esto devolvera un null, por lo tanto no vale la pena seguir

        if(number <= this.PowerUpChance) {
            // Si el numero random es menor o igual a la chance del powerUp entonces spawneamos ese powerUp elegido al azar
            // Esto es probabilidad porque si nuestra chances de spawnear son "0.1" quiere decir que hay 10% de posibilidades
            // de sacar un numero random por debajo de ese valor. Si fuera "0.8" la posibilidad seria del 80%
            this.GetPool().Spawn(powerUp, this.transform.position, Quaternion.identity); // Spawn del powerUp
        }
        
    }

    private string PickRandomPowerUp() {
        // Metodo que Pickea el powerUp
        if(!(GetPowerUpsNames().Count > 0)) { return null; } // Si la lista de powerUps esta vacia, retornamos un null
            int powerUpIndex = Random.Range(0, this.GetPowerUpsNames().Count); // random entre los indices de la lista
            return this.GetPowerUpsNames()[powerUpIndex]; // Devolvemos el tag del powerUp de ese indice
    }

    #endregion


}
