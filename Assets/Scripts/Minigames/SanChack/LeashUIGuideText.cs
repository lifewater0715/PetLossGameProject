using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextFade))]
public class LeashUIGuideText : MonoBehaviour
{
    [SerializeField] private TMP_Text guideText;
    
    private TextFade textFade;

    private void Awake()
    {
        textFade = GetComponent<TextFade>();
        guideText.gameObject.SetActive(false);
        textFade.StartGuideText(guideText);
    }
    
    public void StopGuide()
    {
        textFade.StopGuideText();
    }
}
