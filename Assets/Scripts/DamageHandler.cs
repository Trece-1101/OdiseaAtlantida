using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    [SerializeField] private float Damage;

    public float GetDamage() {
        return this.Damage;
    }
    
}
