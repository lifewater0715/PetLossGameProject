using System.Collections;
using TMPro;
using UnityEngine;

public class TextFade : MonoBehaviour
{
    [SerializeField] private bool _isTextShow = false;
    private Coroutine GuideTextCoroutine;

    public void StartGuideText(TMP_Text targetText)
    {
        targetText.gameObject.SetActive(true);
        _isTextShow = true;
        GuideTextCoroutine = StartCoroutine(CStartGuideText(0f, 1f, targetText));
    }

    public void StopGuideText()
    {
        if (GuideTextCoroutine == null) return;

        _isTextShow = false;
        GuideTextCoroutine = null;
    }

    private IEnumerator CStartGuideText(
        float startAlpha, float endAlpha, TMP_Text targetText)
    {
        targetText.color = new Color(1f, 0.5f, 0f, startAlpha);
        float alpha = startAlpha;
        float originalAlpha = targetText.color.a;

        while (_isTextShow)
        {
            while (alpha < endAlpha)
            {
                alpha += 0.01f;
                yield return new WaitForSecondsRealtime(0.01f);
                targetText.color = new Color(1f, 0.5f, 0f, alpha);
            }

            while (alpha > originalAlpha)
            {
                alpha -= 0.01f;
                yield return new WaitForSecondsRealtime(0.01f);
                targetText.color = new Color(1f, 0.5f, 0f, alpha);
            }
        }

        targetText.gameObject.SetActive(false);
    }
}
