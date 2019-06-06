using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageControl : MonoBehaviour
{
    #region "Atributos Serializados"
    [SerializeField] private float Damage = 10;
    #endregion

    #region "Setters y Getters"
    public float GetDamage() {
        return this.Damage;
    }
    public void SetDamage(float value) {
        this.Damage = value;
    }
    #endregion



}
