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
        guideText.text = "이제 수건으로 물기를 닦아도 될 것 같아요!";
        StartGuide();
    }
    
    public void ShampooBtnClickText()
    {
        guideText.text = "브러시를 가지고 강아지를 문질러 거품을 내볼까요?";
    }

    public void ShowerBtnClickText()
    {
        guideText.text = "샤워기를 강아지한테 가져다 대보세요. 아마 좋아할거에요!";
    }

    public void TowelBtnClickText()
    {
        guideText.text = "강아지가 뽀송해지도록 닦아볼까요?";
    }
}
