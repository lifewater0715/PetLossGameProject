using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public static FadeManager instance;
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
        if (instance == null)
        {
            instance = this;
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
        StartCoroutine(CFadeIn());
    }

    public void FadeOut()
    {
        StartCoroutine(CFadeOut());
    }

    private IEnumerator CFadeIn()
    {
        float alpha = 1f;
        while (alpha > 0f)
        {
            alpha -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            fadeScreen.color = new Color(0f, 0f, 0f, alpha);
        }
        fadeScreen.gameObject.SetActive(false);
    }

    private IEnumerator CFadeOut()
    {
        fadeScreen.gameObject.SetActive(true);
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += 0.01f;
            yield return new WaitForSeconds(0.01f);
            fadeScreen.color = new Color(0f, 0f, 0f, alpha);
        }
    }
}
