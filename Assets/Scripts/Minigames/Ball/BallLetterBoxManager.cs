using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BallLetterBoxManager : MonoBehaviour
{
    [SerializeField] private Image letterImageTop;
    [SerializeField] private Image letterImageBottom;
    [SerializeField] private BallThrowController ballThrowController;

    private void Awake()
    {
        letterImageTop.gameObject.SetActive(false);
        letterImageBottom.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        ballThrowController.OnChargeStarted += FadeOut;
        ballThrowController.OnThrow += FadeIn;
    }

    private void OnDisable()
    {
        ballThrowController.OnChargeStarted -= FadeOut;
        ballThrowController.OnThrow -= FadeIn;
    }

    public void FadeIn()
    {
        StartCoroutine(CFadeIn(1f, 0f));
    }

    public void FadeOut()
    {
        StartCoroutine(CFadeOut(0f, 1f));
    }

    public void HalfFadeIn()
    {
        StartCoroutine(CFadeIn(0.5f, 0f));
    }

    public void HalfFadeOut()
    {
        StartCoroutine(CFadeOut(0f, 0.5f));
    }


    private IEnumerator CFadeIn(float startAlpha, float endAlpha)
    {
        letterImageTop.color = new Color(0f, 0f, 0f, startAlpha);
        letterImageBottom.color = new Color(0f, 0f, 0f, startAlpha);

        letterImageTop.gameObject.SetActive(true);
        letterImageBottom.gameObject.SetActive(true);

        float alpha = startAlpha;
        while (alpha > endAlpha)
        {
            alpha -= 0.05f;
            yield return new WaitForSeconds(0.01f);
            letterImageTop.color = new Color(0f, 0f, 0f, alpha);
            letterImageBottom.color = new Color(0f, 0f, 0f, alpha);
        }
        letterImageTop.gameObject.SetActive(false);
        letterImageBottom.gameObject.SetActive(false);
    }

    private IEnumerator CFadeOut(float startAlpha, float endAlpha)
    {
        letterImageTop.color = new Color(0f, 0f, 0f, startAlpha);
        letterImageBottom.color = new Color(0f, 0f, 0f, startAlpha);

        letterImageTop.gameObject.SetActive(true);
        letterImageBottom.gameObject.SetActive(true);

        float alpha = startAlpha;
        while (alpha < endAlpha)
        {
            alpha += 0.05f;
            yield return new WaitForSeconds(0.01f);
            letterImageTop.color = new Color(0f, 0f, 0f, alpha);
            letterImageBottom.color = new Color(0f, 0f, 0f, alpha);
        }
    }
}
