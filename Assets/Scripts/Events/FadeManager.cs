using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance { get; private set; }
    [SerializeField] private Image fadeScreen;

#if UNITY_EDITOR
    [ContextMenu("Auto Assign")]
    private void AutoAssign()
    {
        fadeScreen = GameObject.Find("FadeScreen").GetComponent<Image>();
    }
#endif

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
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
        float alpha = startAlpha;
        while (alpha > endAlpha)
        {
            alpha -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            fadeScreen.color = new Color(0f, 0f, 0f, alpha);
        }
        fadeScreen.gameObject.SetActive(false);
    }

    private IEnumerator CFadeOut(float startAlpha, float endAlpha)
    {
        fadeScreen.gameObject.SetActive(true);
        float alpha = startAlpha;
        while (alpha < endAlpha)
        {
            alpha += 0.01f;
            yield return new WaitForSeconds(0.01f);
            fadeScreen.color = new Color(0f, 0f, 0f, alpha);
        }
    }
}
