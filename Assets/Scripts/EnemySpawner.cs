using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Waves {
        public List<Wave> Wave;
    }

    [System.Serializable]
    public class Formations {
        public string Name;
        public List<Waves> JoinWaves;
        public List<int> NextWaveSpawnTime;
    }

    [SerializeField] private Formations FormationLists = new Formations();
    [SerializeField] private List<Wave> waves;
    int startingWave = 0;
    private bool loop = false;
    private bool stillAlive;
    private List<int> enemiesToKill = new List<int>();
    private int currentWave = 0;
    private bool firstWave = true;
    private bool advance = false;
    private int waitSeconds = 3;



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


    private IEnumerator Start() {
        do {
            yield return StartCoroutine(SpawnFormations());
        } while (loop);
        //var currentWave = waves[startingWave];
        //StartCoroutine(SpawnWave(currentWave));
    }

    private IEnumerator SpawnFormations() {
        // FormationLists.JoinWaves.Count
        for (int formationIndex = 0; formationIndex < 2; formationIndex++) {
            var currentFormation = FormationLists.JoinWaves[formationIndex].Wave;            
            enemiesToKill.Add(currentFormation.Count);
            waitSeconds = FormationLists.NextWaveSpawnTime[formationIndex];

            yield return new WaitForSeconds(waitSeconds);

            

            StartCoroutine(SpawnWaves(currentFormation));
           
        }
        //Debug.Log("Final de formaciones");
    }


    private IEnumerator SpawnWaves(List<Wave> currentFormation) {
        //Debug.Log("spawn formation");
        for (int waveCount = 0; waveCount < currentFormation.Count; waveCount++) {            
            var newEnemy = Instantiate(currentFormation[waveCount].GetEnemyPrefab(),
                                        currentFormation[waveCount].GetPathPrefab()[0].transform.position,
                                        Quaternion.identity);

            newEnemy.GetComponent<Path>().SetWave(currentFormation[waveCount]);     

            yield return new WaitForSeconds(0);
        }
    }

    public void DiscountEnemyOnWave() {
        //Debug.Log("current" + currentWave);
        //Debug.Log("enemies" + enemiesToKill[currentWave]);
        enemiesToKill[currentWave] = enemiesToKill[currentWave] - 1;

        if (enemiesToKill[currentWave] == 0) {
            currentWave++;
            advance = true;            
        }
        else {
            advance = false;
        }

    }  
 
    
    

}
