using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextFade))]
public class BallUIGuideText : MonoBehaviour
{
    [SerializeField] private TMP_Text guideText;
    [SerializeField] private BallThrowController ballThrowController;
    
    private TextFade textFade;

    private void Awake()
    {
        textFade = GetComponent<TextFade>();
        guideText.gameObject.SetActive(false);
        textFade.StartGuideText(guideText);
    }

    private void OnEnable()
    {
        ballThrowController.OnChargeStarted += StopGuide;
    }

    private void OnDisable()
    {
        ballThrowController.OnChargeStarted -= StopGuide;
    }

    public void StopGuide()
    {
        textFade.StopGuideText();
    }
}
