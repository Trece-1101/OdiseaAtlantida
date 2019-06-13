//// Esta clase es muy sencilla, se adosa como componente a todos los proyectiles de manera tal que almacena el daño que puede hacer
/// dicho proyectil. Se utiliza como un recurso auxiliar en las colisiones buscando este componente, si existe se sabe que es un objeto que genera un daño X

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageControl : MonoBehaviour
{
    #region "Atributos Serializados"
    [SerializeField] private float Damage = 10; // El daño que produce el objeto
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
