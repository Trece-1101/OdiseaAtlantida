using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    #region "Atributos Serializados"
    [SerializeField] Texture2D MyImage = null;
    #endregion

    #region "Atributos"
    private CursorMode CursorMode = CursorMode.Auto;
    private Camera MyCamera;
    private Vector2 CursorPos = new Vector2(72f, 72f);
    #endregion

    private void Start() {
        //Cursor.visible = false;
        MyCamera = Camera.main;
        Cursor.SetCursor(MyImage, CursorPos, CursorMode);
    }

    private void Update() {
        CursorPos = MyCamera.ScreenToWorldPoint(Input.mousePosition);
        //transform.position = cursorPos;
        
    }
}
