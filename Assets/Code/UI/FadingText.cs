using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadingText : MonoBehaviour
{
    public float fadeOutTime;
    public float fadeInTime;
    public float clearTime;

    public int fadesPerSecond;

    Text text;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    public void StartFade(string message)
    {
        Color tempColor = text.color;
        text.color = tempColor;

        text.text = message;
        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        while (text.color.a <= 1)
        {

            Color tempColor = text.color;
            tempColor.a += (float)1 / (fadeInTime * fadesPerSecond);

            text.color = tempColor;
            yield return new WaitForSeconds((float)1 / fadesPerSecond);

        }
        new WaitForSeconds(clearTime);
        StartCoroutine(FadeOut());
        StopCoroutine(FadeIn());
    }

    private IEnumerator FadeOut()
    {
        while (text.color.a >= 0)
        {
            Color tempColor = text.color;
            tempColor.a -= (float)1 / (fadeOutTime * fadesPerSecond);

            text.color = tempColor;

            yield return new WaitForSeconds((float)1 / fadesPerSecond);

        }
        StopCoroutine(FadeOut());
    }
}
