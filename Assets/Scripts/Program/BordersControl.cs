//// Clase que controla a todos los bordes exteriores (sobre el nivel de la camara que sirven tanto
/// De ayuda visual al spawnear una formacion enemiga como para el rebote de los PowerUps

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BordersControl : MonoBehaviour
{

    #region "Atributos"
    private List<Transform> Borders = new List<Transform>(); // Lista de bordes
    
    private Color OnColor = new Color(255f, 0f, 0f, 110f); // Color "prendido"
    private Color OffColor = new Color(255f, 0f, 0f, 0f); // Color "apagado"
    private float WaitTime = 0.3f; // Tiempo de borde prendido
    #endregion

    #region "Referencias en Cache"
    private SpriteRenderer ChildSprite; // Referencia al componente SpriteRenderer de cada borde
    #endregion

    #region "Setters y Getters"
    public List<Transform> GetChilds() {
        return this.Borders;
    }
    
    public SpriteRenderer GetChildSprite() {
        return this.ChildSprite;
    }
    public void SetChildSprite(SpriteRenderer value) {
        this.ChildSprite = value;
    }

    public Color GetOnColor() {
        return this.OnColor;
    }
    public void SetOnColor(Color value) {
        this.OnColor = value;
    }

    public Color GetOffColor() {
        return this.OffColor;
    }
    public void SetOffColor(Color value) {
        this.OffColor = value;
    }

    public float GetWaitTime() {
        return this.WaitTime;
    }
    public void SetWaitTime(float value) {
        this.WaitTime = value;
    }

    #endregion

    #region "Metodos"
    private void Start() {
        // Llamo al metodo que agrupa a los hijos del game object en una lista
        this.Borders = this.GetChildrens();
        TurnONorOFF(this.Borders, false); // Desactivo los bordes al inicio
    }

    private List<Transform> GetChildrens() {
        // Este metodo busca en un gameobject todos sus hijos (en jerarquia)
        // de 1 a 8 porque el 0 es el parent
        for (int i = 1; i <= this.transform.childCount; i++) {
            // Por cada hijo que hay lo agrega a la lita
            this.Borders.Add(this.GetComponentsInChildren<Transform>()[i]);
        }

        // Devuelve la lista
        return this.Borders;
    }

    public void EnableBorders() {
        // Cuando este metodo es llamado pasa la lista de bordes al metodo de prendido/apagado
        // Diciendole que lo prenda y en una cantidad de tiempo WaitTime lo apague
        this.TurnONorOFF(this.Borders, true);
        Invoke("DisableBorders", this.WaitTime);
    }

    public void DisableBorders() {
        // Llama al metodo de prendido/apagado con la señal de apagar los bordes
        TurnONorOFF(this.Borders, false);
    }
                   

    private void TurnONorOFF(List<Transform> borders, bool ON) {
        foreach (var border in borders) {
            // Por cada borde en la lista asigno a la referencia spriterenderer su sprite
            // Y dependiendo la señal lo "prendo" (oncolor) o lo apago (offcolor)
            var sprite = border.GetComponent<SpriteRenderer>();
            if (ON) {
                sprite.color = this.OnColor;
            }
            else {
                sprite.color = this.OffColor;
            }
        }
    }
    #endregion
}
