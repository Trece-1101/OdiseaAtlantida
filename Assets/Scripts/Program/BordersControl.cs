using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BordersControl : MonoBehaviour
{

    #region "Atributos"
    private List<Transform> Childs = new List<Transform>();
    private SpriteRenderer ChildSprite;
    private Color OnColor = new Color(255f, 0f, 0f, 110f);
    private Color OffColor = new Color(255f, 0f, 0f, 0f);
    private float WaitTime = 0.3f;
    private float EnableIterations = 0.2f;
    #endregion

    #region "Setters y Getters"
    public List<Transform> GetChilds() {
        return this.Childs;
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

    public float GetEnableIterations() {
        return this.EnableIterations;
    }
    public void SetEnableIterations(float value) {
        this.EnableIterations = value;
    }
    #endregion


    private List<Transform> GetChildrens() {
        // de 1 a 8 porque el 0 es el parent
        for (int i = 1; i <= this.transform.childCount; i++) {
            this.Childs.Add(this.GetComponentsInChildren<Transform>()[i]);
        }

        return this.Childs;
    }

    public void EnableBorders() {
        var childs = GetChildrens();
        TurnONorOFF(childs, true);
        Invoke("DisableBorders", this.WaitTime);
    }

    public void DisableBorders() {
        var childs = GetChildrens();
        TurnONorOFF(childs, false);
    }

    private void Start() {
        GetChilds();
    }
        

    private void TurnONorOFF(List<Transform> childs, bool ON) {
        foreach (var child in childs) {
            var sprite = child.GetComponent<SpriteRenderer>();
            if (ON) {
                sprite.color = this.OnColor;
            }
            else {
                sprite.color = this.OffColor;
            }
        }
    }
}
