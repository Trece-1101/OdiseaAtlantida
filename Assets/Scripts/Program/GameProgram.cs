using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgram : MonoBehaviour
{
    public static GameProgram instance = null;

    [SerializeField] GameObject LevelCompleteText = null;
    [SerializeField] GameObject LevelDestroyedText = null;

    #region "Atributos"
    private float LeftBorder;
    private float RightBorder;
    private float UpBorder;
    private float DownBorder;
    private float padding;
    private float scrollSpeed;

    private Vector2 PhysicSize = new Vector2(600f, 500f); // 500 metros ancho x 600 metros alto
    private Vector2 ScreenSize = new Vector2(12f, 10f); // viewport ancho x viewport alto
    private Vector2 Scale;

    private int Score;
    private int KillCount;
    private int TotalEnemies;
    private int LeftEnemies;

    private float TimeToWait = 2f;    
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

    public Vector2 GetScale() {
        return this.Scale;
    }
    public void SetScale(Vector2 value) {
        this.Scale = value;
    }
    #endregion

    #region "Referencias en Cache"
    private Asimov asimov;
    private CrossHair crossHair;
    private EnemySpawner EnemySpawner;
    private LevelManager levelManager;
    #endregion

    #region "Metodos"
    private void Awake() {
        SetScrollSpeed(-2.5f);
        Scale = this.ScreenSize / this.PhysicSize; // 0,02 worldunits del viewport equivalen a 1 metro => 600 m = 12 WU // 500 m = 10WU

        if (instance == null) {
            instance = this;
        }
        else if (instance == this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        LevelCompleteText.SetActive(false);
        LevelDestroyedText.SetActive(false);

        asimov = FindObjectOfType<Asimov>();
        EnemySpawner = FindObjectOfType<EnemySpawner>();
        levelManager = FindObjectOfType<LevelManager>();

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
            this.Lose();
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

    public void SubstractScore(int value) {
        this.Score -= value;
    }

    private void CheckNumberOfEnemies() {
        this.LeftEnemies = this.TotalEnemies - this.KillCount;
        //Debug.Log($"enemigos en pantalla {this.LeftEnemies}");
        if (this.LeftEnemies <= 0 && this.EnemySpawner.GetIsLastFormation()) {
            StartCoroutine(LevelCompleted());
        }
    }

    private void Lose() {
        LevelDestroyedText.SetActive(true);
        Invoke("StopTime", 3f);
    }

    private void StopTime() {
        Time.timeScale = 0;
    }

    IEnumerator LevelCompleted() {
        LevelCompleteText.SetActive(true);

        // agregar audio source
        //GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(this.TimeToWait);

        //cargar proxima escena
        //levelManager.LoadLevel();
    }

    public void ResetGame() {
        Destroy(gameObject);
    }


    #endregion
}
