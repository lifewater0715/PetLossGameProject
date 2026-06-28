using UnityEngine;
using UnityEngine.UI;

public class TugChargeSliderController : MonoBehaviour
{
    [SerializeField] private TugSystemManager tugSystemManager;
    [SerializeField] private Slider chargeSlider;
    [SerializeField] private float decreaseSpeed = 4f;
    [SerializeField] private float slowDownStartValue = 0.25f;
    [SerializeField] private float minDecreaseSpeedMultiplier = 0.005f;

    private void Awake()
    {
        if (chargeSlider == null)
        {
            chargeSlider = GetComponent<Slider>();
        }
    }

    private void Start()
    {
        if (tugSystemManager == null) return;
        if (chargeSlider == null) return;

        chargeSlider.normalizedValue = tugSystemManager.NormalizedCharged;
    }

    private void LateUpdate()
    {
        if (tugSystemManager == null) return;
        if (chargeSlider == null) return;

        float targetValue = tugSystemManager.NormalizedCharged;

        if (targetValue >= chargeSlider.normalizedValue)
        {
            chargeSlider.normalizedValue = targetValue;
            return;
        }

        float decreaseStep = GetDecreaseStep();

        chargeSlider.normalizedValue = Mathf.MoveTowards(
            chargeSlider.normalizedValue,
            targetValue,
            decreaseStep);
    }

    private float GetDecreaseStep()
    {
        float speedMultiplier = 1f;
        float clampedSlowDownStartValue = Mathf.Clamp01(slowDownStartValue);
        float clampedMinMultiplier = Mathf.Clamp01(minDecreaseSpeedMultiplier);

        if (clampedSlowDownStartValue > 0f && chargeSlider.normalizedValue < clampedSlowDownStartValue)
        {
            float slowDownRate = chargeSlider.normalizedValue / clampedSlowDownStartValue;
            speedMultiplier = Mathf.Lerp(clampedMinMultiplier, 1f, slowDownRate);
        }

        return Time.deltaTime * decreaseSpeed * speedMultiplier;
    }
}
