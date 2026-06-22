using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlackBorder : MonoBehaviour
{
    [SerializeField] private Image fadeScreen;

    private Coroutine fadeCoroutine;

    private void Start()
    {
        fadeScreen.gameObject.SetActive(false);
    }

    public void HalfFadeIn()
    {
        StartFade(CFadeIn(0.7f, 0f));
    }

    public void HalfFadeOut()
    {
        StartFade(CFadeOut(0f, 0.7f));
    }

    private void StartFade(IEnumerator fade)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(fade);
    }

    private IEnumerator CFadeIn(float startAlpha, float endAlpha)
    {
        fadeScreen.color = new Color(0f, 0f, 0f, startAlpha);
        fadeScreen.gameObject.SetActive(true);
        float alpha = startAlpha;
        while (alpha > endAlpha)
        {
            alpha -= 0.02f;
            yield return new WaitForSecondsRealtime(0.01f);
            fadeScreen.color = new Color(0f, 0f, 0f, alpha);
        }
        fadeScreen.gameObject.SetActive(false);
        fadeCoroutine = null;
    }

    private IEnumerator CFadeOut(float startAlpha, float endAlpha)
    {
        fadeScreen.color = new Color(0f, 0f, 0f, startAlpha);
        fadeScreen.gameObject.SetActive(true);
        float alpha = startAlpha;
        while (alpha < endAlpha)
        {
            alpha += 0.02f;
            yield return new WaitForSecondsRealtime(0.01f);
            fadeScreen.color = new Color(0f, 0f, 0f, alpha);
        }
        fadeCoroutine = null;
    }
}
