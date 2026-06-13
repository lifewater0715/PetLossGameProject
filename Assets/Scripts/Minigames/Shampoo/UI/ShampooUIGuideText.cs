using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextFade))]
public class ShampooUIGuideText : MonoBehaviour
{
    [SerializeField] private TMP_Text guideText;
    
    private TextFade textFade;

    private void Awake()
    {
        textFade = GetComponent<TextFade>();
        guideText.gameObject.SetActive(false);
        StartGuide();
    }

    public void StartGuide()
    {
        textFade.StartGuideText(guideText);
    }

    public void StopGuide()
    {
        textFade.StopGuideText();
    }

    public void EndToolShampoo()
    {
        guideText.text = "샤워기로 거품을 헹궈내보세요!";
        StartGuide();
    }

    public void EndToolShower()
    {
        guideText.text = "강아지가 뽀송해졌어요! 이제 수건으로 닦아볼까요?";
        StartGuide();
    }
}
