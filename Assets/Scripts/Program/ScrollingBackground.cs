using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    #region "Atributos"    
    private Vector2 Velocity;
    private float OffSet;
    #endregion

    #region "Componentes en Cache"
    private Rigidbody2D Body;
    #endregion

    #region "Setters y Getters"
    public Vector2 GetVelocity() {
        return this.Velocity;
    }
    public void SetVelocity(Vector2 value) {
        this.Velocity = value;
    }

    public float GetOffSet() {
        return this.OffSet;
    }
    public void SetOffset(float value) {
        this.OffSet = value;
    }

    #endregion

    #region "Metodos"
    private void Start() {
        this.Body = GetComponent<Rigidbody2D>();
        //Debug.Log(GameProgram.instance.GetScrollSpeed());
        this.OffSet = 13f;
        this.Body.velocity = new Vector2(0f, GameProgram.instance.GetScrollSpeed());
    }

    private void Update() {
        if(this.transform.position.y < -this.OffSet){
            this.transform.position = new Vector3(this.transform.position.x, 
                                                this.OffSet, 
                                                this.transform.position.z);
        }
       
    }
    #endregion
}

