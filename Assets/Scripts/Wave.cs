using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave")]
public class Wave : ScriptableObject
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject pathPrefab;
    private float timeBetweenSpawn = 0f;
    [SerializeField] private float spawnRandomize = 0.5f;
    [SerializeField] private int numberOfEnemies = 5;
    private float moveSpeed;

    public GameObject GetEnemyPrefab() {
        return this.enemyPrefab;
    }

    public List<Transform> GetPathPrefab() {
        var waveWayPoints = new List<Transform>();
        foreach (Transform wayPoint in pathPrefab.transform) {
            waveWayPoints.Add(wayPoint);
        }

        return waveWayPoints;
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
        Debug.Log(this.moveSpeed);
        return this.moveSpeed;
    }
    public void SetMoveSpeed(float value) {
        this.moveSpeed = value;
        //this.moveSpeed = enemyPrefab.GetComponent<Enemy>().GetVelocity().x;        
    }
   
}
