using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Ship
{
    
    
    #region "Constructor"
    public Enemy(float hitpoints, Vector2 velocity, float bulletDamage, float missileDamage) :
                  base(hitpoints, velocity, bulletDamage, missileDamage) {
        
    }
    #endregion


    public override void Shoot() {
        throw new System.NotImplementedException();
    }

    public override void Move() {
        throw new System.NotImplementedException();
    }
}
