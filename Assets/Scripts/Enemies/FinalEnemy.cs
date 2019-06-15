using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalEnemy : Enemy
{
    private int NumberOfMinions = 8;
    private List<GameObject> Minions = new List<GameObject>();
    private float DistanceToShip = 2f;
    private float DistanceToShip2 = 1.2f;
    private List<Vector3> Positions = new List<Vector3>();
    private bool MinionsRequested = false;
    private float TimeToRespawnMinions = 6f;
    private bool Below50 = false;
    private bool Below10 = false;

    public override void Awake() {
        base.Awake();
        Positions.Add(new Vector3(DistanceToShip, 0f, 0f));
        Positions.Add(new Vector3(0, DistanceToShip, 0f));
        Positions.Add(new Vector3(0, -DistanceToShip, 0f));
        Positions.Add(new Vector3(-DistanceToShip, 0, 0f));

        Positions.Add(new Vector3(DistanceToShip2, DistanceToShip2, 0f));
        Positions.Add(new Vector3(-DistanceToShip2, DistanceToShip2, 0f));
        Positions.Add(new Vector3(-DistanceToShip2, -DistanceToShip2, 0f));
        Positions.Add(new Vector3(DistanceToShip2, -DistanceToShip2, 0f));
        this.GiveMeMinions();
    }

    public override void Update() {
        base.Update();
        this.CheckMinions();
        this.CheckLife();
    }

    private void CheckMinions() {
        int remainingMinions = 0;
        foreach (var minion in this.Minions) {
            if (!minion.activeSelf) {
                remainingMinions++;
            }
        }

        if(remainingMinions == this.NumberOfMinions && !this.MinionsRequested) {
            this.MinionsRequested = true;
            Invoke("GiveMeMinions", this.TimeToRespawnMinions);
        }

    }

    private void CheckLife() {
        float lifePerc = this.HitPoints / this.GetOriginalHitPoints();
        if(lifePerc <= 0.50f && !Below50) {
            Below50 = true;
            this.ModifyAtributes(1.5f);
            this.RefillHealth(this.HitPoints * 0.2f);
            this.GetPool().Spawn("ParticleAnimation", this.transform.position, Quaternion.identity);
        }

        if(lifePerc <= 0.15f && !Below10) {
            Below10 = true;
            this.ModifyAtributes(0.5f);
            this.RefillHealth(this.HitPoints * 1.5f);
            this.GetPool().Spawn("ParticleAnimation", this.transform.position, Quaternion.identity);
        }
    }

    private void ModifyAtributes(float modifier) {
        this.TimeBetweenBulletShoots = this.TimeBetweenBulletShoots / modifier;
        this.TimeBetweenMissileShoots = this.TimeBetweenMissileShoots / modifier;
    }

    public override void CheckRotation() {
        base.CheckRotation();
    }

    private void GiveMeMinions() {
        this.Minions.Clear();

        for (int i = 0; i < this.NumberOfMinions; i++) {
            GameObject minion = this.GetPool().Spawn("AtlasMinion", this.transform.position, this.transform.rotation);
            minion.transform.parent = this.transform;
            this.Minions.Add(minion);
        }

        for (int i = 0; i < this.Minions.Count; i++) {
            this.Minions[i].transform.localPosition = this.Positions[i];
        }

        this.MinionsRequested = false;
    }
}
