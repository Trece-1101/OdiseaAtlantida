using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirMine : Proyectile
{
    private Vector2 Target;
    private bool Shooted;
    private bool OnTarget;
    private Animator MyAnimator;

   
    public void SetTarget(Vector2 value) {
        this.Target = value;
    }

    public bool GetShooted() {
        return this.Shooted;
    }
    public void SetShooted(bool value) {
        this.Shooted = value;
    }

    public bool GetOnTarget() {
        return this.OnTarget;
    }
    public void SetOnTarget(bool value) {
        this.OnTarget = value;
    }

    private void Start() {        
        this.MyAnimator = GetComponentInChildren<Animator>();
    }


    public override void Update() {
        if (this.Shooted) {
            transform.position = Vector2.MoveTowards(transform.position, Target, this.GetSpeed().x * Time.deltaTime);
            if(this.transform.position.x == this.Target.x && this.transform.position.y == this.Target.y) {
                this.OnTarget = true;
                this.Explode();
            }
            else {
                this.OnTarget = false;
            }
        }
    }

    public void Explode() {
        this.MyAnimator.SetTrigger("explode");
        this.transform.localScale = new Vector3(5f, 5f, 1f);
        Invoke("SetInactive", 2f);
    }

    private void SetInactive() {
        this.gameObject.SetActive(false);
    }

    public void OnEnable() {
        //this.MyAnimator.Play();
        this.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
    }
   

    private void OnTriggerEnter2D(Collider2D collision) {
        Explode();
    }


}
