using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private float waitSeconds = 2f;
    [SerializeField] private Animator animator;

    public void LoadStartMenu() {
        SceneManager.LoadScene(0);
    }

    public void LoadPrototype() {
        animator.SetTrigger("FadeOut");
        StartCoroutine(WaitAndLoad("PrototypeLevel"));
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

}
