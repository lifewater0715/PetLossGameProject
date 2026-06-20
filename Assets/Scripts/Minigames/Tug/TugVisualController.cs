using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TugVisualController : MonoBehaviour
{
    [SerializeField] private TugGaugeController tugGaugeController;
    [SerializeField] private Transform dogRoot;
    [SerializeField] private Transform dogShakeVisualTop;
    [SerializeField] private Transform dogShakeVisualBottom;
    [SerializeField] private List<Transform> dogMovePoints = new List<Transform>();
    [SerializeField] private SpriteRenderer dogSpriteRendererTop;
    [SerializeField] private SpriteRenderer dogSpriteRendererBottom;
    [SerializeField] private Sprite dogIdleSpriteTop;
    [SerializeField] private Sprite dogIdleSpriteBottom;
    [SerializeField] private Sprite dogPlayerPullSpriteTop;
    [SerializeField] private Sprite dogPlayerPullSpriteBottom;
    [SerializeField] private Transform armTugVisual;
    [SerializeField] private SpriteRenderer armTugSpriteRenderer;
    [SerializeField] private Sprite[] armTugSprites;
    [SerializeField] private TugSpriteShake dogShakeTop;
    [SerializeField] private TugSpriteShake dogShakeBottom;
    [SerializeField] private TugSpriteShake armTugShake;
    [SerializeField] private float maxShakeStrength = 1f;
    [SerializeField] private float fadeDuration = 0.15f;

    private readonly List<SpriteRenderer> _fadeRenderers = new List<SpriteRenderer>();
    private Coroutine _fadeCoroutine;
    private int _currentDogMoveIndex = -1;
    private int _currentArmTugIndex = -1;
    private int _targetDogMoveIndex = -1;
    private int _targetArmTugIndex = -1;

    private void Awake()
    {
        if (dogShakeVisualTop != null && dogShakeTop == null)
        {
            dogShakeTop = dogShakeVisualTop.GetComponent<TugSpriteShake>();
        }

        if (dogShakeVisualBottom != null && dogShakeBottom == null)
        {
            dogShakeBottom = dogShakeVisualBottom.GetComponent<TugSpriteShake>();
        }

        if (dogShakeVisualTop != null && dogSpriteRendererTop == null)
        {
            dogSpriteRendererTop = dogShakeVisualTop.GetComponent<SpriteRenderer>();
        }

        if (dogShakeVisualBottom != null && dogSpriteRendererBottom == null)
        {
            dogSpriteRendererBottom = dogShakeVisualBottom.GetComponent<SpriteRenderer>();
        }

        if (armTugVisual != null)
        {
            if (armTugSpriteRenderer == null)
            {
                armTugSpriteRenderer = armTugVisual.GetComponent<SpriteRenderer>();
            }

            if (armTugShake == null)
            {
                armTugShake = armTugVisual.GetComponent<TugSpriteShake>();
            }
        }

        AddFadeRenderer(dogSpriteRendererTop);
        AddFadeRenderer(dogSpriteRendererBottom);
        AddFadeRenderer(armTugSpriteRenderer);
    }

    private void LateUpdate()
    {
        if (tugGaugeController == null) return;

        float gauge = tugGaugeController.NormalizedGauge;
        UpdateVisualStep(gauge);
        UpdateShake(gauge);
    }

    private void UpdateVisualStep(float gauge)
    {
        int dogMoveIndex = GetStepIndex(gauge, dogMovePoints);
        int armTugIndex = GetStepIndex(gauge, armTugSprites);

        if (_currentDogMoveIndex < 0 && _currentArmTugIndex < 0)
        {
            ApplyVisualStep(dogMoveIndex, armTugIndex);
            SetVisualAlpha(1f);
            return;
        }

        if (dogMoveIndex != _targetDogMoveIndex || armTugIndex != _targetArmTugIndex)
        {
            StartFade(dogMoveIndex, armTugIndex);
            return;
        }

        if (_fadeCoroutine == null)
        {
            UpdateDogSprite();
        }
    }

    private void StartFade(int dogMoveIndex, int armTugIndex)
    {
        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
        }

        _targetDogMoveIndex = dogMoveIndex;
        _targetArmTugIndex = armTugIndex;
        _fadeCoroutine = StartCoroutine(CFadeVisualStep(dogMoveIndex, armTugIndex));
    }

    private IEnumerator CFadeVisualStep(int dogMoveIndex, int armTugIndex)
    {
        yield return CFadeAlpha(GetCurrentAlpha(), 0f);

        ApplyVisualStep(dogMoveIndex, armTugIndex);

        yield return CFadeAlpha(GetCurrentAlpha(), 1f);

        _fadeCoroutine = null;
    }

    private IEnumerator CFadeAlpha(float startAlpha, float targetAlpha)
    {
        if (fadeDuration <= 0f)
        {
            SetVisualAlpha(targetAlpha);
            yield break;
        }

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            SetVisualAlpha(alpha);
            yield return null;
        }

        SetVisualAlpha(targetAlpha);
    }

    private void ApplyVisualStep(int dogMoveIndex, int armTugIndex)
    {
        MoveDog(dogMoveIndex);
        UpdateDogSprite();
        UpdateArmTugSprite(armTugIndex);

        _currentDogMoveIndex = dogMoveIndex;
        _currentArmTugIndex = armTugIndex;
        _targetDogMoveIndex = dogMoveIndex;
        _targetArmTugIndex = armTugIndex;
    }

    private void MoveDog(int index)
    {
        if (dogRoot == null) return;
        if (dogMovePoints == null) return;
        if (index < 0) return;
        if (index >= dogMovePoints.Count) return;

        Transform targetPoint = dogMovePoints[index];

        if (targetPoint == null) return;

        dogRoot.position = targetPoint.position;
    }

    private void UpdateDogSprite()
    {
        if (dogSpriteRendererTop == null || dogSpriteRendererBottom == null) return;

        if (tugGaugeController.IsPlayerPulling)
        {
            if (dogPlayerPullSpriteTop != null) dogSpriteRendererTop.sprite = dogPlayerPullSpriteTop;

            if (dogPlayerPullSpriteBottom != null) dogSpriteRendererBottom.sprite = dogPlayerPullSpriteBottom;
        }
        else
        {
            if (dogIdleSpriteTop != null) dogSpriteRendererTop.sprite = dogIdleSpriteTop;

            if (dogIdleSpriteBottom != null) dogSpriteRendererBottom.sprite = dogIdleSpriteBottom;
        }
    }

    private void UpdateArmTugSprite(int index)
    {
        if (armTugSpriteRenderer == null) return;
        if (armTugSprites == null) return;
        if (armTugSprites.Length == 0) return;
        if (index < 0) return;
        if (index >= armTugSprites.Length) return;

        armTugSpriteRenderer.sprite = armTugSprites[index];
    }

    private void UpdateShake(float gauge)
    {
        float centerTension = 1f - Mathf.Abs(gauge - 0.5f) * 2f;
        float shakeStrength = centerTension * maxShakeStrength;

        if (tugGaugeController.IsDogPulling)
        {
            shakeStrength += 0.2f;
        }

        shakeStrength = Mathf.Clamp01(shakeStrength);

        if (dogShakeTop != null)
        {
            dogShakeTop.SetShakeStrength(shakeStrength);
        }

        if (dogShakeBottom != null)
        {
            dogShakeBottom.SetShakeStrength(shakeStrength);
        }

        if (armTugShake != null)
        {
            armTugShake.SetShakeStrength(shakeStrength * 0.6f);
        }
    }

    private int GetStepIndex(float gauge, List<Transform> points)
    {
        if (points == null) return -1;
        if (points.Count == 0) return -1;

        return GetStepIndex(gauge, points.Count);
    }

    private int GetStepIndex(float gauge, Sprite[] sprites)
    {
        if (sprites == null) return -1;
        if (sprites.Length == 0) return -1;

        return GetStepIndex(gauge, sprites.Length);
    }

    private int GetStepIndex(float gauge, int stepCount)
    {
        float clampedGauge = Mathf.Clamp01(gauge);
        int index = Mathf.FloorToInt(clampedGauge * stepCount);
        return Mathf.Clamp(index, 0, stepCount - 1);
    }

    private void AddFadeRenderer(SpriteRenderer spriteRenderer)
    {
        if (spriteRenderer == null) return;
        if (_fadeRenderers.Contains(spriteRenderer)) return;

        _fadeRenderers.Add(spriteRenderer);
    }

    private void SetVisualAlpha(float alpha)
    {
        for (int i = 0; i < _fadeRenderers.Count; i++)
        {
            SpriteRenderer spriteRenderer = _fadeRenderers[i];

            if (spriteRenderer == null) continue;

            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }

    private float GetCurrentAlpha()
    {
        for (int i = 0; i < _fadeRenderers.Count; i++)
        {
            if (_fadeRenderers[i] != null)
            {
                return _fadeRenderers[i].color.a;
            }
        }

        return 1f;
    }
}
