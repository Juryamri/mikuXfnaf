using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntroSequence : MonoBehaviour
{
    public player_movements playerMovement;
    public FirstPersonLook playerLook;

    public Image blackPanel;
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    public string[] lines;
    public float fadeDuration = 2f;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip dialogueClip;

    private int currentLine = 0;
    private bool introStarted = false;
    private bool canAdvance = false;

    void Start()
    {
        playerMovement.enabled = false;
        playerLook.enabled = false;

        dialoguePanel.SetActive(false);
        StartCoroutine(BeginIntro());
    }

    IEnumerator BeginIntro()
    {
        yield return StartCoroutine(FadeFromBlack());

        dialoguePanel.SetActive(true);
        introStarted = true;
        canAdvance = true;

        if (lines.Length > 0)
        {
            dialogueText.text = lines[currentLine];
            PlayDialogueSound();
        }
    }

    void Update()
    {
        if (!introStarted || !canAdvance)
            return;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            currentLine++;

            if (currentLine < lines.Length)
            {
                dialogueText.text = lines[currentLine];
                PlayDialogueSound();
            }
            else
            {
                EndIntro();
            }
        }
    }

    void PlayDialogueSound()
    {
        if (audioSource != null && dialogueClip != null)
        {
            audioSource.PlayOneShot(dialogueClip);
        }
    }

    IEnumerator FadeFromBlack()
    {
        float t = 0f;
        Color c = blackPanel.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            blackPanel.color = c;
            yield return null;
        }

        c.a = 0f;
        blackPanel.color = c;
    }

    void EndIntro()
    {
        dialoguePanel.SetActive(false);
        playerMovement.enabled = true;
        playerLook.enabled = true;
        introStarted = false;
    }
}