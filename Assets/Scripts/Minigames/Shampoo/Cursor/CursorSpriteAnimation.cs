using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CursorSpriteAnimation : MonoBehaviour
{
    private Image image;
    [SerializeField] private Sprite[] frames;
    [SerializeField] private float delay = 1f;

    private Coroutine animationCoroutine;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    
    public void Play()
    {
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        animationCoroutine = StartCoroutine(PlayAnimation());
    }

    public void Stop()
    {
        if (animationCoroutine == null)
            return;

        StopCoroutine(animationCoroutine);
        animationCoroutine = null;
    }

    private IEnumerator PlayAnimation()
    {
        int index = 0;

        while (true)
        {
            image.sprite = frames[index];
            index++;

            if (index >= frames.Length)
                index = 0;

            yield return new WaitForSeconds(delay);
        }
    }
}
