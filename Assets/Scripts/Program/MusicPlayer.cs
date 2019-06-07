using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    AudioSource AudioSrc;

    private void Start() {
        DontDestroyOnLoad(this);
        this.AudioSrc = GetComponent<AudioSource>();
        //this.AudioSrc.volume = PlayerPrefController.GetMasterVolume();
        this.AudioSrc.volume = 0.8f;
    }

    public void SetVolume(float value) {
        this.AudioSrc.volume = value;
    }

}
