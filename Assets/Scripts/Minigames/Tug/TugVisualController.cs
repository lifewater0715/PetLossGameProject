using System.Collections.Generic;
using UnityEngine;

public class TugVisualController : MonoBehaviour
{
    [SerializeField] private TugGaugeController tugGaugeController;
    [SerializeField] private Transform dogRoot;
    [SerializeField] private Transform dogShakeVisual;
    [SerializeField] private List<Transform> dogMovePoints = new List<Transform>();
    [SerializeField] private SpriteRenderer dogSpriteRenderer;
    [SerializeField] private Sprite dogIdleSprite;
    [SerializeField] private Sprite dogPlayerPullSprite;
    [SerializeField] private Transform armTugVisual;
    [SerializeField] private SpriteRenderer armTugSpriteRenderer;
    [SerializeField] private Sprite[] armTugSprites;
    [SerializeField] private TugSpriteShake dogShake;
    [SerializeField] private TugSpriteShake armTugShake;
    [SerializeField] private float maxShakeStrength = 1f;

    private void Awake()
    {
        if (dogShakeVisual != null && dogShake == null)
        {
            dogShake = dogShakeVisual.GetComponent<TugSpriteShake>();
        }

        if (dogShakeVisual != null && dogSpriteRenderer == null)
        {
            dogSpriteRenderer = dogShakeVisual.GetComponent<SpriteRenderer>();
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
        if (dogSpriteRenderer == null) return;

        if (tugGaugeController.IsPlayerPulling)
        {
            if (dogPlayerPullSprite != null)
            {
                dogSpriteRenderer.sprite = dogPlayerPullSprite;
            }
        }
        else
        {
            if (dogIdleSprite != null)
            {
                dogSpriteRenderer.sprite = dogIdleSprite;
            }
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

        if (dogShake != null)
        {
            dogShake.SetShakeStrength(shakeStrength);
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
