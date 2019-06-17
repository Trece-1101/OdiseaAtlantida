//// Clase Padre que describe a todos los objetos del tipo proyectil (balas, misiles, bombas), es abstracta, no se instancia

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Proyectile : MonoBehaviour
{
    #region "Atributos Serializados"
    [SerializeField] private Vector2 Speed; // Velocidad vectorial del proyectil
    private Vector2 InitialSpeed; // Velocidad inicial para almacenar, muy util en proyectiles quo no tienen MRU
    #endregion

    #region "Atributos"
    private float LifeTime = 5f; // Tiempo que pasa hasta que se desactiva el proyectil
    #endregion

    #region "Referencias en Cache"
    private DamageControl DamageCtrl; // Referencia al componente DamageControl que va a almacenar el daño que genera el proyectil
    private GameSession GameSessionControl; // Atributo que enlaza al Game Program que es el encargado de las funciones generales del juego (es un singleton)
    #endregion

    #region "Setters/Getters"
    public Vector2 GetSpeed() {
        return this.Speed;
    }
    public void SetSpeed(Vector2 value) {
        this.Speed = value;
    }

    public float GetLifeTime() {
        return this.LifeTime;
    }
    public void SetLifeTime(float value) {
        this.LifeTime = value;
    }
    
    public Vector2 GetInitalSpeed() {
        return this.InitialSpeed;
    }
    public void SetInitialSpeed(Vector2 value) {
        this.InitialSpeed = value;
    }

    public DamageControl GetDamageCtrl() {
        return this.DamageCtrl;
    }
    public void SetDamageCtrl(DamageControl value) {
        this.DamageCtrl = value;
    }

    public GameSession GetGameSessionControl() {
        return this.GameSessionControl;
    }
    public void SetGameSessionControl(GameSession value) {
        this.GameSessionControl = value;
    }

    #endregion

    #region "Metodos"
    public virtual void Awake() {
        // Primer metodo que se ejecuta cuando el objeto es "visto" en la jerarquia
        // Enlazamos los componentes en cache con sus respectivas referencias
        this.DamageCtrl = GetComponent<DamageControl>();
        this.GameSessionControl = FindObjectOfType<GameSession>();

    }

    public abstract void Update(); // Hacemos que el update sea abstracto, ya que cada proyectil implementa distintas fisicas
    
    private void OnDisable() {
        // Si se desactiva el objeto cancelamos las invocaciones posibles que puedan haber
        // esto es porque hay muchas maneras distintas que el proyectil puede desactivarse
        CancelInvoke();
    }

    public virtual void OnEnable() {
        // Cuando se activa el objeto invocamos su metodo morir
        Invoke("Die", GetLifeTime());
    }

    public virtual void Die() {
        // Metodo que controla la muerte (desactivacion) del objeto
        if (gameObject.activeSelf) {
            // Al haber muchas formas que se puede desactivar hacemos un doble chequeo, solo desactivarlo si esta activado
            gameObject.SetActive(false);
        }
        // Retornamos la velocidad a su vel inicial, fundamental para proyectiles con mov distinto al MRU     
        this.Speed = this.InitialSpeed;    
    }
    #endregion



}
