using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSequence : MonoBehaviour
{
    [System.Serializable]
    public class SpriteEntry
    {
        public string key;
        public Sprite sprite;
    }

    public enum Speaker
    {
        Player,
        Miku
    }

    [System.Serializable]
    public class DialogueLine
    {
        public Speaker speaker;
        [TextArea(2, 4)] public string text;
        public string spriteKey;
        public AudioClip voiceClip;
    }

    [Header("UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;
    public Image portraitImage;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip textBlip;
    public float textSpeed = 0.04f;
    public float blipInterval = 0.05f;

    [Header("Sprites")]
    public SpriteEntry[] spriteEntries;

    [Header("Dialogue")]
    public DialogueLine[] lines;

    private int currentIndex = 0;
    private bool isTyping = false;
    private bool skipTyping = false;
    private Coroutine typingRoutine;

    void Start()
    {
        dialoguePanel.SetActive(true);
        ShowLine();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                skipTyping = true;
            }
            else
            {
                NextLine();
            }
        }
    }

    void ShowLine()
    {
        if (currentIndex >= lines.Length)
        {
            dialoguePanel.SetActive(false);
            return;
        }

        DialogueLine line = lines[currentIndex];

        speakerNameText.text = line.speaker == Speaker.Miku ? "Miku" : "Me";

        if (!string.IsNullOrEmpty(line.spriteKey))
        {
            Sprite s = GetSprite(line.spriteKey);
            if (s != null)
                portraitImage.sprite = s;
        }

        if (audioSource.isPlaying)
            audioSource.Stop();

        if (line.voiceClip != null)
            audioSource.PlayOneShot(line.voiceClip);

        if (typingRoutine != null)
            StopCoroutine(typingRoutine);

        typingRoutine = StartCoroutine(TypeText(line.text));
    }

    IEnumerator TypeText(string fullText)
    {
        isTyping = true;
        skipTyping = false;
        dialogueText.text = "";

        float lastBlipTime = -999f;

        foreach (char c in fullText)
        {
            if (skipTyping)
            {
                dialogueText.text = fullText;
                break;
            }

            dialogueText.text += c;

            if (textBlip != null && !char.IsWhiteSpace(c))
            {
                if (Time.time - lastBlipTime >= blipInterval)
                {
                    audioSource.PlayOneShot(textBlip);
                    lastBlipTime = Time.time;
                }
            }

            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
    }

    void NextLine()
    {
        currentIndex++;
        ShowLine();
    }

    Sprite GetSprite(string key)
    {
        foreach (var entry in spriteEntries)
        {
            if (entry.key == key)
                return entry.sprite;
        }
        return null;
    }
}