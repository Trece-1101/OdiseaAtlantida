using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private Vector3 upPos;
    private Vector3 downPos;
    private Vector3 rightPos;
    private Vector3 leftPos;
    private Quaternion noRotation;
    private Quaternion rotated;
    private int positionCounter;
    private List<Vector3> positions = new List<Vector3>();
    
    private void Start() {
        upPos = new Vector3(0f, 0.55f, 0f);        
        downPos = new Vector3(0f, -0.55f, 0f);        
        rightPos = new Vector3(0.55f, 0f, 0f);        
        leftPos = new Vector3(-0.55f, 0f, 0f);
        positions.Add(upPos);
        positions.Add(rightPos);
        positions.Add(downPos);
        positions.Add(leftPos);       

        noRotation = Quaternion.Euler(0f, 0f, 0f);
        rotated = Quaternion.Euler(0f, 0f, 90f);
        positionCounter = 0;
        ShieldControl(positionCounter);
    }

    public void ShieldControl(int value) {
        positionCounter += value;
        if(positionCounter > 3) {
            positionCounter = 0;
        }

        if(positionCounter < 0) {
            positionCounter = 3;
        }

        if(positionCounter == 0 || positionCounter == 2) {
            this.transform.localRotation = rotated;
        }
        else {
            this.transform.localRotation = noRotation;
        }

        this.transform.localPosition = positions[positionCounter];

    }
  
}
