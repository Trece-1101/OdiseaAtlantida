using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDefense
{
    float HitPoints { get; set; }

    void Die();
    void ReceiveDamage(DamageControl damageControl);

}
