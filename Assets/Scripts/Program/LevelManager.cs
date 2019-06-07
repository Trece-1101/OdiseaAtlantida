using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region "Atributos Serializados"
    [SerializeField] private Slider Slider = null;
    [SerializeField] private Animator Animator = null;
    #endregion

    #region "Atributos"
    private float WaitSeconds = 2f;
    #endregion

    #region "Setters y Getters"
    public Slider GetSlider() {
        return this.Slider;
    }
    public void SetSlider(Slider value) {
        this.Slider = value;
    }

    public Animator GetAnimator() {
        return this.Animator;
    }
    public void SetAnimator(Animator value) {
        this.Animator = value;
    }

    public float GetWaitSeconds() {
        return this.WaitSeconds;
    }
    public void SetWaitSeconds(float value) {
        this.WaitSeconds = value;
    }
    #endregion

    #region "Metodos"
    public void LoadStartMenu() {        
        SceneManager.LoadScene(0);
    }

    public void LoadPrototype() {
        this.Animator.SetTrigger("FadeOut");
        //StartCoroutine(WaitAndLoad("PrototypeLevel"));
        StartCoroutine(LoadLevelAsynch("PrototypeLevel"));
    }

    public void LoadGameOver() {
        StartCoroutine(WaitAndLoad("GameOver"));        
    }

    public void LoadOptions() {
        StartCoroutine(WaitAndLoad("Opciones"));
    }

    IEnumerator WaitAndLoad(string scene) {
        yield return new WaitForSeconds(this.WaitSeconds);
        SceneManager.LoadScene(scene);
    }


    public void LoadLevel(string sceneName) {
        Time.timeScale = 1;
        StartCoroutine(LoadLevelAsynch(sceneName));
    }

    IEnumerator LoadLevelAsynch (string sceneName) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone) {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            this.Slider.value = progress;

            //Debug.Log(progress);

            // espera un frame
            yield return null;
        }
    }

    public void QuitGame() {
        Application.Quit();
    }
    #endregion


}
