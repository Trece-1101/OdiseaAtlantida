using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    #region "Atributos"
    private string Name;
    private Vector2 Speed;
    private Vector2 SpeedChange;
    private bool FirstContact;
    #endregion

    #region "Componentes en Cache"
    Rigidbody2D MyBody;
    ObjectPool Pool;
    Asimov Asimov;
    #endregion

    #region "Setters y Getters"
    public string GetName() {
        return this.name;
    }
    public void SetName(string value) {
        this.name = value;
    }

    public Vector2 GetSpeed() {
        return this.Speed;
    }
    public void SetSpeed(Vector2 value) {
        this.Speed = value;
    }

    public Vector2 GetSpeedChange() {
        return this.SpeedChange;
    }
    public void SetSpeedChange(Vector2 value) {
        this.SpeedChange = value;
    }

    public Rigidbody2D GetMyBody() {
        return this.MyBody;
    }
    public void SetMyBody(Rigidbody2D value) {
        this.MyBody = value;
    }

    public bool GetFirstContact() {
        return this.FirstContact;
    }
    public void SetFirstContact(bool value) {
        this.FirstContact = value;
    }

    public Asimov GetAsimov() {
        return this.Asimov;
    }
    public void SetAsimov(Asimov value) {
        this.Asimov = value;
    }
    #endregion

    private void Start() {
        this.FirstContact = false;
        this.MyBody = GetComponent<Rigidbody2D>();
        this.Pool = FindObjectOfType<ObjectPool>();
        this.Asimov = FindObjectOfType<Asimov>();
        this.Speed = RandomSpeed();
        this.SpeedChange = this.Speed / 2;
        this.MyBody.velocity = this.Speed;
    }
    
    private Vector2 RandomSpeed() {
        var speed = Vector2.zero;
        while (speed.x < 1 && speed.x > -1) {
            speed.x = Random.Range(-4f, 4f);
        }
        while (speed.y < 1 && speed.y > -1) {
            speed.y = Random.Range(-3f, 3f);
        }

        return speed;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!FirstContact) {
            this.FirstContact = true;
            Invoke("Die", 5f);
        }

        this.Speed = Vector2.Reflect(this.Speed, collision.contacts[0].normal);
        this.SpeedChange = Vector2.Reflect(this.SpeedChange, collision.contacts[0].normal);
        this.SpeedChange *= 1.2f;
        //Debug.Log("---- Contacto ----");
        //Debug.Log(this.Speed);
        //Debug.Log(this.SpeedChange);

        this.MyBody.velocity = this.Speed + this.SpeedChange;
        //Debug.Log($"Velocidad PowerUp {this.MyBody.velocity}");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        PickUp();
    }

    private void PickUp() {
        // crear efecto de particulas
        this.Pool.Spawn("PowerUpParticles", this.transform.position, Quaternion.identity);

        // afectar player
        this.Asimov.SetHasPowerUp(true);
                
        Die();
    }

    //public abstract void MakeYourMagic();

    private void Die() {
        this.gameObject.SetActive(false);
    }


}
