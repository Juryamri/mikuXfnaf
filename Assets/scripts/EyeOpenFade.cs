using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EyeOpenFade : MonoBehaviour
{
    public Image blackImage;
    public float fadeDuration = 2f;

    void Start()
    {
        StartCoroutine(OpenEyes());
    }

    IEnumerator OpenEyes()
    {
        Color c = blackImage.color;
        c.a = 1f;
        blackImage.color = c;

        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            blackImage.color = c;
            yield return null;
        }

        c.a = 0f;
        blackImage.color = c;
        blackImage.gameObject.SetActive(false);
    }
}