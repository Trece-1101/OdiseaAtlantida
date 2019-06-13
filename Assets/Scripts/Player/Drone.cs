//// Clase que describe al objeto "Drone" que son hijos (en jerarquia del motor) de la Nave y solo llamados por un powerUp

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour, IAttack
{
    #region "Atritutos Serializados"
    [Header("Shoot")]
    [SerializeField] private List<Transform> ShootsPositions = null; // Puntos desde donde puede disparar el drone
    #endregion

    #region "Atributos"
    public float TimeBetweenBulletShoots { get; set; } // Implementacion de la interfaz IAttack, tiempo que debe pasar para poder ejecutar disparos de balas consecutivos
    public float TimeBetweenMissileShoots { get; set; } // Implementacion de la interfaz IAttack, tiempo que debe pasar para poder ejecutar disparos de misiles consecutivos
    public float RemainTimeForShootBullet { get; set; } // Implementacion de la interfaz IAttack, tiempo restante para disparar balas
    public float RemainTimeForShootMissile { get; set; } // Implementacion de la interfaz IAttack, tiempo restante para disparar misiles
    public bool CanShoot { get; set; }  // Implementacion de la interfaz IAttack, determina la posibilidad de disparar balas
    public bool CanShootMissile { get; set; } // Implementacion de la interfaz IAttack, determina la posibilidad de disparar misiles   
    public bool CanMove { get; set; } // Implementacion de la interfaz IAttack, determina la posibilidad de moverse
    #endregion

    #region "Componentes en Cache"
    private ObjectPool Pool; // Referencia al Pool que contiene los objetos instanciados
    private Asimov Player; // Referencia al Player
    public DamageControl DamageCtrl { get; set; } // Implementacion de la interfaz IDefense
    #endregion

    #region "Metodos"
    private void Start() {
        // Primer metodo que se ejecuta cuando el objeto es "visto" en la jerarquia si no esta declarado "Awake"
        // Enlazamos los componentes en cache con sus respectivas referencias
        this.Player = FindObjectOfType<Asimov>();
        this.Pool = ObjectPool.Instance;

        // Asignamos valores de inicio
        this.TimeBetweenBulletShoots = 0.2f;
        this.TimeBetweenMissileShoots = 1f;
        this.RemainTimeForShootBullet = 0.1f;
        this.RemainTimeForShootMissile = 0.1f;
        this.CanShootMissile = true;
        this.CanShoot = true;
    }

    private void Update() {
        // Los drones disparan constantemente hasta que se desactivan
        this.Shoot(); // Metodo para disparar
    }


    public void Shoot() {
        // Metodo para disparar. Si el tiempo de refresco entre disparos es menor a 0 y el drone puede disparar
        if (this.RemainTimeForShootBullet <= 0 && this.CanShoot) {
            // Por cada posicion de disparo llamamos al pool y activamos la bala del drone
            for (int i = 0; i < this.ShootsPositions.Count; i++) {
                this.Pool.Spawn("DroneBullet", ShootsPositions[i].position, this.Player.GetMyBulletRotation());
            }

            this.RemainTimeForShootBullet = this.TimeBetweenBulletShoots; // reiniciamos el tiempo de refresco
        }
        else {
            this.RemainTimeForShootBullet -= Time.deltaTime; // si aun no se acaba el tiempo de refresco descontamos un deltaTime
        }

    }
    #endregion
}
