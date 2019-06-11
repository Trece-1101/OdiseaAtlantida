using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallisticBullet : Proyectile {

    private Rigidbody2D Body;
    private Asimov Player;
    private Vector2 SideControl;
    private bool Initial;

    private void Start() {
        this.Player = FindObjectOfType<Asimov>();
        SetInitialSpeed(this.GetSpeed());
        Body = this.GetComponent<Rigidbody2D>();
        Initial = true;

    }

    public override void Update() {
        //this.SetSpeed(this.GetSpeed() + this.Gravity * Time.deltaTime);
        //this.transform.Translate(Vector2.right * ((this.GetSpeed() * Time.deltaTime) + ((this.Gravity * Mathf.Pow(Time.deltaTime, 2)) / 2)));
        if (Initial) {
            Initial = false;
            Body.velocity = this.GetInitalSpeed();
            this.SideControl = Body.velocity;
            if (this.Player.transform.position.x < this.transform.position.x) {
                SideControl.x *= -1;
            }
            Body.velocity = this.SideControl;
        }
        //Debug.Log($"{this.gameObject.name} -- {this.Body.velocity}");

        Invoke("Die", GetLifeTime());
    }

    public override void Die() {
        gameObject.SetActive(false);
        Initial = true;
    }
}
