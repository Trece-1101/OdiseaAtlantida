//// Clase Singleton que controla parametros y flujo del programa

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    #region "Atributos"
    [SerializeField] private int Score = 0; // Puntaje
    private int KillCount = 0; // Cantidad de enemigos destruidos
    private Vector2 PhysicSize = new Vector2(600f, 500f); // Tamaño fisico 500 metros ancho x 600 metros alto
    private Vector2 ScreenSize = new Vector2(12f, 10f); // Tamaño pantalla/camara viewport ancho x viewport alto
    private Vector2 Scale;
    private LevelLoader LevelLd;
    private float PlayTime;
    #endregion

    #region "Setters y Getters"
    public int GetScore() {
        return this.Score;
    }
    public void SetScore(int value) {
        this.Score = value;
    }

    public Vector2 GetScale() {
        return this.Scale;
    }
    public void SetScale(Vector2 value) {
        this.Scale = value;
    }

    public int GetKillCount() {
        return this.KillCount;
    }

    public float GetPlayTime() {
        return this.PlayTime;
    }

    #endregion

    #region "Metodos"
    private void Awake() {
        int gameSessionNumber = FindObjectsOfType<GameSession>().Length;
        if (gameSessionNumber > 1) {
            Destroy(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
        }

        this.LevelLd = FindObjectOfType<LevelLoader>();
        this.Scale = this.ScreenSize / this.PhysicSize; // 0,02 worldunits del viewport equivalen a 1 metro => 600 m = 12 WU // 500 m = 10WU
    }

    private void Update() {
        this.PlayTime += Time.deltaTime;
    }

    public void PlayerDead() {
        this.LevelLd.ResetLevel();
    }



    public void AddScore(int value) {
        // Metodo que aumenta el score del jugador
        this.Score += value;
        this.KillCount++; // suma un enemigo destruido
        //this.CheckNumberOfEnemies(); // chequea cuantos enemigos restan
    }

    public void SubstractScore(int value) {
        // Metodo que quita score del jugador
        this.Score -= value;
        if(this.Score <= 0) {
            this.Score = 0;
        }
    }

    //public void AddToKillCount() {
    //    this.KillCount++;
    //}

    //private void ResetLevel() {
    //    string currentScene = SceneManager.GetActiveScene().name;
    //    SceneManager.LoadScene(currentScene);
    //    //Destroy(this.gameObject);
    //}

    public void ResetGame() {
        Destroy(gameObject);
    }

    #endregion

}
