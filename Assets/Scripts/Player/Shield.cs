using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, IAttack, IDefense
{

    #region "Atritutos Serializados"
    [Header("Shoot")]
    [SerializeField] private List<Transform> ShootsPositions = null;
    #endregion

    #region "Atributos"
    private int PositionCounter;
    private List<Vector3> Positions = new List<Vector3>();
    private List<Quaternion> Rotations = new List<Quaternion>();
    public float HitPoints { get; set; }
    public float OriginalHitPoints { get; set; }
    private bool IsEnable;
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
    private Animator MyAnimator;
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
        this.MyAnimator = GetComponent<Animator>();

        TimeBetweenBulletShoots = 0.8f;
        this.OriginalHitPoints = 150;
        this.HitPoints = this.OriginalHitPoints;

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
            ReceiveDamage(this.DamageCtrl);
        }

    }

    public void ReceiveDamage(DamageControl damageControl) {
        this.HitPoints -= this.DamageCtrl.GetDamage();
        ControlShieldLife();
    }

    private void ControlShieldLife() {
        var lifePercentage = this.HitPoints / this.OriginalHitPoints;
        if (this.HitPoints <= 0) {
            this.CanShoot = false;
            this.IsEnable = false;
            this.Die();
        }

        if (lifePercentage > 0.75) {
            this.MyAnimator.SetTrigger("HighLife");
        }
        else if (lifePercentage < 0.75 && lifePercentage > 0.25) {
            this.MyAnimator.SetTrigger("HalfLife");
        }
        else {
            this.MyAnimator.SetTrigger("LowLife");
        }

        
    }

    public void Die() {
        this.gameObject.SetActive(false);
    }

    public void RestartShield() {
        this.CanShoot = true;
        this.IsEnable = true;
        this.gameObject.SetActive(true);
        this.transform.localPosition = this.Positions[0];
        this.transform.localRotation = this.Rotations[0];
        this.transform.localScale = new Vector3(1f, 1f, 1f);
        this.HitPoints = this.OriginalHitPoints;
        this.MyAnimator.SetTrigger("HighLife");
    }

    public void NormalShield() {                
        this.RestartShield();
    }

    public void BigShield() {
        this.RestartShield();
        this.transform.localScale = new Vector3(2.5f, 2.5f, 1f);
        this.HitPoints = this.OriginalHitPoints * 2;
    }

    public void Shoot() {  
        if (CanShoot) {
            for (int i = 0; i < this.ShootsPositions.Count; i++) {
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
