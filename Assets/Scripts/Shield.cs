using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, IAttack
{
    private Vector3 upPos;
    private Vector3 downPos;
    private Vector3 rightPos;
    private Vector3 leftPos;
    private Quaternion noRotation;
    private Quaternion rotated;
    private int positionCounter;
    private List<Vector3> positions = new List<Vector3>();
    [SerializeField] private Transform shootPoint;
    public float TimeBetweenShoots { get; set; }
    public float TimeBetweenMissileShoots { get; set; }
    private ObjectPool objectPool;
    public float RemainTimeForShootBullet { get; set; }
    public float RemainTimeForShootMissile { get; set; }
    public bool CanShoot { get; set; }

    private void Start() {
        this.objectPool = ObjectPool.Instance;

        TimeBetweenShoots = 0.8f;

        this.CanShoot = true;
        this.RemainTimeForShootBullet = this.TimeBetweenShoots;
        this.RemainTimeForShootMissile = this.TimeBetweenMissileShoots;

        upPos = new Vector3(0f, 0.55f, 0f);        
        downPos = new Vector3(0f, -0.55f, 0f);        
        rightPos = new Vector3(0.55f, 0f, 0f);        
        leftPos = new Vector3(-0.55f, 0f, 0f);
        positions.Add(upPos);
        positions.Add(rightPos);
        positions.Add(downPos);
        positions.Add(leftPos);       

        noRotation = Quaternion.Euler(0f, 0f, 0f);
        rotated = Quaternion.Euler(0f, 0f, 90f);
        positionCounter = 0;
        ShieldControl(positionCounter);
    }

    public void ShieldControl(int value) {
        positionCounter += value;
        if(positionCounter > 3) {
            positionCounter = 0;
        }

        if(positionCounter < 0) {
            positionCounter = 3;
        }

        if(positionCounter == 0 || positionCounter == 2) {
            this.transform.localRotation = rotated;
        }
        else {
            this.transform.localRotation = noRotation;
        }

        this.transform.localPosition = positions[positionCounter];

    }

    public void Shoot() {
        //if (this.RemainTimeForShootBullet <= 0 && this.CanShoot) {
        //    objectPool.Spawn("PlayerBullet", shootPoint.position, this.transform.localRotation);

        //    this.RemainTimeForShootBullet = this.TimeBetweenShoots;
        //}
        //else {
        //    this.RemainTimeForShootBullet -= Time.deltaTime;
        //}

       
    }

    public void Shoot(Quaternion rotation) {
        //if (this.RemainTimeForShootBullet <= 0) {
        //    objectPool.Spawn("PlayerBullet", shootPoint.position, rotation);

        //    this.RemainTimeForShootBullet = this.TimeBetweenShoots;
        //}
        //else {
        //    this.RemainTimeForShootBullet -= Time.deltaTime;
        //}


    }

}
