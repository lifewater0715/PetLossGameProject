using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class FadeManager : MonoBehaviour
{
    public static FadeManager instance;
    
    [SerializeField] private string targetVisualElementName = "FadeScreen";
    private VisualElement _fadeScreen;

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

        _fadeScreen = GetComponent<UIDocument>().
        rootVisualElement.Q<VisualElement>(targetVisualElementName);
    }

    private void Start()
    {
        InputManager.instance.onKeyInputF += FadeIn;
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
        float fadeCount = 1f;
        while (fadeCount > 0f)
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            _fadeScreen.style.opacity = fadeCount;
        }
        _fadeScreen.pickingMode = PickingMode.Ignore;
    }

    private IEnumerator CFadeOut()
    {
        _fadeScreen.pickingMode = PickingMode.Position;
        float fadeCount = 0f;
        _fadeScreen.style.opacity = fadeCount;
        while (fadeCount < 1f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            _fadeScreen.style.opacity = fadeCount;
        }
    }
}
