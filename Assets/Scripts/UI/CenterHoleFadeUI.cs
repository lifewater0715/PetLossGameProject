using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CenterHoleFadeUI : MonoBehaviour
{
    [SerializeField] private Image targetImage;
    [SerializeField] private float duration = 1f;

    private Material _material;
    private Coroutine _playCoroutine;

    private static readonly int ProgressId = Shader.PropertyToID("_Progress");

    private void Awake()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();
    }

    public void Initialize(Image image, float newDuration, float unusedEndRadius, Material sourceMaterial = null)
    {
        targetImage = image;
        duration = Mathf.Max(0.01f, newDuration);

        ApplyMaterial(sourceMaterial);
        ResetToClosed(false);
    }

    public void SetDuration(float newDuration)
    {
        duration = Mathf.Max(0.01f, newDuration);
    }

    public void SetEndRadius(float unusedEndRadius)
    {
    }

    public void Play()
    {
        if (_playCoroutine != null)
            StopCoroutine(_playCoroutine);

        _playCoroutine = StartCoroutine(CPlay());
    }

    public IEnumerator PlayAndWait()
    {
        if (_playCoroutine != null)
        {
            StopCoroutine(_playCoroutine);
            _playCoroutine = null;
        }

        yield return CPlay();
    }

    public void ResetToClosed(bool active)
    {
        gameObject.SetActive(active);
        SetProgress(0f);
    }

    public void OpenImmediate()
    {
        SetProgress(1f);
        gameObject.SetActive(false);
    }

    private void ApplyMaterial(Material sourceMaterial)
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();

        if (targetImage == null)
            return;

        if (_material != null)
            Destroy(_material);

        if (sourceMaterial != null)
        {
            _material = Instantiate(sourceMaterial);
        }
        else
        {
            Shader shader = Shader.Find("UI/Center Hole Fade");
            if (shader != null)
                _material = new Material(shader);
        }

        if (_material != null)
        {
            _material.name = "M_CenterHole_Runtime";
            targetImage.material = _material;
            SetProgress(0f);
        }
    }

    private IEnumerator CPlay()
    {
        gameObject.SetActive(true);
        SetProgress(0f);

        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            float t = Mathf.Clamp01(time / duration);
            t = Mathf.SmoothStep(0f, 1f, t);

            SetProgress(t);

            yield return null;
        }

        SetProgress(1f);

        yield return null;

        gameObject.SetActive(false);
        _playCoroutine = null;
    }

    private void SetProgress(float value)
    {
        if (_material != null)
            _material.SetFloat(ProgressId, value);
    }
}