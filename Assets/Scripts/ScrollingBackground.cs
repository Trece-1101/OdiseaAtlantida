using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{

    private Rigidbody2D body;
    private Vector2 velocity;
    private float offSet;

    private void Start() {
        body = GetComponent<Rigidbody2D>();
        //Debug.Log(GameProgram.instance.GetScrollSpeed());
        offSet = 13f;
        body.velocity = new Vector2(0f, GameProgram.instance.GetScrollSpeed());
    }

    private void Update() {
        if(this.transform.position.y < -offSet){
            this.transform.position = new Vector3(this.transform.position.x, 
                                                offSet, 
                                                this.transform.position.z);
        }
       
    }
}

