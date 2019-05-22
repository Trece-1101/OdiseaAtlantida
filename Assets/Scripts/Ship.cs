using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ship : MonoBehaviour, IAttack
{
    #region "Atributos"
    private float HitPoints;
    private Vector2 Velocity;
    public float TimeBetweenShoots { get; set; }
    public float TimeBetweenMissileShoots { get; set; }
    public float RemainTimeForShootBullet { get; set; }
    public float RemainTimeForShootMissile { get; set; }
    public bool CanShoot { get; set; }
    //private float Reward;
    #endregion

    #region "Setters/Getters"
    public float GetHitPoints() {
        return this.HitPoints;
    }
    public void SetHitPoints(float value) {
        this.HitPoints = value;
    }

    public Vector2 GetVelocity() {
        return this.Velocity;
    }
    public void SetVelocity(Vector2 value) {
        this.Velocity = value;
    }    

    public float GetTimeBetweenShoots() {
        return this.TimeBetweenShoots;
    }
    public void SetTimeBetweenShoots(float value) {
        this.TimeBetweenShoots = value;
    }

    public float GetTimeBetweenMissileShoots() {
        return this.TimeBetweenMissileShoots;
    }
    public void SetTimeBetweenMissileShoots(float value) {
        this.TimeBetweenMissileShoots = value;
    }
    #endregion

    #region "Constructor"
    public Ship(float hitpoints, Vector2 velocity) {
        this.HitPoints = hitpoints;
        this.Velocity = velocity;
        
    }
    #endregion

    #region "Aux"
    private DamageControl damageControl;
    #endregion

    #region "Metodos"
    public abstract void Move();
    public abstract void Shoot();
    public abstract void CheckRotation();
    public abstract void Die();
    public abstract void PlayImpact();

    public float AngleWithCompensateRotation(Vector3 direction, int compensation) {
        var angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - compensation;
        return angle;
    }

    public Dictionary<string, Quaternion> Rotate(Vector3 dir, int shipAngleCompensation, int bulletAngleCompesation) {
        // vector_direccion_ataque = vector_posicion_mouse - vector_centro_camara // en el caso de nuestra nave
        // vector_direccion_ataque = vector_posicion_asimov // en el caso de los enemigos
 
        // angulo_rotacion = arcoseno(dy/dx) - X grados
        // 90 grados para compensar que el sprite tiene su 0° hacia el Norte y la camara tiene sus 0° hacia el Este en el caso de la asimov
        // var angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 90;
        var angle_ship = this.AngleWithCompensateRotation(dir, shipAngleCompensation);
        // el sprite del proyectil ya apunta hacia el Este asi que no tiene compensacion
        var angle_bullet = this.AngleWithCompensateRotation(dir, bulletAngleCompesation);
        var rotation_ship = Quaternion.AngleAxis(angle_ship, Vector3.forward);
        var rotation_bullet = Quaternion.AngleAxis(angle_bullet, Vector3.forward);
        // rotar el componente en el angulo calculado y con z como eje de rotacion
        transform.rotation = Quaternion.AngleAxis(angle_ship, Vector3.forward);
        Dictionary<string, Quaternion> rotations = new Dictionary<string, Quaternion>() {
            {"rotation_ship", rotation_ship },
            {"rotation_bullet", rotation_bullet },
            {"transform_rotation", transform.rotation }
        };

        return rotations;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        damageControl = collision.gameObject.GetComponent<DamageControl>();
        if (damageControl != null) {
            ReceiveDamage(damageControl);
            collision.gameObject.SetActive(false);
            PlayImpact();
        }
        else {
            Die();
        }
    }

    public void ReceiveDamage(DamageControl damageControl) {
        this.SetHitPoints(this.GetHitPoints() - damageControl.GetDamage());
        if(this.GetHitPoints() <= 0) {
            Die();
        }
    }

    





    #endregion
}
