using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextFade))]
public class PlayerRoomUIGuideText : MonoBehaviour
{
    [SerializeField] private TMP_Text guideText;
    
    private TextFade textFade;

    private Coroutine GuideCoroutine;

    private void Awake()
    {
        textFade = GetComponent<TextFade>();
        guideText.gameObject.SetActive(false);
    }

    public void StartGuide()
    {
        if (GuideCoroutine != null) return;

        GuideCoroutine = StartCoroutine(CStartGuide());
    }

    private IEnumerator CStartGuide()
    {
        textFade.StartGuideText(guideText);
        yield return new WaitForSecondsRealtime(0.2f);
        textFade.StopGuideText();
        yield return new WaitForSecondsRealtime(2.5f);
        GuideCoroutine = null;
    }
}
