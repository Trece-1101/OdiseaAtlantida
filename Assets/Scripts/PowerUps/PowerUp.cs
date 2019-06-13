//// Clase Padre que describe a todos los objetos de tipo PowerUp, es abstracta, no se instancia

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    #region "Atributos"
    private Vector2 Speed; // Velocidad vectorial del PowerUp (su representacion grafica)
    private Vector2 SpeedChange; // Cambio de velocidad ante cada rebote
    private bool FirstContact = false; // Variable que controla si el objeto ya colisiono contra algun objeto
    private float TimeToDie = 5f; // Variable que controla el tiempo para que se desactive el objeto (su representacion grafica) tras primera colision

    private float CoolTime = 6f; // Tiempo en el que el powerUp se enfria (se desactiva) al ser activado
    #endregion

    #region "Componentes en Cache"
    private Rigidbody2D MyBody; // Referencia al componente RigidBody
    private ObjectPool Pool; // Referencia al Pool que contiene los objetos instanciados
    private Asimov Player; // Referencia al Player
    #endregion

    #region "Setters y Getters"
    public string GetName() {
        return this.name;
    }
    public void SetName(string value) {
        this.name = value;
    }

    public Vector2 GetSpeed() {
        return this.Speed;
    }
    public void SetSpeed(Vector2 value) {
        this.Speed = value;
    }

    public Vector2 GetSpeedChange() {
        return this.SpeedChange;
    }
    public void SetSpeedChange(Vector2 value) {
        this.SpeedChange = value;
    }

    public Rigidbody2D GetMyBody() {
        return this.MyBody;
    }
    public void SetMyBody(Rigidbody2D value) {
        this.MyBody = value;
    }

    public bool GetFirstContact() {
        return this.FirstContact;
    }
    public void SetFirstContact(bool value) {
        this.FirstContact = value;
    }

    public Asimov GetAsimov() {
        return this.Player;
    }
    public void SetAsimov(Asimov value) {
        this.Player = value;
    }

    public float GetCoolTime() {
        return this.CoolTime;
    }
    public void SetCoolTime(float value) {
        this.CoolTime = value;
    }

    public ObjectPool GetPool() {
        return this.Pool;
    }
    public void SetPool(ObjectPool value) {
        this.Pool = value;
    }
    #endregion


    private void Start() {
        //this.Pool = FindObjectOfType<ObjectPool>();
        // Primer metodo que se ejecuta cuando el objeto es "visto" en la jerarquia si no esta declarado "Awake"
        // Enlazamos los componentes en cache con sus respectivas referencias
        this.Pool = ObjectPool.Instance;
        this.Player = FindObjectOfType<Asimov>();        

        // Llamamos al metodo que genera una velocidad random cuando se instancia el objeto, queda guardado en el pool con velocidad
        // Incluso antes de su primera activacion
        this.SetSpeedChange(); 
    }

    public abstract void MakeYourMagic(); // Metodo abstracto que tiene que implementarse en cada subclase

    private void SetSpeedChange() {
        // Este metodo genera una velocidad aleatoria y los cambios de velocidad
        this.MyBody = GetComponent<Rigidbody2D>(); // Referencia al rigidbody
        this.Speed = this.RandomSpeed(); // llamada al metodo para elegir velocidad aleatoria
        this.SpeedChange = this.Speed / 2; // le asignamos a la variable de cambio de velocidad la mitad de la velocidad aleatoria
        this.MyBody.velocity = this.Speed; // le damos al rigidbody la velocidad
    }

    private Vector2 RandomSpeed() {
        var speed = Vector2.zero; // variable local de velocidad en 0 para forzar entrada al while

        while (speed.x < 1 && speed.x > -1) {
            // con esto nos aseguramos que la velocidad en X sea mayor a 4 o menor a -4
            speed.x = Random.Range(-4f, 4f);
        }
        while (speed.y < 1 && speed.y > -1) {
            // con esto nos aseguramos que la velocidad en y sea mayor a 3 o menor a -3
            speed.y = Random.Range(-3f, 3f);
        }
        
        return speed; // devolvemos el vector velocidad calculado
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // Metodo que calcula la colision del powerUP (el elemento visible) contra no-triggers

        if (!FirstContact) {
            // Si aun no habia realizado un primer contacto lo hace ahora
            // se cambia el valor de la variable y se invoka al metodo de morir dentro de TimeToDie segundos
            this.FirstContact = true;
            Invoke("Die", this.TimeToDie);
        }

        // Por cada colision tomamos al vector velocidad y lo reflejamos por el vector normal del objeto colisionado
        this.Speed = Vector2.Reflect(this.Speed, collision.contacts[0].normal);
        // Tambien hay que reflejar el vector velocidad de cambio para que al sumar los vectores siempre esten en el mismo sentido/direccion
        this.SpeedChange = Vector2.Reflect(this.SpeedChange, collision.contacts[0].normal);
        // Aumentamos el valor del vector de cambio de velocidad (que al iniciar es la mitad de la velocidad) por un 20%
        // Es decir que por cada contacto aumenta un 20%
        this.SpeedChange *= 1.2f;

        // le asignamos al rigidBody una velocidad igual al vector velocidad + vector cambio de velocidad
        // Vt = V + VC
        this.MyBody.velocity = this.Speed + this.SpeedChange;

        //Debug.Log("---- Contacto ----");
        //Debug.Log(this.Speed);
        //Debug.Log(this.SpeedChange);
        //Debug.Log($"Velocidad PowerUp {this.MyBody.velocity}");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        // Metodo que controla la colision contra triggers en la capa de colision (solo el player)
        this.PickUp(); // Llamada al metodo de PickUp
    }
    
    private void PickUp() {
        // Metodo que se activa cuando el objeto visible colisiona contra el player
        // crea el efecto de particulas
        this.Pool.Spawn("ParticleAnimation", this.transform.position, Quaternion.identity);

        this.Player.SetHasPowerUp(true); // Le dice al player que tiene un powerUp para activar
        this.Player.SetPowerUpType(this); // Le pasa al atributo PowerUp su tipo
                
        this.Die(); // Llama al metodo morir instantaneamente
    }


    private void OnEnable() {
        // Como esta en un Pool se va a activar y desactivar muchas veces
        // Para que no tenga siempre la misma velocidad que en su instanciacion generamos una velocidad random
        SetSpeedChange();        
    }    

    protected void Die() {
        // Metodo que desactiva al powerUp (su representacion grafica)
        this.FirstContact = false; // Por si hicimos el PickUp antes de un primer contacto

        if (this.gameObject.activeSelf) {
            // Como hay 2 instancias para desactivarlo (PickUp o por tiempo) 
            // Nos aseguramos de pedir su desactivacion solo si esta activo
            this.gameObject.SetActive(false);
        }
    }


}
