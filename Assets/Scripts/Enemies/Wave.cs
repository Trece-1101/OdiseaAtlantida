//// Clase que contiene la Oleada del enemigo => une una lista de enemigos con un camino a seguir

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave")]
public class Wave : ScriptableObject
{

    #region "Atributos Serializables"
    [SerializeField] private List<GameObject> enemyPrefab; // Listas de enemigos que pueden aparecer
    [SerializeField] private GameObject pathPrefab; // Camino a seguir
    #endregion

    #region "Atributos"
    //private float timeBetweenSpawn = 0f; // TODO: quitar esto
    //private float spawnRandomize = 0f;
    private int NumberOfEnemies = 1;
    private float MoveSpeed;
    #endregion


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

    //public float GetTimeBetweenSpawns() {
    //    return this.timeBetweenSpawn;
    //}

    //public float GetSpawnRandomize() {
    //    return this.spawnRandomize;
    //}

    public int GetNumberOfEnemies() {
        return this.NumberOfEnemies;
    }

    public float GetMoveSpeed() {
        return this.MoveSpeed;
    }
    public void SetMoveSpeed(Vector2 value) {
        //this.moveSpeed = 1.3f;
        //this.moveSpeed = this.GetEnemyPrefab().GetComponent<Enemy>().GetVelocity().x;        
        this.MoveSpeed = value.x;
    }
   
}
