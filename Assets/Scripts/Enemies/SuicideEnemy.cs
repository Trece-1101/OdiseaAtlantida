using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideEnemy : Enemy
{
   
    private void Update() {
        if (this.GetPlayer().GetIsAlive()) {
            CheckRotation();
            Move();
        }
    }

    public override void Move() {
        Vector3 targetPosition;
        if (this.GetPlayer().GetIsCloned()) {
            targetPosition = new Vector3(Random.Range(-4.0f, 4.0f), Random.Range(-4.0f, 4.0f), 0f);
        }
        else {
            targetPosition = this.GetPlayer().transform.position;
        }
        this.transform.position = Vector2.MoveTowards(transform.position, targetPosition, (this.GetVelocity().x * Time.deltaTime * this.GetGameProg().GetScale().x));
    }

}
