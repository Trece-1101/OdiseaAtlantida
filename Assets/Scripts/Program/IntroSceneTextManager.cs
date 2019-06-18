using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroSceneTextManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textDisplay = null;
    [SerializeField] string[] sentences = null;
    private int indexSentences;
    private float typeSpeed = 0.2f;

    private void Start() {
        StartCoroutine(TypeLetters());
    }

    IEnumerator TypeLetters() {
        foreach (var letter in sentences[indexSentences].ToCharArray()) {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }
        NextSentence();
    }

    public void NextSentence() {
        this.typeSpeed = 0.05f;
        textDisplay.text += "\n";
        if (indexSentences < sentences.Length - 1) {
            indexSentences++;
            StartCoroutine(TypeLetters());
        }
        else {
            FindObjectOfType<LevelLoader>().LoadPrototype();
        }
        
    }

}
