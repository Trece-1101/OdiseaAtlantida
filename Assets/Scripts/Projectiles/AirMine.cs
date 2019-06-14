//// Clase derivada/Hija de Proyectile, un tipo de proyectil que es una mina aerea, vuela hacia un punto/area y al llegar explota con radio dado

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirMine : Proyectile
{
    #region "Atributos"
    private Vector2 Target; // Vector posicion objetivo (area, no enemigo)
    private bool Shooted; // Atributo que controla si ya fue disparado, usado para los triggers
    private bool OnTarget; // Atributo que controla si ya llego a su destino, usado para los triggers
    #endregion

    #region "Componentes en cache"
    private Animator MyAnimator; // Referencia al componente animator del objeto
    #endregion

    #region "Auxiliares"
    private float speed;
    private float scale;
    #endregion

    #region "Setters y Getters"
    public void SetTarget(Vector2 value) {
        this.Target = value;
    }

    public bool GetShooted() {
        return this.Shooted;
    }
    public void SetShooted(bool value) {
        this.Shooted = value;
    }

    public bool GetOnTarget() {
        return this.OnTarget;
    }
    public void SetOnTarget(bool value) {
        this.OnTarget = value;
    }
    #endregion

    private void Start() {
        // Primer metodo que se ejecuta si no esta declarado "Awake"
        // Enlazamos los componentes en cache con sus respectivas referencias
        this.MyAnimator = GetComponentInChildren<Animator>();

        this.speed = this.GetSpeed().x;
        this.scale = this.GetGameProg().GetScale().x;
    }


    public override void Update() {
        // Controlamos la fisica cuadro a cuadro
        if (this.Shooted) {
            // Si la mina esta disparada en cada cuadro vamos a trasladarla
            this.transform.position = Vector2.MoveTowards(transform.position, Target, this.speed * this.scale * Time.deltaTime);
            // Si la posicion del centro de la mina coincide con la del vector posicion objetivo
            if(this.transform.position.x == this.Target.x && this.transform.position.y == this.Target.y) {
                // Estamos en el objetivo y debemos explotar
                this.OnTarget = true;
                this.Explode();
            }
            else {
                this.OnTarget = false;
            }
        }
    }

    public void Explode() {
        // Activa la animacion de explotar y crea un rango de explosion de 6x6
        this.MyAnimator.SetTrigger("explode");
        this.transform.localScale = new Vector3(6f, 6f, 6f);
        Invoke("SetInactive", 2f);
    }

    private void SetInactive() {
        if (gameObject.activeSelf) {
            this.gameObject.SetActive(false);
        }
    }

    public override void OnEnable() {
        //this.MyAnimator.Play();
        this.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
    }
   

    private void OnTriggerEnter2D(Collider2D collision) {
        this.Explode();
    }


}
