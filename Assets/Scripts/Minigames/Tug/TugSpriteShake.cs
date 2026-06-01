using UnityEngine;

public class TugSpriteShake : MonoBehaviour
{
    [SerializeField] private bool playOnStart = true;
    [SerializeField] private Vector2 shakeDirection = new Vector2(1f, 0.2f);
    [SerializeField] private float positionAmount = 0.03f;
    [SerializeField] private float rotationAmount = 2f;
    [SerializeField] private float frequency = 25f;

    private Vector3 _originalLocalPosition;
    private Quaternion _originalLocalRotation;
    private bool _isShaking;
    private float _shakeStrength = 1f;

    private void Awake()
    {
        _originalLocalPosition = transform.localPosition;
        _originalLocalRotation = transform.localRotation;
    }

    private void OnEnable()
    {
        if (playOnStart)
        {
            StartShake();
        }
    }

    private void LateUpdate()
    {
        if (!_isShaking) return;

        Vector2 direction = shakeDirection.normalized;
        float shakeValue = Mathf.Sin(Time.time * frequency);
        Vector3 positionOffset = direction * positionAmount * _shakeStrength * shakeValue;
        float rotationOffset = rotationAmount * _shakeStrength * shakeValue;

        transform.localPosition = _originalLocalPosition + positionOffset;
        transform.localRotation = _originalLocalRotation * Quaternion.Euler(0f, 0f, rotationOffset);
    }

    public void StartShake()
    {
        _isShaking = true;
    }

    public void StopShake()
    {
        _isShaking = false;
        transform.localPosition = _originalLocalPosition;
        transform.localRotation = _originalLocalRotation;
    }

    public void SetShakeStrength(float strength)
    {
        _shakeStrength = Mathf.Clamp01(strength);
    }
}
