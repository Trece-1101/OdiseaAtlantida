using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asimov : Ship
{
    #region "Atributos"
    private float Shield;
    #endregion      

    #region "Componentes"
    private Rigidbody2D Body;
    private Transform target;
    [SerializeField] private List<Sprite> sprites;
    private SpriteRenderer Img;
    private Sprite actualSprite;
    #endregion

    #region "Aux"
    private float leftBorder;
    private float rightBorder;
    private float upBorder;
    private float downBorder;
    private float padding;
    #endregion

    #region "Setters/Getters"
    public float GetShield() {
        return this.Shield;
    }
    public void SetShield(float value) {
        this.Shield = value;
    }
    #endregion

    #region "Constructor"
    public Asimov(float hitpoints, Vector2 velocity, float bulletDamage, float missileDamage, float shield):
                  base(hitpoints, velocity, bulletDamage, missileDamage){
        this.Shield = shield;        
    }
    #endregion

    #region "Metodos"
   
    private void Start() {
        Body = GetComponent<Rigidbody2D>();
        Img = GetComponent<SpriteRenderer>();
        actualSprite = sprites[0];
        Img.sprite = actualSprite;
        SetUpBorders();
    }


    private void Update() {
        Move();
        Rotate();
        CheckSprite();
    }

    private void CheckSprite() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (actualSprite == sprites[0]) {
                actualSprite = sprites[1];
            }
            else {
                actualSprite = sprites[0];
            }

            Img.sprite = actualSprite;
        }
    }

    private void SetUpBorders() {
        Camera mainCamera = Camera.main;
        padding = 0.5f;
        leftBorder = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        rightBorder = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        upBorder = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).y + padding;
        downBorder = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0)).y - padding;
    }

    public override void Move() {
        var deltaX = Input.GetAxis("Horizontal") * GetVelocity().x * Time.deltaTime;
        var deltaY = Input.GetAxis("Vertical") * GetVelocity().y * Time.deltaTime;

        var nextPosX = Mathf.Clamp(transform.position.x + deltaX, leftBorder, rightBorder);
        var nextPosY = Mathf.Clamp(transform.position.y + deltaY, upBorder, downBorder);

        // pos(n) = pos(n-1) + v*t
        transform.position = new Vector2(nextPosX, nextPosY);
    }

    public void Rotate() {
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    

        
    

    #endregion
}
