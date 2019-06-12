//// Clase Derivada/Hija de "Enemy" que describe a a enemigos que no disparan balas sino
//// que persiguen al jugador por todo el escenario

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideEnemy : Enemy
{

    private float Speed;

    public override void Awake() {
        base.Awake();
        this.Speed = this.GetVelocity().x * this.GetGameProg().GetScale().x;
    }

    private void Update() {
        // solo si el jugador esta activo/vivo
        if (this.GetPlayer().GetIsAlive()) {
            this.CheckRotation(); // El chequeo de rotacion es la de la clase Enemy
            this.Move(); // Metodo que controla el movimiento
        }
    }

    public override void Move() {
        Vector3 targetPosition; // Vector que guarda la posicion a la que se va a trasladar
        if (this.GetPlayer().GetIsCloned()) {
            // Si el player esta clonada en vez de perseguirla persigue un rectangulo random
            targetPosition = new Vector3(Random.Range(-4.0f, 4.0f), Random.Range(-4.0f, 4.0f), 0f);
        }
        else {
            // De otra manera el vector target es la posicion del jugador en el cuadro dado
            targetPosition = this.GetPlayer().transform.position;
        }
        // Usando el metodo MoveTowards (mueve un punto desde una posicion X1 a una X2 de a DeltaX pasos
        // d = v * t
        var distance = this.Speed * Time.deltaTime;
        this.transform.position = Vector2.MoveTowards(transform.position, targetPosition, distance);
    }

}
