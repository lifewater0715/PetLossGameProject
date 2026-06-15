using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BookPageColorShiftFade : MonoBehaviour
{
    [SerializeField] private Book book;
    [SerializeField] private BookPageSlot targetSlot = BookPageSlot.LeftNext;
    [SerializeField] private int targetPageIndex = 3;

    [Header("Cold Animation")]
    [SerializeField] private Sprite[] coldFrames;
    [SerializeField] private float frameRate = 8f;

    [Header("Timing")]
    [SerializeField] private float delayBeforeFade = 1f;
    [SerializeField] private float fadeDuration = 1.5f;

    private bool played;
    private GameObject overlayObject;
    private Image overlayImage;
    private CanvasGroup overlayCanvasGroup;
    private Coroutine playCoroutine;
    private Coroutine frameCoroutine;
    private bool coldAnimationFinished;

    public void PlayOnce()
    {
        if (played) return;
        played = true;

        if (playCoroutine != null)
            StopCoroutine(playCoroutine);

        playCoroutine = StartCoroutine(CPlay());
    }

    public void RefreshOverlay()
    {
        if (book == null) return;
        if (playCoroutine != null) return;

        int currentIndex = book.GetPageIndex(targetSlot);

        if (currentIndex != targetPageIndex)
            ClearOverlayOnly();
    }

    private IEnumerator CPlay()
    {
        yield return new WaitForSeconds(delayBeforeFade);
    
        if (book == null)
        {
            playCoroutine = null;
            yield break;
        }
    
        if (book.GetPageIndex(targetSlot) != targetPageIndex)
        {
            playCoroutine = null;
            yield break;
        }
    
        Image baseImage = book.GetPageImage(targetSlot);
        if (baseImage == null)
        {
            playCoroutine = null;
            yield break;
        }
    
        if (coldFrames == null || coldFrames.Length == 0)
        {
            playCoroutine = null;
            yield break;
        }
    
        CreateOverlay(baseImage);
    
        float frameInterval = 1f / frameRate;
        float frameTimer = 0f;
        int frameIndex = 0;
    
        overlayImage.sprite = coldFrames[frameIndex];
    
        float fadeTime = 0f;
        float totalAnimationTime = coldFrames.Length * frameInterval;
    
        while (frameIndex < coldFrames.Length)
        {
            // 페이드
            fadeTime += Time.deltaTime;
            float fadeT = Mathf.Clamp01(fadeTime / fadeDuration);
    
            if (overlayCanvasGroup != null)
                overlayCanvasGroup.alpha = Mathf.SmoothStep(0f, 1f, fadeT);
    
            // 프레임
            frameTimer += Time.deltaTime;
    
            if (frameTimer >= frameInterval)
            {
                frameTimer -= frameInterval;
                frameIndex++;
    
                if (frameIndex < coldFrames.Length && overlayImage != null)
                    overlayImage.sprite = coldFrames[frameIndex];
            }
    
            yield return null;
        }
    
        if (overlayCanvasGroup != null)
            overlayCanvasGroup.alpha = 1f;
    
        if (overlayImage != null)
            overlayImage.sprite = coldFrames[coldFrames.Length - 1];
    
        if (baseImage != null)
            baseImage.sprite = coldFrames[coldFrames.Length - 1];
    
        ClearOverlayOnly();
    
        playCoroutine = null;
    }

    private void CreateOverlay(Image baseImage)
    {
        ClearOverlayOnly();

        overlayObject = new GameObject("__ColdOverlay", typeof(RectTransform), typeof(Image), typeof(CanvasGroup));

        RectTransform overlayRect = overlayObject.GetComponent<RectTransform>();
        overlayRect.SetParent(baseImage.rectTransform, false);
        overlayRect.anchorMin = Vector2.zero;
        overlayRect.anchorMax = Vector2.one;
        overlayRect.offsetMin = Vector2.zero;
        overlayRect.offsetMax = Vector2.zero;
        overlayRect.pivot = new Vector2(0.5f, 0.5f);
        overlayRect.localScale = Vector3.one;

        overlayImage = overlayObject.GetComponent<Image>();
        overlayImage.raycastTarget = false;

        overlayCanvasGroup = overlayObject.GetComponent<CanvasGroup>();
        overlayCanvasGroup.alpha = 0f;
    }

    private void StartFrameAnimation()
    {
        if (frameCoroutine != null)
            StopCoroutine(frameCoroutine);

        frameCoroutine = StartCoroutine(CPlayFrames());
    }

    private IEnumerator CPlayFrames()
    {
        coldAnimationFinished = false;

        if (coldFrames == null || coldFrames.Length == 0)
        {
            coldAnimationFinished = true;
            yield break;
        }

        WaitForSeconds delay = new WaitForSeconds(1f / frameRate);

        for (int i = 0; i < coldFrames.Length; i++)
        {
            if (overlayImage == null)
                yield break;

            overlayImage.sprite = coldFrames[i];
            yield return delay;
        }

        if (overlayImage != null)
            overlayImage.sprite = coldFrames[coldFrames.Length - 1];

        coldAnimationFinished = true;
        frameCoroutine = null;
    }

    private void ClearOverlayOnly()
    {
        if (frameCoroutine != null)
        {
            StopCoroutine(frameCoroutine);
            frameCoroutine = null;
        }

        overlayImage = null;
        overlayCanvasGroup = null;

        if (overlayObject != null)
        {
            Destroy(overlayObject);
            overlayObject = null;
        }
    }

    private void StopAll()
    {
        if (playCoroutine != null)
        {
            StopCoroutine(playCoroutine);
            playCoroutine = null;
        }

        ClearOverlayOnly();
    }

    private void OnDisable()
    {
        StopAll();
    }
}