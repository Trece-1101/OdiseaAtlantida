using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverMetrics : MonoBehaviour
{
    private int PlayerID { set; get; }
    private float Points { set; get; }
    private int Kills { set; get; }
    private float TimePlayed { set; get; }

    public void RecoverPlayMetrics() {
        GameMetrics data = SaveMetrics.LoadMetrics();

        PlayerID = data.PlayerID;
        Points = data.Points;
        Kills = data.Kills;
        TimePlayed = data.TimePlayed;
    }
}
