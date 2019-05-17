using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgram : MonoBehaviour
{

    #region "Atributos"
    private float LeftBorder;
    private float RightBorder;
    private float UpBorder;
    private float DownBorder;
    private float padding;
    #endregion

    #region "Setters/Getters"
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
    #endregion

    #region "Referencias en Cache"
    private Asimov asimov;
    private CrossHair crossHair;
    #endregion

    #region "Metodos"
    private void Start() {
        asimov = FindObjectOfType<Asimov>();
        asimov.SetVelocity(new Vector2(4f, 4f));
        asimov.SetTimeBetweenShoots(0.2f);
        asimov.SetTimeBetweenMissileShoots(1f);
        asimov.SetHitPoints(100);

        crossHair = FindObjectOfType<CrossHair>();
        Cursor.visible = false;
        SetUpBorders();
    }

    private void Update() {
        crossHair.transform.position = Input.mousePosition;
        //Debug.Log(asimov.GetHitPoints());
    }


    private void SetUpBorders() {
        Camera mainCamera = Camera.main;
        padding = 0.8f;
        this.LeftBorder = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        this.RightBorder = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        this.UpBorder = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).y + padding;
        this.DownBorder = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0)).y - padding;
    }


    #endregion
}
