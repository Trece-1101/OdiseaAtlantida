//// Clase que instancia todas las oleadas de objetos en formaciones

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region "Clase Waves (lista de waves)"
    [System.Serializable]
    public class Waves {
        public List<Wave> Wave;
    }
    #endregion

    #region "Clase Formaciones (Lista de listas de Wave)"
    [System.Serializable]
    public class Formations {
        public string Name;
        public List<Waves> JoinWaves;
        public List<int> NextWaveSpawnTime;
    }
    #endregion

    #region "Atributos Serializables"
    [SerializeField] private Formations FormationLists = new Formations(); // Una lista de listas de Oleadas
    [SerializeField] private List<GameObject> SuicideEnemyPrefab = null; // Un prefab para el enemigo suicida (que no sigue un camino)
    #endregion

    #region "Atributos"
    private int StartingFormation = 0; // Formacion de Inicio
    private int LastFormation; // Formacion final
    private int StartingWave = 0; // Oleada de inicio
    private int LastWave; // Oleada final
    private bool Loop = false; // variable de control
    private float WaitSeconds = 3f; // Segundos para la proxima oleada
    private bool IsLastFormation = false; // variable de control si se llego a la ultima formacion
    private float Timer = 0f; // Timer para control de salida de suicidas
    private float TimeToSpawnSuicide;
    private float OriginalTimeToSpawnSuicide = 15f;
    private float LimitTimeForSpawnSuicide = 8f;
    // Puntos donde pueden instanciarse los enemigos suicidas
    private List<Vector3> InstantiatePoints = new List<Vector3>() { new Vector3(-12f, 8f, 0f), new Vector3(0f, 8f, 0f), new Vector3(-12f, 8f, 0f),
                                                                    new Vector3(-12f, -8f, 0f), new Vector3(0f, -8f, 0f), new Vector3(-12f, -8f, 0f),
                                                                    new Vector3(12f, 0f, 0f), new Vector3(-12f, 0f, 0f)};
    #endregion

    #region "Componentes en Cache"
    private BordersControl BorderCtrl; // Referencia a los bordes que sirven de aviso visual de nueva oleada
    private AudioSource SpawnAudio; // Referencia auditiva de proxima oleada
    #endregion

    #region "Setters Y Getters"
    public Formations GetFormatios() {
        return this.FormationLists;
    }
    public void SetFormations(Formations value) {
        this.FormationLists = value;
    }

    public int GetStartingWave() {
        return this.StartingWave;
    }
    public void SetStartingWave(int value) {
        this.StartingWave = value;
    }

    public int GetLastWave() {
        return this.LastWave;
    }
    public void SetLastWave(int value) {
        this.LastWave = value;
    }

    public int GetStartingFormation() {
        return this.StartingFormation;
    }
    public void SetStartingFormation(int value) {
        this.StartingFormation = value;
    }

    public int GetLastFormation() {
        return this.LastFormation;
    }
    public void SetLastFormation(int value) {
        this.LastFormation = value;
    }

    public bool GetLoop() {
        return this.Loop;
    }
    public void SetLoop(bool value) {
        this.Loop = value;
    }

    public float GetWaitSeconds() {
        return this.WaitSeconds;
    }
    public void SetWaitSeconds(float value) {
        this.WaitSeconds = value;
    }

    public bool GetIsLastFormation() {
        return this.IsLastFormation;
    }
    public void SetIsLastFormation(bool value) {
        this.IsLastFormation = value;
    }
    #endregion

    #region "Metodos"
    private void Awake() {
        // Primer metodo que se ejecuta cuando el objeto es "visto" en la jerarquia
        // Enlazamos los componentes en cache con sus respectivas referencias
        this.BorderCtrl = FindObjectOfType<BordersControl>();
        this.SpawnAudio = GetComponent<AudioSource>();

        this.BorderCtrl.DisableBorders(); // Desactivamos los bordes

        this.TimeToSpawnSuicide = this.OriginalTimeToSpawnSuicide;
    }

    private IEnumerator Start() {        
        // Hacemos que el metodo Start emita corutinas
        do {
            // Llamamos a la corutina que spawnea formaciones
            yield return StartCoroutine(SpawnFormations());
        } while (this.Loop);
    }

    private IEnumerator SpawnFormations() {
        //Esta rutina spawnea formaciones (lista de oleadas)

        //this.StartingFormation = this.LastFormation; // Descomentar para iniciar en una formacion deseada distinta a 0

        this.LastFormation = FormationLists.JoinWaves.Count; // la ultima formacion es la ultima de la lista de formaciones
        this.StartingFormation = 0; // La primera formacion es la de indice 0

        for (int formationIndex = this.StartingFormation; formationIndex < this.LastFormation; formationIndex++) {
            // Itera dentro de las formaciones (desde 0 hasta la ultima)
            var currentFormation = FormationLists.JoinWaves[formationIndex].Wave; // asignamos formacion actual a una var
            this.WaitSeconds = FormationLists.NextWaveSpawnTime[formationIndex]; // tomamos el valor de segundos de salida de la formacion

            yield return new WaitForSeconds(this.WaitSeconds); // Esperamos ese valor de salida

            if(formationIndex == this.LastFormation - 1) {
                // Controlamos si estamos ante la ultima formacion
                this.IsLastFormation = true;
            }

            // Spawneamos las oleadas de la formacion que conforman a la formacion entera
            StartCoroutine(SpawnWaves(currentFormation));

        }
    }


    private IEnumerator SpawnWaves(List<Wave> currentFormation) {
        //Debug.Log("spawn formation");
        SpawnAudio.Play(); // Ejecutamos el sonido de la salida de la oleada

        this.BorderCtrl.EnableBorders(); // Activamos las ayudas visuales

        this.LastWave = currentFormation.Count; // asignamos al valor de la ulitma oleada

        for (int waveCount = this.StartingWave; waveCount < this.LastWave; waveCount++) {
            // desde la primera oleada (0) hasta la ultima instanciamos un enemigo
            // cada enemigo tiene un prefab, empieza en el punto 0 de su camino y sin rotacion
            var newEnemy = Instantiate(currentFormation[waveCount].GetEnemyPrefab(),
                                        currentFormation[waveCount].GetPathPrefab()[0].transform.position,
                                        Quaternion.identity);

            // Seteamos la oleada en el Path
            newEnemy.GetComponent<Path>().SetWave(currentFormation[waveCount]);                       

            // con el 0 nos aseguramos que todas las oleadas hagan spawn al mismo tiempo
            yield return new WaitForSeconds(0);
        }
        
    }

    private void Update() {
        // Acrecentamos el timer y cada cierta cantidad de tiempo spawneamos un enemigo suicida       
        this.Timer += Time.deltaTime;

        if(this.Timer >= this.TimeToSpawnSuicide && !this.IsLastFormation) {
            this.TimeToSpawnSuicide /= 1.1f; // Reducimos el tiempo de salida del proximo suicida
            InstantiateSuicide(); // Llamamos al metodo que instancia al enemigo suicida
            this.Timer = 0f; // volvemos el timer a 0
        }

        // Si el tiempo para instanciar suicidas es menor o igual al minimo limite, volvemos al origen
        if (this.TimeToSpawnSuicide <= this.LimitTimeForSpawnSuicide) {
            this.TimeToSpawnSuicide = this.OriginalTimeToSpawnSuicide;
        }
    }

    private void InstantiateSuicide() {
        // Instanciamos un suicida random de la listas de enemigos suicidas
        // y lo hacemos en una posicion random utilizando los puntos creados
        int enemySelected = Random.Range(0, SuicideEnemyPrefab.Count);
        int spawnPointSelected = Random.Range(0, InstantiatePoints.Count);
        Instantiate(SuicideEnemyPrefab[enemySelected], InstantiatePoints[spawnPointSelected], Quaternion.identity);
    }

    //private void DebugRazonar() {
    //    Debug.Log("Numero de formaciones " + FormationLists.JoinWaves.Count);
    //    Debug.Log("########################################################");
    //    for (int i = 0; i < FormationLists.JoinWaves.Count; i++) {
    //        Debug.Log("Numero de waves de la formacion " + FormationLists.JoinWaves[i].Wave.Count);
    //        var currentFormation = FormationLists.JoinWaves[i].Wave;
    //        Debug.Log("########################################################");
    //        for (int f = 0; f < currentFormation.Count; f++) {
    //            Debug.Log("nombre wave: " + currentFormation[f].name);
    //        }
    //    }
    //    Debug.Log("########################################################");
    //}
    #endregion




}
