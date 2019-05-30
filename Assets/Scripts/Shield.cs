using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, IAttack
{    
    private int positionCounter;
    private List<Vector3> positions = new List<Vector3>();
    private List<Quaternion> rotations = new List<Quaternion>();

    [SerializeField] private Transform shootPoint = null;
    public float TimeBetweenBulletShoots { get; set; }
    public float TimeBetweenMissileShoots { get; set; }
    private ObjectPool objectPool;
    public float RemainTimeForShootBullet { get; set; }
    public float RemainTimeForShootMissile { get; set; }
    public bool CanShoot { get; set; }
    public bool CanShootMissile { get; set; }

    private void Start() {
        this.objectPool = ObjectPool.Instance;

        TimeBetweenBulletShoots = 0.8f;

        //this.CanShoot = true;
        this.RemainTimeForShootBullet = this.TimeBetweenBulletShoots;
        this.RemainTimeForShootMissile = this.TimeBetweenMissileShoots;

        
        positions.Add(new Vector3(0f, 0.4f, 0f)); // escudo arriba
        positions.Add(new Vector3(0.4f, 0f, 0f)); // escudo derecha
        positions.Add(new Vector3(0f, -0.4f, 0f)); // escudo abajo        
        positions.Add(new Vector3(-0.4f, 0f, 0f)); // escudo izquierda

        rotations.Add(Quaternion.Euler(0f, 0f, 270f)); // escudo arriba
        rotations.Add(Quaternion.Euler(0f, 0f, 180f)); // escudo derecha
        rotations.Add(Quaternion.Euler(0f, 0f, 90f)); // escudo abajo        
        rotations.Add(Quaternion.Euler(0f, 0f, 0f)); // escudo izquierda

  
        positionCounter = 0;
        ShieldControl(positionCounter);
        this.CanShoot = true;
    }

    public void ShieldControl(int value) {
        positionCounter += value;             

        if(positionCounter > 3) {
            positionCounter = 0;
        }

        if(positionCounter < 0) {
            positionCounter = 3;
        }

        CanShoot = positionCounter == 0 ? true : false; // solo dispara cuando el escudo esta frontal
        this.transform.localPosition = positions[positionCounter];
        this.transform.localRotation = rotations[positionCounter];

    }

    public void GetShieldOnFront() {
        ShieldControl(-positionCounter);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.name == "EnemyBullet(Clone)" || collision.gameObject.name == "MissileEnemy(Clone)") {
            Shoot(collision.gameObject.transform.rotation);
            collision.gameObject.SetActive(false);
        }
        if(collision.gameObject.tag == "Enemy") {
            Destroy(collision.gameObject);
        }
    }

    public void Shoot() { }

    public void Shoot(Quaternion rotation) {
        //Debug.DrawLine(transform.position, Vector3.up * 10, Color.red);
        //Quaternion.Inverse(rotation)
        //Debug.Log(rotation);
        //Vector3 angles = rotation.eulerAngles - new Vector3(0f, 0f, 90f);
        //rotation = Quaternion.Euler(angles);
        ////Debug.Log(angles);
        //Debug.Log(rotation);

        if (CanShoot) {
            objectPool.Spawn("PlayerBullet", shootPoint.position, rotation);
        }
    }

}
