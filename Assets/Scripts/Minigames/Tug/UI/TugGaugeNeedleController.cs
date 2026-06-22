using UnityEngine;

public class TugGaugeNeedleController : MonoBehaviour
{
    [SerializeField] private TugGaugeController tugGaugeController;
    [SerializeField] private Transform needleTransform;
    [SerializeField] private float minAngle = -90f;
    [SerializeField] private float maxAngle = 90f;
    [SerializeField] private bool useSmoothRotation = true;
    [SerializeField] private float smoothSpeed = 15f;

    private void Awake()
    {
        if (needleTransform == null)
        {
            needleTransform = transform;
        }
    }

    private void LateUpdate()
    {
        if (tugGaugeController == null) return;
        if (needleTransform == null) return;

        float targetAngle = GetTargetAngle();
        RotateNeedle(targetAngle);
    }

    private float GetTargetAngle()
    {
        float gauge = Mathf.Clamp01(tugGaugeController.NormalizedGauge);
        return Mathf.Lerp(minAngle, maxAngle, gauge);
    }

    private void RotateNeedle(float targetAngle)
    {
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);

        if (!useSmoothRotation)
        {
            needleTransform.localRotation = targetRotation;
            return;
        }

        needleTransform.localRotation = Quaternion.Lerp(
            needleTransform.localRotation,
            targetRotation,
            Time.deltaTime * smoothSpeed);
    }
}
