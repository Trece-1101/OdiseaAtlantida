using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameMetrics
{    
    public int PlayerID;
    public float Points;
    public int Kills;
    public float TimePlayed;

    public GameMetrics(GameSession gameSession) {
        PlayerID = Random.Range(0, 15000000);
        Points = gameSession.GetScore();
        Kills = gameSession.GetKillCount();
        TimePlayed = gameSession.GetPlayTime();
    }

}
