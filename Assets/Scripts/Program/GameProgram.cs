using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgram : MonoBehaviour
{
    public static GameProgram instance = null;

    [SerializeField] GameObject LevelCompleteText = null;

    #region "Atributos"
    private float LeftBorder;
    private float RightBorder;
    private float UpBorder;
    private float DownBorder;
    private float padding;
    private float scrollSpeed;

    private int Score;
    private int KillCount;
    private int TotalEnemies;
    private int LeftEnemies;
    #endregion

    #region "Setters/Getters"
    public float GetLeftBorder() {
        return this.LeftBorder;
    }
    public void SetLeftBorder(float value) {
        this.LeftBorder = value;
    }

    public float GetRightBorder() {
        return this.RightBorder;
    }
    public void SetRightBorder(float value) {
        this.RightBorder = value;
    }

    public float GetUpBorder() {
        return this.UpBorder;
    }
    public void SetUpBorder(float value) {
        this.UpBorder = value;
    }

    public float GetDownBorder() {
        return this.DownBorder;
    }
    public void SetDownBorder(float value) {
        this.DownBorder = value;
    }

    public float GetScrollSpeed() {
        return this.scrollSpeed;
    }
    public void SetScrollSpeed(float value) {
        this.scrollSpeed = value;
    }

    public int GetScore() {
        return this.Score;
    }
    public void SetScore(int value) {
        this.Score = value;
    }
    #endregion

    #region "Referencias en Cache"
    private Asimov asimov;
    private CrossHair crossHair;
    private EnemySpawner enemySpawner;
    #endregion

    #region "Metodos"
    private void Awake() {
        SetScrollSpeed(-1.5f);


        if (instance == null) {
            instance = this;
        }
        else if(instance == this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        LevelCompleteText.SetActive(false);

        asimov = FindObjectOfType<Asimov>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        

        crossHair = FindObjectOfType<CrossHair>();
        Cursor.visible = false;
        SetUpBorders();
    }

    private void Update() {
        if (asimov.GetIsAlive()) {
            crossHair.transform.position = Input.mousePosition;
        }
        else {
            Cursor.visible = true;
        }        
        
    }
    
    
    private void SetUpBorders() {
        Camera mainCamera = Camera.main;
        padding = 0.8f;
        this.LeftBorder = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        this.RightBorder = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        this.UpBorder = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).y + (padding + 0.6f);
        this.DownBorder = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0)).y - padding;
    }

    public void AddScore(int value) {
        this.Score += value;
        this.KillCount++;
        CheckNumberOfEnemies();
    }

    public void AddEnemyToCount() {
        this.TotalEnemies++;
        CheckNumberOfEnemies();
    }

    private void CheckNumberOfEnemies() {
        this.LeftEnemies = this.TotalEnemies - this.KillCount;
        //Debug.Log($"enemigos en pantalla {this.LeftEnemies}");
        if(this.LeftEnemies <= 0) {

        }
    }

    public void SubstractScore(int value) {
        this.Score -= value;
    }

    public void ResetGame() {
        Destroy(gameObject);
    }


    #endregion
}
