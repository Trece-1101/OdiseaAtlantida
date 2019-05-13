using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgram : MonoBehaviour
{
    #region "Referencias en Cache"
    private Asimov asimov;
    private CrossHair crossHair;
    #endregion

    #region "Funciones"
    private void Start() {
        asimov = FindObjectOfType<Asimov>();
        crossHair = FindObjectOfType<CrossHair>();
        asimov.SetVelocity(new Vector2(4f, 4f));
        Cursor.visible = false;
    }

    private void Update() {
        crossHair.transform.position = Input.mousePosition;
    }
    

    #endregion
}
