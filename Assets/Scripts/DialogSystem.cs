using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogSystem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textDisplay;
    [SerializeField] string[] sentences;
    [SerializeField] GameObject continueButton;
    [SerializeField] Animator textAnimator;
    [SerializeField] GameObject panel;
    private RectTransform panelRect;
    private Image panelImg;
    private AudioSource continueSound;
    private int indexSentences;
    private float typeSpeed = 0.05f;
    private Dictionary<string, List<float>> whoTalks;
    

    private void Start() {
        panelRect = panel.GetComponent<RectTransform>();
        panelImg = panel.GetComponent<Image>();
        whoTalks = new Dictionary<string, List<float>> {
            { "PlayerPos", new List<float>{422f, 752f} },
            { "EnemyPos", new List<float>{712f, 462f} },
            { "PlayerColor", new List<float>{0f, 255f, 255f, 255f} },
            { "EnemyColor", new List<float>{255f, 0f, 190f, 255f} }
        };
        continueSound = GetComponent<AudioSource>();
        StartCoroutine(TypeLetters());
    }

    private void Update() {
        if(textDisplay.text == sentences[indexSentences]) {
            continueButton.SetActive(true);
        }
    }

    private void checkSpeaker() {
        if (sentences[indexSentences].Contains("PLAYER")) {
            panelImg.color = new Color(whoTalks["PlayerColor"][0], whoTalks["PlayerColor"][1], whoTalks["PlayerColor"][2], whoTalks["PlayerColor"][3]);
            panelRect.transform.position = new Vector3(whoTalks["PlayerPos"][0], whoTalks["PlayerPos"][1], panelRect.transform.position.z);
        }
        else if (sentences[indexSentences].Contains("ENEMY")) {
            panelImg.color = new Color(whoTalks["EnemyColor"][0], whoTalks["EnemyColor"][1], whoTalks["EnemyColor"][2], whoTalks["EnemyColor"][3]);
            panelRect.transform.position = new Vector3(whoTalks["EnemyPos"][0], whoTalks["EnemyPos"][1], panelRect.transform.position.z);
        }
    }

    IEnumerator TypeLetters() {
        checkSpeaker();
        foreach (var letter in sentences[indexSentences].ToCharArray()) {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }

    }

    public void NextSentence() {
        continueSound.Play();
        textAnimator.SetTrigger("changeText");
        continueButton.SetActive(false);
        if (indexSentences < sentences.Length - 1) {
            indexSentences++;
            textDisplay.text = "";
            StartCoroutine(TypeLetters());
        }
        else {
            textDisplay.text = "";
            continueButton.SetActive(false);
        }
    }
}
