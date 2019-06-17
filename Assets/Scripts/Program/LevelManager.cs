using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] List<GameObject> Spawners = null;
    [SerializeField] private float ScrollSpeed = -3.5f; //private float ScrollSpeed; // velocidad de movimiento del escenario
    [SerializeField] private int NumberAlternatives = 1;
    [SerializeField] private float Offset = 13f;
    private EnemySpawner EnemySpawner;

    private float LeftBorder; // Borde izquierdo maximo donde puede moverse el jugador
    private float RightBorder; // Borde derecho maximo donde puede moverse el jugador
    private float UpBorder; // Borde superior maximo donde puede moverse el jugador
    private float DownBorder; // Borde inferior maximo donde puede moverse el jugador
    private float Padding; // distancia de seguridad (por el tamaño de la nave)
                           


    public float GetLeftBorder() {
        return this.LeftBorder;
    }
    public void SetLeftBorder(float value) {
        this.LeftBorder = value;
    }

    public float GetRightBorder() {
        return this.RightBorder;
    }
    public void SetRightBorder(float value) {
        this.RightBorder = value;
    }

    public float GetUpBorder() {
        return this.UpBorder;
    }
    public void SetUpBorder(float value) {
        this.UpBorder = value;
    }

    public float GetDownBorder() {
        return this.DownBorder;
    }
    public void SetDownBorder(float value) {
        this.DownBorder = value;
    }

    public float GetScrollSpeed() {
        return this.ScrollSpeed;
    }
    public void SetScrollSpeed(float value) {
        this.ScrollSpeed = value;
    }

    public float GetOffset() {
        return this.Offset;
    }
    public void SetOffset(float value) {
        this.Offset = value;
    }

    public int GetNumberAlternatives() {
        return this.NumberAlternatives;
    }
    public void SetNumberAlternatives(int value) {
        this.NumberAlternatives = value;
    }


         

    private void Start() {
        //this.SetScrollSpeed(-3.5f); // Velocidad de desplazamiento de la camara
        this.SetUpBorders();
        Cursor.visible = false;

        if (this.Spawners.Count > 0) {
            // elegimos un enemyspawner random de la lista (asi los niveles nunca seran iguales)
            var spawnThis = Random.Range(0, this.Spawners.Count);
            this.Spawners[spawnThis].SetActive(true);
            this.EnemySpawner = FindObjectOfType<EnemySpawner>();
            //Debug.Log(this.EnemySpawner.name);
        }
    }

    private void SetUpBorders() {
        // En este metodo establecemos hasta donde se puede mover la nave del Player
        // Estos valores lo va a usar en su movimiento junto con la funcion "clamp"
        Camera mainCamera = Camera.main; // Referencia a la camara principal
        this.Padding = 0.8f; // Padding

        this.LeftBorder = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + this.Padding; // Borde izquierdo 
        this.RightBorder = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - this.Padding; // Borde derecho
        this.UpBorder = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).y + (this.Padding + 0.6f); // Borde superior
        this.DownBorder = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0)).y - this.Padding; // Borde inferior
    }

    


}
