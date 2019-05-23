﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Waves {
        public List<Wave> list;
    }

    [System.Serializable]
    public class Formations {
        public List<Waves> list;
    }

    [SerializeField] private Formations FormationLists = new Formations();
    [SerializeField] private List<Wave> waves;
    int startingWave = 0;
    private bool loop = false;
    private bool stillAlive;

    private IEnumerator Start() {
        do {
            yield return StartCoroutine(SpawnFormation());
        } while (loop);
        //var currentWave = waves[startingWave];
        //StartCoroutine(SpawnWave(currentWave));
    }

    private IEnumerator SpawnFormation() {
        for (int waveIndex = startingWave; waveIndex < waves.Count; waveIndex++) {
            var currentWave = waves[waveIndex];
            yield return StartCoroutine(SpawnWave(currentWave));
        }
    }

    private IEnumerator SpawnWave(Wave currentWave) {
        for (int enemyCount = 0; enemyCount < currentWave.GetNumberOfEnemies(); enemyCount++) {
            var newEnemy = Instantiate(currentWave.GetEnemyPrefab(),
                        currentWave.GetPathPrefab()[0].transform.position,
                        Quaternion.identity);

            newEnemy.GetComponent<Path>().SetWave(currentWave);
            //currentWave.SetMoveSpeed(newEnemy.GetComponent<Enemy>().GetVelocity().x);

            yield return new WaitForSeconds(currentWave.GetTimeBetweenSpawns());
        }
        
    }

}
