using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFirstRun : MonoBehaviour
{

    private void Start() {
        // solo sirve para el primer RUN
        // setea por Default los valores en las preferencias de usuario
        //PlayerPrefs.DeleteAll();  // OJO con descomentar esto, borra todos los valores
        PlayerPrefs.SetFloat("master_volume", PlayerPrefs.GetFloat("master_volume", 1f));
        PlayerPrefs.SetFloat("dificulty", PlayerPrefs.GetFloat("dificulty", 1f));
        PlayerPrefs.SetFloat("dificulty_modifier", PlayerPrefs.GetFloat("dificulty_modifier", 1f));
        PlayerPrefs.SetInt("crosshair", PlayerPrefs.GetInt("crosshair", 1));


    }


}
