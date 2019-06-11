using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave")]
public class Wave : ScriptableObject
{
    [SerializeField] private List<GameObject> enemyPrefab;
    [SerializeField] private GameObject pathPrefab;
    private float timeBetweenSpawn = 0f;
    [SerializeField] private float spawnRandomize = 0f;
    [SerializeField] private int numberOfEnemies = 1;
    private float moveSpeed;

  

    public GameObject GetEnemyPrefab() {
        int enemySelected = Random.Range(0, enemyPrefab.Count);
        //Debug.Log(enemySelected);
        return this.enemyPrefab[enemySelected];
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
        return this.moveSpeed;
    }
    public void SetMoveSpeed(Vector2 value) {
        //this.moveSpeed = 1.3f;
        //this.moveSpeed = this.GetEnemyPrefab().GetComponent<Enemy>().GetVelocity().x;        
        this.moveSpeed = value.x;
    }
   
}
