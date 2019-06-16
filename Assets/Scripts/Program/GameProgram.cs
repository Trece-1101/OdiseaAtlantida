//// Clase Singleton que controla parametros y flujo del programa


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgram : MonoBehaviour
{
    public static GameProgram Instance = null;

    [SerializeField] GameObject LevelCompleteText = null; // Banner de nivel completado
    [SerializeField] GameObject LevelDestroyedText = null; // Banner de derrota
    [SerializeField] List<GameObject> Spawners = null;

    #region "Atributos"
    private float LeftBorder; // Borde izquierdo maximo donde puede moverse el jugador
    private float RightBorder; // Borde derecho maximo donde puede moverse el jugador
    private float UpBorder; // Borde superior maximo donde puede moverse el jugador
    private float DownBorder; // Borde inferior maximo donde puede moverse el jugador
    private float Padding; // distancia de seguridad (por el tamaño de la nave)
    private float ScrollSpeed; // velocidad de movimiento del escenario

    private Vector2 PhysicSize = new Vector2(600f, 500f); // Tamaño fisico 500 metros ancho x 600 metros alto
    private Vector2 ScreenSize = new Vector2(12f, 10f); // Tamaño pantalla/camara viewport ancho x viewport alto
    private Vector2 Scale;

    private int Score = 10000; // Puntaje
    private int KillCount; // Cantidad de enemigos destruidos
    private int TotalEnemies; // Total de enemigos Spawneados
    private int LeftEnemies; // Enemigos restantes en juego

    private float TimeToWait = 2f; // Tiempo de espera
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
        return this.ScrollSpeed;
    }
    public void SetScrollSpeed(float value) {
        this.ScrollSpeed = value;
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
    private Asimov Player; // Referencia al objeto de tipo player
    private CrossHair CrossHair; // Referencia a la mira del player (es una Imagen de un canvas)
    private EnemySpawner EnemySpawner; // Referencia a la clase que spawnea enemigos
    private LevelLoader LevelLoader; // Referencia al objeto que controla la carga del nivel
    #endregion

    #region "Metodos"
    private void Awake() {
        // Primer metodo que se ejecuta

        this.SetScrollSpeed(-3.5f); // Velocidad de desplazamiento de la camara
        this.Scale = this.ScreenSize / this.PhysicSize; // 0,02 worldunits del viewport equivalen a 1 metro => 600 m = 12 WU // 500 m = 10WU

        if (Instance == null) {
            // Si no hay instancia (primera vez que se corre el script), instanciamos
            Instance = this;
        }
        else if (Instance == this) {
            // SI ya existe una instancia destruimos la instancia que se intenta crear
            Destroy(gameObject);
        }
        // Forzamos a que no destruya la instancia anteriormente creada (sacar esto puede generar un bug)
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        // Desactivamos los banners de victoria/derrota
        this.LevelCompleteText.SetActive(false);
        this.LevelDestroyedText.SetActive(false);

        // Enlazamos los componentes en cache con sus respectivas referencias
        this.Player = FindObjectOfType<Asimov>();        

        if(this.Spawners.Count > 0) {
            // elegimos un enemyspawner random de la lista (asi los niveles nunca seran iguales)
            var spawnThis = Random.Range(0, this.Spawners.Count);            
            this.Spawners[spawnThis].SetActive(true);
            this.EnemySpawner = FindObjectOfType<EnemySpawner>();
            //Debug.Log(this.EnemySpawner.name);
        }     

        this.LevelLoader = FindObjectOfType<LevelLoader>();
        this.CrossHair = FindObjectOfType<CrossHair>();

        // Hacemos que el cursor (la flechita) desaparezca
        Cursor.visible = false;

        // Llamamos al metodo que establece los bordes
        this.SetUpBorders();
    }

    
    private void SetUpBorders() {
        // En este metodo establecemos hasta donde se puede mover la nave del Player
        // Estos valores lo va a usar en su movimiento junto con la funcion "clamp"
        Camera mainCamera = Camera.main; // Referencia a la camara principal
        this.Padding = 0.8f; // Padding

        this.LeftBorder = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + this.Padding; // Borde izquierdo 
        this.RightBorder = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - this.Padding; // Borde derecho
        this.UpBorder = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).y + (this.Padding + 0.6f); // Borde superior
        this.DownBorder = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0)).y - this.Padding; // Borde inferior
    }


    private void Update() {
        // Si el player sigue vivo
        if (this.Player != null || this.Player.GetIsAlive()) {
            this.CrossHair.transform.position = Input.mousePosition; // Mueve la mira a la posicion del mouse
        }
        else {            
            Cursor.visible = true;
            this.Lose(); // Llama al metodo de derrota
        }                
    }
    
    private void Lose() {
        LevelDestroyedText.SetActive(true); // Activa el banner de derrota
        //Invoke("StopTime", this.TimeToWait); // Llama al metodo que detiene el tiempo
        this.LevelLoader.RestartScene();
    }

    private void StopTime() {
        Time.timeScale = 0;
    }

    public void AddEnemyToCount() {
        // Metodo que suma un enemigo al conteo (cuando Spawnea)
        this.TotalEnemies++;
        this.CheckNumberOfEnemies();
    }

    public void AddScore(int value) {
        // Metodo que aumenta el score del jugador
        this.Score += value;
        this.KillCount++; // suma un enemigo destruido
        this.CheckNumberOfEnemies(); // chequea cuantos enemigos restan
    }

    public void SubstractScore(int value) {
        // Metodo que quita score del jugador
        this.Score -= value;
    }

    private void CheckNumberOfEnemies() {
        // Metodo que chequea la cantidad de enemigos vivos que quedan
        this.LeftEnemies = this.TotalEnemies - this.KillCount; // Enemigos_restantes = Enemigos_totales_spawneados - Enemigos_totales_destruidos
        //Debug.Log($"enemigos en pantalla {this.LeftEnemies}");
        if (this.LeftEnemies <= 0 && this.EnemySpawner.GetIsLastFormation() && this.Player != null) {
            // Si ya se espawneo la ultima formacion y no quedan mas enemigos en pantalla
            // Ejecutamos lo siguiente
            StartCoroutine(LevelCompleted());
        }
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
