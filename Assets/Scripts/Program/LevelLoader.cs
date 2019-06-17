using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LevelLoader : MonoBehaviour
{
    #region "Atributos Serializados"
    [SerializeField] private Animator Animator = null;
    [SerializeField] private GameObject PauseUI = null;
    [SerializeField] private bool CanPause = false;
    #endregion

    #region "Atributos"
    private float WaitSeconds = 2f;
    public static bool IsPaused = false;
    #endregion

    #region "Componentes en Cache"
    private MouseCursor PersonalCursor;
    #endregion

    #region "Scenes Names"
    private string MainMenu = "MenuPrincipal";
    private string OptionsMenu = "Opciones";

    private string EnemiesShowDown = "EnemyShowDown";
    private string FirstEnemyShowDown = "EnemyShowDownOrange";
    private string LastEnemyShowDown = "EnemyShowDownSuicide";

    private string PowerUpsShowDown = "PowerUpShowDown";
    private string FirstPowerUpShowDown = "PowerUpShowDownSpeedUp";
    private string LastPowerUpShowDOwn = "PowerUpShowDownClone";
    #endregion

    #region "Setters y Getters"
    //public Slider GetSlider() {
    //    return this.Slider;
    //}
    //public void SetSlider(Slider value) {
    //    this.Slider = value;
    //}

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
    private void Start() {
        this.PersonalCursor = FindObjectOfType<MouseCursor>();

        this.Resume();
    }

    private void Update() {
        this.CheckPause();
        this.CheckToNextLevel();
    }

    private void CheckPause() {
        if (Input.GetKeyDown(KeyCode.Escape) && this.CanPause) {
            if (IsPaused) {
                this.Resume();
            }
            else {
                this.Pause();
            }
        }
    }


    public void Resume() {
        if(this.PersonalCursor == null) {
            Cursor.visible = false;
        }

        this.PauseAndResume(false, 1f);
 
    }

    private void Pause() {
        this.PauseAndResume(true, 0f);
    }

    private void PauseAndResume(bool value, float tScale) {
        this.PauseUI.SetActive(value);
        Time.timeScale = tScale;
        IsPaused = value;
    }

    public void ResetLevel() {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }


    public void LoadStartMenu() {        
        SceneManager.LoadScene(this.MainMenu);
    }

    public void LoadOptions() {
        SceneManager.LoadScene(this.OptionsMenu);
    }


    private void CheckToNextLevel() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            if (CheckCurrentSceneName().Contains(this.EnemiesShowDown)) {
                if(CheckCurrentSceneName() != this.LastEnemyShowDown) {
                    this.LoadNextLevel();
                }
                else {
                    this.LoadStartMenu();
                }
            }
            else if (CheckCurrentSceneName().Contains(this.PowerUpsShowDown)) {
                if (CheckCurrentSceneName() != this.LastPowerUpShowDOwn) {
                    this.LoadNextLevel();
                }
                else {
                    this.LoadStartMenu();
                }
            }
        }
    }

    private string CheckCurrentSceneName() {
        return SceneManager.GetActiveScene().name;
    }

    private void LoadNextLevel() {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }


    public void LoadEnemiesShowDown() {
        this.PersonalCursor = null;
        SceneManager.LoadScene(this.FirstEnemyShowDown);
    }

    public void LoadPowerUpsShowDown() {
        this.PersonalCursor = null;
        SceneManager.LoadScene(this.FirstPowerUpShowDown);
    }


    public void LoadPrototype() {
        this.Animator.SetTrigger("FadeOut");
        //StartCoroutine(WaitAndLoad("PrototypeLevel"));
        StartCoroutine(LoadLevelAsynch("PrototypeLevel"));
    }

    public void LoadGameOver() {
        StartCoroutine(WaitAndLoad("GameOver"));        
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

            //this.Slider.value = progress;

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
