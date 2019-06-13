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

        this.WayPoints = this.Wave.GetPathPrefab();
        this.transform.position = this.WayPoints[this.WayPointIndex].transform.position;
        this.Wave.SetMoveSpeed(this.Enemy.GetVelocity());
        this.MoveSpeed = this.Wave.GetMoveSpeed();
        //Debug.Log(this.MoveSpeed);
    }

    private void Update() {
        this.MoveInPath();
    }

    public void SetWave(Wave wave) {
        this.Wave = wave;
    }


    private void MoveInPath() {
        if (this.WayPointIndex < this.WayPoints.Count) {
            MoveToPoint(this.WayPointIndex);
            if (this.WayPointIndex == 2) {
                //enemy.SetIsVulnerable(true);
                this.Enemy.CanShoot = true;
                this.Enemy.CanShootMissile = true;
            }
        }
        else {
            this.WayPointIndex = 1;
            MoveToPoint(this.WayPointIndex);
        }
    } 

    private void MoveToPoint(int index) {
        var targetPosition = this.WayPoints[index].transform.position;     
        this.transform.position = Vector2.MoveTowards(transform.position, targetPosition, this.MoveSpeed * this.Scale * Time.deltaTime );
        

        if (this.transform.position == targetPosition) {
            this.WayPointIndex++;
        }
    }
          
   
}

