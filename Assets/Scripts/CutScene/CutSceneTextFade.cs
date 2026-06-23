using System.Collections;
using TMPro;
using UnityEngine;

public class CutSceneTextFade : MonoBehaviour
{
    [SerializeField] private TMP_Text cutSceneText;

    private void Start()
    {
        cutSceneText.gameObject.SetActive(false);
    }

    public void FadeIn()
    {
        StartCoroutine(CFadeIn(0f, 1f));
    }

    public void FadeOut()
    {
        StartCoroutine(CFadeOut(1f, 0f));
    }

    private IEnumerator CFadeIn(float startAlpha, float endAlpha)
    {
        cutSceneText.color = new Color(1f, 0.9f, 0.5f, startAlpha);
        cutSceneText.gameObject.SetActive(true);
        float alpha = startAlpha;
        while (alpha < endAlpha)
        {
            alpha += 0.01f;
            yield return new WaitForSecondsRealtime(0.01f);
            cutSceneText.color = new Color(1f, 0.9f, 0.5f, alpha);
        }
    }

    private IEnumerator CFadeOut(float startAlpha, float endAlpha)
    {
        cutSceneText.color = new Color(1f, 0.9f, 0.5f, startAlpha);
        cutSceneText.gameObject.SetActive(true);
        float alpha = startAlpha;
        while (alpha > endAlpha)
        {
            alpha -= 0.01f;
            yield return new WaitForSecondsRealtime(0.01f);
            cutSceneText.color = new Color(1f, 0.9f, 0.5f, alpha);
        }
        cutSceneText.gameObject.SetActive(false);
    }
}
