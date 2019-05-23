using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    TextMeshProUGUI ScoreText;
    GameProgram GameProg;

    private void Start() {
        this.GameProg = FindObjectOfType<GameProgram>();
        this.ScoreText = GetComponent<TextMeshProUGUI>();
    }

    private void Update() {
        this.ScoreText.SetText($"{GameProg.GetScore().ToString()} XP");
    }
}
