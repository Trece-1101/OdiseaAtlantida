using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefController : MonoBehaviour
{
    const string MASTER_VOLUME = "master_volume";
    const float MIN_VOLUME = 0f;
    const float MAX_VOLUME = 1f;

    const string DIFICULTY = "dificulty";
    const float MIN_DIFICULTY = 0f;
    const float MAX_DIFICULTY = 2f;


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
        }
        else {
            Debug.LogError("");
        }
    }

    #endregion


}
