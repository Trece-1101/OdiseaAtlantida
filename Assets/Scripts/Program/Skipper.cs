using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skipper : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        Skip();
    }

    private void Skip() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            FindObjectOfType<LevelLoader>().LoadPrototype();
        }
    }
}
