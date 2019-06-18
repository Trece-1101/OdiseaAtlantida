//// Clase que controla la Musica a traves del juego

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    AudioSource AudioSrc;
    [SerializeField] private float Volumen = 0.05f;

    private void Start() {
        DontDestroyOnLoad(this);
        this.AudioSrc = GetComponent<AudioSource>();
        this.AudioSrc.volume = this.Volumen;
    }

    public void SetVolume(float value) {
        this.AudioSrc.volume = value;
    }

    public void Reset() {
        Destroy(gameObject);
    }

}
