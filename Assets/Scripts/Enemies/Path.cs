//// Clase que controla el "camino" que siguen los enemigos y su movimiento punto a punto

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    #region "Atributos"
    private List<Transform> WayPoints; // Lista de puntos por donde va pasando el enemigo
    private int WayPointIndex = 0; // Indice de la lista de puntos
    #endregion

    #region "Componentes en Cache"
    private Wave Wave; // Referencia a la Oleada de enemigos
    private Enemy Enemy; // Referencia al objeto enemigo
    private GameProgram GameProg; // Referencia al GameProgram
    #endregion

    #region "Auxiliares"
    private float MoveSpeed; // Velocidad de movimiento
    private float Scale; // Escala del programa
    #endregion

    #region "Setters y Getters"
    public void SetWave(Wave wave) {
        this.Wave = wave;
    }
    #endregion

    #region "Metodos"
    private void Awake() {
        this.GameProg = FindObjectOfType<GameProgram>();
        this.Scale = this.GameProg.GetScale().x;
    }

    private void Start() {
        // Primer metodo que se ejecuta cuando el objeto es "visto" en la jerarquia
        // Enlazamos los componentes en cache con sus respectivas referencias

        // Esta clase se va a instaciar dentro de la clase Wave todo dentro del Objeto de jerarquia "EnemySpawner"
        // Por lo tanto aca le decimos que "Enemigo" va a ser del tipo "Enemy" dentro de su padre
        this.Enemy = gameObject.GetComponentInParent(typeof(Enemy)) as Enemy;

        this.Enemy.CanShoot = false; // Le quitamos la clase de disparar balas y misiles (para que no disparen desde afuera de camara
        this.Enemy.CanShootMissile = false;

        this.WayPoints = this.Wave.GetPathPrefab(); // Tomamos la lista de puntos a utilizar desde la oleada
        this.transform.position = this.WayPoints[this.WayPointIndex].transform.position; // ubicamos al enemigo en el punto de partida (indice 0)
        this.Wave.SetMoveSpeed(this.Enemy.GetVelocity()); // le pasamos la velocidad del enemigo elegido
        this.MoveSpeed = this.Wave.GetMoveSpeed(); // tomamos esa velocidad para usarla con el metodo MoveFoward
        //Debug.Log(this.MoveSpeed);
    }

    private void Update() {
        this.MoveInPath(); // En cada cuadro llamamos al metodo que nos mueve en el camino
    }
    
    private void MoveInPath() {
        // Metodo que hace que el enemigo se mueva en el camino
        if (this.WayPointIndex < this.WayPoints.Count) {
            // Mientras el punto  que busquemos no sea el ultimo
            this.MoveToPoint(this.WayPointIndex); // llamamos al metodo que nos traslada

            if (this.WayPointIndex == 2) {
                //Debug.Log("pos2");
                // recien en el segundo punto los enemigos empiezan a disparar
                //enemy.SetIsVulnerable(true);
                //this.Enemy.CanShoot = true;
                //this.Enemy.CanShootMissile = true;
            }
        }
        else {
            // si el punto es el ultimo, el enmigo regresa al indice 1 (segundo punto)
            this.WayPointIndex = 1;
            this.MoveToPoint(this.WayPointIndex); // trasladamos al enemigo
        }
    } 

    private void MoveToPoint(int index) {
        // este es el metodo que traslada al enemigo
        // creamos una posicion target
        var targetPosition = this.WayPoints[index].transform.position;     
        // nos movemos al punto siguiente (desde - hasta -> d = v*t)
        this.transform.position = Vector2.MoveTowards(transform.position, targetPosition, this.MoveSpeed * this.Scale * Time.deltaTime );
        
        // Si nuestra posicion es igual a la del punto objetivo, pasamos a la siguiente posicion
        if (this.transform.position == targetPosition) {
            this.WayPointIndex++;
        }
    }
    #endregion

}

