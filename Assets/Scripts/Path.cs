using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    private Wave wave;
    private List<Transform> wayPoints;
    private float moveSpeed;
    private int wayPointIndex = 0;
    private Enemy enemy;

    private void Start() {
        enemy = gameObject.GetComponentInParent(typeof(Enemy)) as Enemy;
        enemy.CanShoot = false;
        enemy.CanShootMissile = false;
        wayPoints = wave.GetPathPrefab();
        transform.position = wayPoints[wayPointIndex].transform.position;
        wave.SetMoveSpeed(enemy.GetVelocity().x);
        moveSpeed = wave.GetMoveSpeed();
        
    }

    private void Update() {
        MoveInPath();
    }

    public void SetWave(Wave wave) {
        this.wave = wave;
    }


    private void MoveInPath() {
        if (wayPointIndex < wayPoints.Count) {
            MoveToPoint(wayPointIndex);
            if (wayPointIndex == 2) {
                enemy.CanShoot = true;
                enemy.CanShootMissile = true;
            }
        }
        else {
            wayPointIndex = 1;
            MoveToPoint(wayPointIndex);
        }
    } 

    private void MoveToPoint(int index) {
        var targetPosition = wayPoints[index].transform.position;
        var speed = moveSpeed * Time.deltaTime;
        this.transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed);
        if (this.transform.position == targetPosition) {
            wayPointIndex++;
        }
    }
          
   
}

