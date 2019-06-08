using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour, IAttack
{
    #region "Atritutos Serializados"
    [Header("Shoot")]
    [SerializeField] private List<Transform> ShootsPositions;
    #endregion

    #region "Atributos"
    public float TimeBetweenBulletShoots { get; set; }
    public float TimeBetweenMissileShoots { get; set; }
    public float RemainTimeForShootBullet { get; set; }
    public float RemainTimeForShootMissile { get; set; }
    public bool CanShoot { get; set; }
    public bool CanShootMissile { get; set; }
    public bool CanMove { get; set; }
    #endregion

    #region "Componentes en Cache"
    private ObjectPool objectPool;
    public DamageControl DamageCtrl { get; set; }
    private Asimov asimov;
    #endregion

    private void Start() {
        this.asimov = FindObjectOfType<Asimov>();
        this.objectPool = ObjectPool.Instance;
        this.TimeBetweenBulletShoots = 0.2f;
        this.TimeBetweenMissileShoots = 1f;
        this.RemainTimeForShootBullet = 0.1f;
        this.RemainTimeForShootMissile = 0.1f;
        this.CanShootMissile = true;
        this.CanShoot = true;
    }

    private void Update() {
        Shoot();
    }


    public void Shoot() {
        if (this.RemainTimeForShootBullet <= 0 && this.CanShoot) {
            for (int i = 0; i < this.ShootsPositions.Count; i++) {
                //objectPool.Spawn("PlayerBullet", this.ShootsPositions[i].position, asimov.transform.rotation);
                objectPool.Spawn("DroneBullet", ShootsPositions[i].position, this.asimov.GetMyBulletRotation());
            }

            this.RemainTimeForShootBullet = this.TimeBetweenBulletShoots;
        }
        else {
            this.RemainTimeForShootBullet -= Time.deltaTime;
        }

    }
}
