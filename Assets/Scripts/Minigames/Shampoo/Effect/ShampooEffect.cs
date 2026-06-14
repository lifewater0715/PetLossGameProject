using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShampooEffect : MonoBehaviour
{
    [Header("거품")]
    [SerializeField] private SpriteRenderer bubbleEffect;

    [Header("샤워")]
    [SerializeField] private SpriteRenderer floorWaterEffect;
    [SerializeField] private float animDelay = 1f;
    [SerializeField] private List<Sprite> floorWaterSprite = new List<Sprite>();

    [SerializeField] private float[] floorWaterThresholds = { 15f, 30f, 45f, 60f, 75f };

    [SerializeField] private int _floorWaterIndex = 0;

    private Coroutine _currentAnimation;

    private void Awake()
    {
        bubbleEffect.color = new Color(0f, 0f, 0f, 0f);
        floorWaterEffect.gameObject.SetActive(false);
    }

    public void OnBubble()
    {
        if (bubbleEffect.color.a >= 1f) return;

        bubbleEffect.color = new Color(1f, 1f, 1f, bubbleEffect.color.a + 0.01f);
    }

    public void OnShower()
    {
        bubbleEffect.color = new Color(1f, 1f, 1f, bubbleEffect.color.a - 0.01f);
    }

    public void ChangeWaterState(float charged)
    {
        if (!floorWaterEffect.gameObject.activeSelf) floorWaterEffect.gameObject.SetActive(true);

        if (_floorWaterIndex >= floorWaterSprite.Count) return;
        if (charged < floorWaterThresholds[_floorWaterIndex]) return;

        //floorWaterEffect.sprite = floorWaterSprite[_floorWaterIndex];

        if (_floorWaterIndex < floorWaterSprite.Count - 1) _floorWaterIndex++;

        if (_floorWaterIndex < 4) 
        {
            floorWaterEffect.sprite = floorWaterSprite[_floorWaterIndex];
            return;
        }

        StopFloorWaterAnimation();

        _currentAnimation = StartCoroutine(FloorWaterAnimation());
    }

    public void StopFloorWaterAnimation()
    {
        if (_currentAnimation == null) return;
        
        StopCoroutine(_currentAnimation);
        _currentAnimation = null;
    }

    private IEnumerator FloorWaterAnimation()
    {
        int prevIndex = _floorWaterIndex - 1;

        while (true)
        {
            floorWaterEffect.sprite = floorWaterSprite[prevIndex];
            yield return new WaitForSeconds(animDelay);

            floorWaterEffect.sprite = floorWaterSprite[_floorWaterIndex];
            yield return new WaitForSeconds(animDelay);
        }
    }
}
