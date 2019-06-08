using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, IAttack
{

    #region "Atritutos Serializados"
    [Header("Shoot")]
    [SerializeField] private List<Transform> ShootsPositions;
    #endregion

    #region "Atributos"
    private int PositionCounter;
    private List<Vector3> Positions = new List<Vector3>();
    private List<Quaternion> Rotations = new List<Quaternion>();
    private float HitPoints;
    private bool IsEnable;
    private float OriginalHitPoints;
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

    #region "Setters y Getters"
    public int GetPositionCounter() {
        return this.PositionCounter;
    }
    public void SetPositionCounter(int value) {
        this.PositionCounter = value;
    }

    public List<Vector3> GetPositions() {
        return this.Positions;
    }
    public void SetPositions(List<Vector3> value) {
        this.Positions = value;
    }

    public List<Quaternion> GetRotations() {
        return this.Rotations;
    }
    public void SetRotations(List<Quaternion> value) {
        this.Rotations = value;
    }

    public float GetHitPoints() {
        return this.HitPoints;
    }
    public void SetHitPoints(float value) {
        this.HitPoints = value;
    }

    public float GetOriginalsHitPoints() {
        return this.HitPoints;
    }
    public void SetOriginalsHitPoints(float value) {
        this.HitPoints = value;
    }

    public bool GetIsEnable() {
        return this.IsEnable;
    }
    public void SetIsEnable(bool value) {
        this.IsEnable = value;
    }
    #endregion

    #region "Metodos"
    private void Start() {
        this.asimov = FindObjectOfType<Asimov>();
        this.objectPool = ObjectPool.Instance;

        TimeBetweenBulletShoots = 0.8f;
        this.HitPoints = 60;
        this.OriginalHitPoints = this.HitPoints;

        //this.CanShoot = true;
        this.RemainTimeForShootBullet = this.TimeBetweenBulletShoots;
        this.RemainTimeForShootMissile = this.TimeBetweenMissileShoots;


        this.Positions.Add(new Vector3(0f, 0.4f, 0f)); // escudo arriba
        this.Positions.Add(new Vector3(0.4f, 0f, 0f)); // escudo derecha
        this.Positions.Add(new Vector3(0f, -0.4f, 0f)); // escudo abajo        
        this.Positions.Add(new Vector3(-0.4f, 0f, 0f)); // escudo izquierda

        this.Rotations.Add(Quaternion.Euler(0f, 0f, 270f)); // escudo arriba
        this.Rotations.Add(Quaternion.Euler(0f, 0f, 180f)); // escudo derecha
        this.Rotations.Add(Quaternion.Euler(0f, 0f, 90f)); // escudo abajo        
        this.Rotations.Add(Quaternion.Euler(0f, 0f, 0f)); // escudo izquierda


        this.CanMove = true;
        this.PositionCounter = 0;
        ShieldControl(this.PositionCounter);
        this.CanShoot = true;
    }

    public void ShieldControl(int value) {
        this.PositionCounter += value;

        if (this.PositionCounter > 3) {
            this.PositionCounter = 0;
        }

        if (this.PositionCounter < 0) {
            this.PositionCounter = 3;
        }

        if (this.CanMove) {
            this.transform.localPosition = this.Positions[this.PositionCounter];
            this.transform.localRotation = this.Rotations[this.PositionCounter];
        }
    }

    public void GetShieldOnFront() {
        ShieldControl(-this.PositionCounter);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        this.DamageCtrl = collision.gameObject.GetComponent<DamageControl>();
        if (this.DamageCtrl != null) {
            collision.gameObject.SetActive(false);
            this.HitPoints -= this.DamageCtrl.GetDamage();
            ControlShieldLife();
        }

    }

    private void ControlShieldLife() {
        if (this.HitPoints <= 0) {
            this.CanShoot = false;
            this.IsEnable = false;
            this.gameObject.SetActive(false);
        }
    }

    public void RestartShield() {
        this.CanShoot = true;
        this.IsEnable = true;
        this.gameObject.SetActive(true);
        this.transform.localPosition = this.Positions[0];
        this.transform.localRotation = this.Rotations[0];
        this.NormalShield();
    }

    public void NormalShield() {
        this.transform.localScale = new Vector3(1f, 1f, 1f);
        this.SetHitPoints(this.OriginalHitPoints);
    }

    public void BigShield() {
        this.RestartShield();
        this.transform.localScale = new Vector3(2.5f, 2.5f, 1f);
        this.SetHitPoints(this.OriginalHitPoints * 2);
    }

    public void Shoot() {    
        if (CanShoot) {
            for (int i = 0; i < this.ShootsPositions.Count; i++) {
                //objectPool.Spawn("PlayerBullet", this.ShootsPositions[i].position, asimov.transform.rotation);
                RedirectShoot(this.ShootsPositions[i]);
            }
        }

    }

    public void RedirectShoot(Transform shootpos) {
        Quaternion direction;
        if(this.PositionCounter == 0) {
            direction = asimov.GetMyBulletRotation();
        }
        else if(this.PositionCounter == 1) {
            direction = asimov.GetMyBulletRotation() * Quaternion.Euler(0f, 0f, -90f);
        }
        else if (this.PositionCounter == 2) {
            direction = asimov.GetMyBulletRotation() * Quaternion.Euler(0f, 0f, 180f);
        }
        else {
            direction = asimov.GetMyBulletRotation() * Quaternion.Euler(0f, 0f, 90f);
        }

        objectPool.Spawn("ShieldBullet", shootpos.position, direction);
    }

    #endregion


}
