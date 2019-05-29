using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{  

    [SerializeField] Texture2D myImage;
    private CursorMode cursorMode = CursorMode.Auto;
    private Camera myCamera;
    private Vector2 cursorPos = new Vector2(72f, 72f);


    private void Start() {
        //Cursor.visible = false;
        myCamera = Camera.main;
        Cursor.SetCursor(myImage, cursorPos, cursorMode);
    }

    private void Update() {
        cursorPos = myCamera.ScreenToWorldPoint(Input.mousePosition);
        //transform.position = cursorPos;
        
    }
}
