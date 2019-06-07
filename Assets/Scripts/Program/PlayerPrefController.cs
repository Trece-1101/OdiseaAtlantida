using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefController : MonoBehaviour
{
    const string MASTER_VOLUME = "master_volume";
    const float MIN_VOLUME = 0f;
    const float MAX_VOLUME = 1f;

    const string DIFICULTY = "dificulty";
    const string DIFICULTY_MODIFIER = "dificulty_modifier";
    const float MIN_DIFICULTY = 0f;
    const float MAX_DIFICULTY = 2f;

    const string CROSSHAIR = "crosshair";


    #region "Setters y Getters"
    public static float GetMasterVolume() {
        return PlayerPrefs.GetFloat(MASTER_VOLUME);
    }
    public static void SetMasterVolume(float value) {
        if(value >= MIN_VOLUME && value <= MAX_VOLUME) {
            PlayerPrefs.SetFloat(MASTER_VOLUME, value);
        }
        else {
            Debug.LogError("");
        }
    }

    public static float GetDificulty() {
        return PlayerPrefs.GetFloat(DIFICULTY);
    }
    public static void SetDificulty(float value) {
        if(value >= MIN_DIFICULTY && value <= MAX_DIFICULTY) {
            PlayerPrefs.SetFloat(DIFICULTY, value);
            SetDificultyModifier(value);
        }
        else {
            Debug.LogError("");
        }
    }

    public static float GetDificultyModifier() {
        return PlayerPrefs.GetFloat(DIFICULTY_MODIFIER);
    }
    public static void SetDificultyModifier(float value) {
        if(value == 0) {
            PlayerPrefs.SetFloat(DIFICULTY_MODIFIER, 0.75f);
        }
        else if(value > 0 && value <= 1) {
            PlayerPrefs.SetFloat(DIFICULTY_MODIFIER, 1f);
        }
        else {
            PlayerPrefs.SetFloat(DIFICULTY_MODIFIER, 1.25f);
        }
    }

    public static int GetCrosshair() {
        return PlayerPrefs.GetInt(CROSSHAIR);
    }
    public static void SetCrosshair(int value) {
        PlayerPrefs.SetInt(CROSSHAIR, value);
    }
    #endregion


}
