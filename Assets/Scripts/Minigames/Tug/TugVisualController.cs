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
    }

    private void LateUpdate()
    {
        if (tugGaugeController == null) return;

        float gauge = tugGaugeController.NormalizedGauge;
        MoveDog(gauge);
        UpdateDogSprite();
        UpdateArmTugSprite(gauge);
        UpdateShake(gauge);
    }

    private void MoveDog(float gauge)
    {
        if (dogRoot == null) return;
        if (dogMovePoints == null) return;
        if (dogMovePoints.Count == 0) return;

        int index = GetStepIndex(gauge, dogMovePoints.Count);
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

    private void UpdateArmTugSprite(float gauge)
    {
        if (armTugSpriteRenderer == null) return;
        if (armTugSprites == null) return;
        if (armTugSprites.Length == 0) return;

        int index = GetStepIndex(gauge, armTugSprites.Length);
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

    private int GetStepIndex(float gauge, int stepCount)
    {
        float clampedGauge = Mathf.Clamp01(gauge);
        int index = Mathf.FloorToInt(clampedGauge * stepCount);
        return Mathf.Clamp(index, 0, stepCount - 1);
    }
}
