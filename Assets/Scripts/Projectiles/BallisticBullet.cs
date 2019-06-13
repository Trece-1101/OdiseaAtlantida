//// Clase derivada/Hija de DestroyableProjectile, un tipo de proyectil afectado por la gravedad
/// Dependiendo de su velocidad inicial puede ser una caida libre, un tiro oblicuo o una caida impulsada

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallisticBullet : DestroyableProjectile {

    #region "Atributos"
    private bool Initial = true; // Variable de control para saber cual es el primer frame del update
    private Vector2 SpeedWithSideControl; // Vector que disparara dependiendo de la posicion del jugador
    #endregion

    #region "Componentes en cache"
    private Rigidbody2D Body; // Referencia al componente RigidBody
    private Asimov Player; // Referencia al Player
    #endregion

    private void Start() {
        // Primer metodo que se ejecuta cuando el objeto es "visto" en la jerarquia si no esta declarado "Awake"
        // Enlazamos los componentes en cache con sus respectivas referencias
        this.Player = FindObjectOfType<Asimov>();
        this.Body = this.GetComponent<Rigidbody2D>();

        //this.SetInitialSpeed(this.GetSpeed() * this.GetGameProg().GetScale());
    }

    public override void Update() {
        // Estas serian las formulas a utilizar si quisieramos emular la gravedad con un KinematicBody
        // Donde this.Gravity = gravedad del juego
        //this.SetSpeed(this.GetSpeed() + this.Gravity * Time.deltaTime);
        //this.transform.Translate(Vector2.right * ((this.GetSpeed() * Time.deltaTime) + ((this.Gravity * Mathf.Pow(Time.deltaTime, 2)) / 2)));


        if (this.Initial) {
            // Si estamos en el primer frame desde su activacion
            this.Initial = false; // marcamos el atributo como falso
            //this.Body.velocity = this.GetInitalSpeed(); // 
            this.SpeedWithSideControl = this.GetInitalSpeed(); // Asignamos al vector con control del lado la velocidad inicial
            if (!this.Player.GetIsAlive()) { return; }

            if (this.Player.transform.position.x < this.transform.position.x) {
                // Si el Player esta a la izquierda, invertimos el valor de la componente X del vector velocidad
                this.SpeedWithSideControl.x *= -1;
            }

            // Le damos al rigidbody la velocidad calculada
            this.Body.velocity = this.SpeedWithSideControl;
        }
        //Debug.Log($"{this.gameObject.name} -- {this.Body.velocity}");

    }

    public override void Die() {
        gameObject.SetActive(false);
        Initial = true;
    }

}
