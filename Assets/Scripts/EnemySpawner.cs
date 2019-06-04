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
    [SerializeField] private Formations FormationLists = new Formations();
    #endregion

    #region "Atributos"
    private int StartingWave = 0;
    private int LastWave;
    private int StartingFormation = 0;
    private int LastFormation;
    private bool Loop = false;
    private float WaitSeconds = 3;
    #endregion

    #region "Componentes en Cache"
    private BordersControl BorderCtrl;
    private AudioSource SpawnAudio;
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
    #endregion

    #region "Metodos"
    private void Awake() {
        this.BorderCtrl = FindObjectOfType<BordersControl>();
        this.BorderCtrl.DisableBorders();
        this.SpawnAudio = GetComponent<AudioSource>();
    }

    private IEnumerator Start() {
        do {
            yield return StartCoroutine(SpawnFormations());
        } while (this.Loop);
        //var currentWave = waves[startingWave];
        //StartCoroutine(SpawnWave(currentWave));
    }

    private IEnumerator SpawnFormations() {
        this.LastFormation = FormationLists.JoinWaves.Count;
        this.StartingFormation = this.LastFormation;
        for (int formationIndex = StartingFormation; formationIndex < this.LastFormation; formationIndex++) {
            var currentFormation = FormationLists.JoinWaves[formationIndex].Wave;
            this.WaitSeconds = FormationLists.NextWaveSpawnTime[formationIndex];

            yield return new WaitForSeconds(this.WaitSeconds);



            StartCoroutine(SpawnWaves(currentFormation));

        }
        //Debug.Log("Final de formaciones");
    }


    private IEnumerator SpawnWaves(List<Wave> currentFormation) {
        //Debug.Log("spawn formation");
        SpawnAudio.Play();

        this.BorderCtrl.EnableBorders();

        LastWave = currentFormation.Count;

        for (int waveCount = StartingWave; waveCount < LastWave; waveCount++) {
            var newEnemy = Instantiate(currentFormation[waveCount].GetEnemyPrefab(),
                                        currentFormation[waveCount].GetPathPrefab()[0].transform.position,
                                        Quaternion.identity);

            newEnemy.GetComponent<Path>().SetWave(currentFormation[waveCount]);

            yield return new WaitForSeconds(0);
        }
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
