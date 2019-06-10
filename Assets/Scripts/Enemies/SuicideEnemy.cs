using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideEnemy : Enemy
{
   
    private void Update() {
        CheckRotation();
        Move();
    }

    public override void Move() {
        var targetPosition = this.GetPlayer().transform.position;
        this.transform.position = Vector2.MoveTowards(transform.position, targetPosition, this.GetVelocity().x * Time.deltaTime);
    }

}
