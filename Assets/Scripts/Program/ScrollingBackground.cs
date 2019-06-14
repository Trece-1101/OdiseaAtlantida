//// Clase que controla el scrolling de los Backgrounds

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    #region "Atributos"    
    private Vector2 Velocity;
    private float OffSet;
    private float NumberOfAlternatives;
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
        // Esto valores estan hardcodeados y deben ser modificados dependiendo del tamaño
        // Que querramos dibujar cada segmento del mapa, todos deben ser del mismo tamaño
        // Y la cantidad de segmentos
        this.OffSet = 13f;
        this.NumberOfAlternatives = 1;
        // Le damos al rigidbody la velocidad almacenada en el GameProgram
        this.Body.velocity = new Vector2(0f, GameProgram.Instance.GetScrollSpeed());
    }

    private void Update() {
        // En cada frame vamos a calcular la posicion en 'y' de la porcion del mapa
        if(this.transform.position.y < -(this.OffSet)){
            // Si la posicion 'y' es menor al offset significa que ya esta por fuera de la camara
            // Subimos la porcion de mapa sobre la anterior
            this.transform.position = new Vector3(this.transform.position.x, 
                                                this.OffSet * this.NumberOfAlternatives, 
                                                this.transform.position.z);
        }
       
    }
    #endregion
}

