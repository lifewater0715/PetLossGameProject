using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextFade))]
public class PlayerRoomTutorialUIGuideText : MonoBehaviour
{
    [SerializeField] private TMP_Text guideText;
    
    private TextFade textFade;

    private void Awake()
    {
        textFade = GetComponent<TextFade>();
        guideText.gameObject.SetActive(false);
    }

    public void StartGuide()
    {
        textFade.StartGuideText(guideText);
    }

    public void StopGuide()
    {
        textFade.StopGuideText();
    }
}
