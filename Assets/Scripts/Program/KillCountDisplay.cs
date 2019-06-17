using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KillCountDisplay : MonoBehaviour
{
    #region "Componentes en Cache"
    TextMeshProUGUI KillCountText;
    GameSession GameS;
    #endregion

    #region "Metodos"
    private void Start() {
        //this.GameProg = FindObjectOfType<GameProgram>()
        this.GameS = FindObjectOfType<GameSession>();
        this.KillCountText = GetComponent<TextMeshProUGUI>();
    }

    private void Update() {
        //this.ScoreText.SetText($"{GameProg.GetScore().ToString()} XP");
        this.KillCountText.SetText($"{this.GameS.GetKillCount().ToString()} Kills");
    }
    #endregion
}
