using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    [SerializeField] Slider VolSlider = null;
    [SerializeField] private float DefaultVol = 0.8f;
    [SerializeField] Slider DificultySlider = null;
    [SerializeField] private float DefaultDif = 1f;
    MusicPlayer MusicPly;

    private void Start() {
        MusicPly = FindObjectOfType<MusicPlayer>();
        VolSlider.value = PlayerPrefController.GetMasterVolume();
        DificultySlider.value = PlayerPrefController.GetDificulty();
    }

    private void Update() {
        if (MusicPly) {
            MusicPly.SetVolume(VolSlider.value);
        }
        else {
            Debug.LogWarning("");
        }
    }

    public void SaveAndExit() {
        PlayerPrefController.SetMasterVolume(VolSlider.value);
        PlayerPrefController.SetDificulty(DificultySlider.value);
        FindObjectOfType<LevelManager>().LoadStartMenu();
    }

    public void SetDefaults() {
        VolSlider.value = this.DefaultVol;
        DificultySlider.value = this.DefaultDif;
    }

}
