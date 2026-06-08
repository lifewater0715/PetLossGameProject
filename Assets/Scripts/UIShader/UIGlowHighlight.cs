using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIGlowHighlight : MonoBehaviour
{
    [SerializeField] private float fadeSpeed = 2f;
    [SerializeField] private float minPower = 0f;
    [SerializeField] private float maxPower = 1.5f;

    private Image targetImage;

    private Material _materialInstance;
    private Coroutine _glowRoutine;

    private static readonly int GlowPowerId = Shader.PropertyToID("_GlowPower");

    private void Awake()
    {
        targetImage = GetComponent<Image>();

        _materialInstance = Instantiate(targetImage.material);
        targetImage.material = _materialInstance;

        SetGlowPower(0f);
    }

    public void SetHighlight(bool active)
    {
        if (_glowRoutine != null)
        {
            StopCoroutine(_glowRoutine);
            _glowRoutine = null;
        }

        if (active)
            _glowRoutine = StartCoroutine(GlowLoop());
        else
            _glowRoutine = StartCoroutine(FadeOut());
    }

    private IEnumerator GlowLoop()
    {
        float t = 0f;

        while (true)
        {
            t += Time.deltaTime * fadeSpeed;

            float power = Mathf.Lerp(minPower, maxPower, Mathf.PingPong(t, 1f));
            SetGlowPower(power);

            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        float currentPower = _materialInstance.GetFloat(GlowPowerId);

        while (currentPower > 0.01f)
        {
            currentPower = Mathf.MoveTowards(
                currentPower,
                0f,
                Time.deltaTime * fadeSpeed * maxPower
            );

            SetGlowPower(currentPower);
            yield return null;
        }

        SetGlowPower(0f);
    }

    private void SetGlowPower(float value)
    {
        _materialInstance.SetFloat(GlowPowerId, value);
    }

    private void OnDestroy()
    {
        if (_materialInstance != null)
            Destroy(_materialInstance);
    }
}