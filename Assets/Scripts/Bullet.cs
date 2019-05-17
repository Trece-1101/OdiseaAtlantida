using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Proyectile
{
    private Asimov asimov;
    private Enemy enemy;
    

    private void Start() {
        SetSpeed(10f);
        SetLifeTime(3f);
        
    }

    public override void Update() {
        // translate mueve el sprite en la direccion y distancia dada (pos = pos + v*t)
        // como el sprite del proyectil esta en sentido Este el vector direccion es el vector unitario (1, 0)
        transform.Translate(Vector2.right * this.GetSpeed() * Time.deltaTime);
        Invoke("Die", GetLifeTime());
    }

    //private void OnTriggerEnter2D(Collider2D collision) {
    //    //Debug.Log(collision.gameObject.name);
    //    var whoShoot = GetComponentInParent<LayerMask>();
    //    if (whoShoot == 0) {
    //        Debug.Log("dispara asimov");
    //        if (collision.gameObject.tag == "Enemy") {
    //            collision.gameObject.GetComponent<Enemy>().ReceiveDamage(10);
    //            gameObject.SetActive(false);
    //        }
    //    }
    //    else {
    //        Debug.Log("dispara el otro");
    //        if (collision.gameObject.tag == "Player") {
    //            collision.gameObject.GetComponent<Asimov>().ReceiveDamage(20);
    //            gameObject.SetActive(false);
    //        }
    //    }


    //}


}
