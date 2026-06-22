using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextFade))]
public class PictureUIGuideText : MonoBehaviour
{
    [SerializeField] private TMP_Text guideText;
    [SerializeField] private float rememberTime = 10f;
    
    private TextFade textFade;
    private bool stopText = false;
    private float _rememberTimer;

    private void Awake()
    {
        textFade = GetComponent<TextFade>();
        guideText.gameObject.SetActive(false);
        textFade.StartGuideText(guideText);
    }

    private void Update()
    {
        if (_rememberTimer >= rememberTime && stopText)
        {
            stopText = false;
            textFade.StartGuideText(guideText);
        }

        if (_rememberTimer < rememberTime && stopText)
        {
            _rememberTimer += Time.deltaTime;
        }
    }

    public void StopGuide()
    {
        _rememberTimer = 0f;
        if (stopText) return;

        textFade.StopGuideText();
        stopText = true;
    }
}
