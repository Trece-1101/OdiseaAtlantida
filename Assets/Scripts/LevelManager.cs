using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private float waitSeconds = 2f;
    [SerializeField] private Animator animator;
    [SerializeField] private Slider slider;



    public void LoadStartMenu() {
        SceneManager.LoadScene(0);
    }

    public void LoadPrototype() {
        animator.SetTrigger("FadeOut");
        //StartCoroutine(WaitAndLoad("PrototypeLevel"));
        StartCoroutine(LoadLevelAsynch("PrototypeLevel"));
    }

    public void LoadGameOver() {
        StartCoroutine(WaitAndLoad("GameOver"));        
    }

    IEnumerator WaitAndLoad(string scene) {
        yield return new WaitForSeconds(waitSeconds);
        SceneManager.LoadScene(scene);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void LoadLevel(string sceneName) {
        StartCoroutine(LoadLevelAsynch(sceneName));
    }

    IEnumerator LoadLevelAsynch (string sceneName) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone) {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            slider.value = progress;

            //Debug.Log(progress);

            // espera un frame
            yield return null;
        }
    }

}
