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
    #endregion

    #region "Metodos"
    private void Start() {
        this.GameProg = FindObjectOfType<GameProgram>();
        this.ScoreText = GetComponent<TextMeshProUGUI>();
    }

    private void Update() {
        this.ScoreText.SetText($"{GameProg.GetScore().ToString()} XP");
    }
    #endregion
}
