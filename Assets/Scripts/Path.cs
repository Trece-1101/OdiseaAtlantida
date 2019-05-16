using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    [SerializeField] private List<Transform> wayPoints;
    private float moveSpeed = 4f;
    private int wayPointIndex = 0;

    private void Start() {
        transform.position = wayPoints[wayPointIndex].transform.position;
    }

    private void Update() {
        MoveInPath();
    }

    private void MoveInPath() {
        if (wayPointIndex < wayPoints.Count) {
            var targetPosition = wayPoints[wayPointIndex].transform.position;
            var movement = moveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movement);
            // TODO: verificar si esto no es al pedo
            if (transform.position == targetPosition) {
                wayPointIndex++;
            }
        }
        else {
            Destroy(gameObject);
        }
    }
}
