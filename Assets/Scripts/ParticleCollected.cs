using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollected : MonoBehaviour
{
    private void OnEnable() {
        Invoke("End", 2f);
    }

    private void End() {
        gameObject.SetActive(false);
    }
}
