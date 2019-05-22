using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageControl : MonoBehaviour
{
    [SerializeField] private float damage = 10;

    public float GetDamage() {
        return this.damage;
    }

    public void SetDamage(float value) {
        this.damage = value;
    }
    
}
