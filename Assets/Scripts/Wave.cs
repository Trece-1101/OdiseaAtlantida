using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave")]
public class Wave : ScriptableObject
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject pathPrefab;
    [SerializeField] private float timeBetweenSpawn = 1.5f;
    [SerializeField] private float spawnRandomize = 0.5f;
    [SerializeField] private int numberOfEnemies = 5;
    [SerializeField] private float moveSpeed = 2f;

    public GameObject GetEnemyPrefab() {
        return this.enemyPrefab;
    }

    public GameObject GetPathPrefab() {
        return this.pathPrefab;
    }

    public float GetTimeBetweenSpawns() {
        return this.timeBetweenSpawn;
    }

    public float GetSpawnRandomize() {
        return this.spawnRandomize;
    }

    public int GetNumberOfEnemies() {
        return numberOfEnemies;
    }

    public float GetMoveSpeed() {
        return this.moveSpeed;
    }
}
