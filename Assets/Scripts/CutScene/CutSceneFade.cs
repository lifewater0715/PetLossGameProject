using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneFade : MonoBehaviour
{
    [SerializeField] private Image cutScene;

    private void Start()
    {
        cutScene.gameObject.SetActive(false);
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
        cutScene.color = new Color(1f, 1f, 1f, startAlpha);
        cutScene.gameObject.SetActive(true);
        float alpha = startAlpha;
        while (alpha < endAlpha)
        {
            alpha += 0.01f;
            yield return new WaitForSeconds(0.01f);
            cutScene.color = new Color(1f, 1f, 1f, alpha);
        }
    }

    private IEnumerator CFadeOut(float startAlpha, float endAlpha)
    {
        cutScene.color = new Color(1f, 1f, 1f, startAlpha);
        cutScene.gameObject.SetActive(true);
        float alpha = startAlpha;
        while (alpha > endAlpha)
        {
            alpha -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            cutScene.color = new Color(1f, 1f, 1f, alpha);
        }
        cutScene.gameObject.SetActive(false);
    }
}
