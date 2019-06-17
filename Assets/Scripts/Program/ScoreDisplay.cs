//// Clase que controla el display en pantalla del Score

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    #region "Componentes en Cache"
    TextMeshProUGUI ScoreText;
    GameProgram GameProg;
    GameSession GameS;
    #endregion

    #region "Metodos"
    private void Start() {
        //this.GameProg = FindObjectOfType<GameProgram>()
        this.GameS = FindObjectOfType<GameSession>();
        this.ScoreText = GetComponent<TextMeshProUGUI>();
    }

    private void Update() {
        //this.ScoreText.SetText($"{GameProg.GetScore().ToString()} XP");
        this.ScoreText.SetText($"{GameS.GetScore().ToString()} XP");
    }
    #endregion
}
