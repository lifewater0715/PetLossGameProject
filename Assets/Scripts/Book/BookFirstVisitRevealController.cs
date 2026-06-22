using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class BookFirstVisitRevealController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Book book;

    [Header("Cover")]
    [SerializeField] private Material centerHoleFadeMaterial;
    [SerializeField] private Sprite leftCoverSprite;
    [SerializeField] private Sprite rightCoverSprite;
    [SerializeField] private Color coverColor = Color.white;

    [Header("Reveal Timing")]
    [SerializeField] private float revealDuration = 1f;
    [SerializeField] private float revealEndRadius = 1.2f;
    [SerializeField] private float revealDelayAfterFlip = 0.05f;
    [SerializeField] private float revealDelayBetweenPages = 0.35f;
    [SerializeField] private bool playInitialRevealOnStart = true;
    [SerializeField] private bool revealLeftPageFirst = true;
    [SerializeField] private bool blockBookInputDuringReveal = true;

    public bool IsRevealing { get; private set; }

    private readonly Dictionary<BookPageSlot, CenterHoleFadeUI> _covers = new Dictionary<BookPageSlot, CenterHoleFadeUI>();
    private readonly HashSet<int> _revealedPages = new HashSet<int>();
    private readonly HashSet<int> _revealingPages = new HashSet<int>();

    private Coroutine _revealCoroutine;
    private bool _initialized;

    private void Awake()
    {
        if (book == null)
            book = GetComponent<Book>();

        SetupCovers();
    }

    private void OnEnable()
    {
        if (book == null)
            book = GetComponent<Book>();

        SetupCovers();

        if (book != null)
        {
            book.OnPageSlotsUpdated += RefreshAllCovers;
            book.OnFlip.AddListener(PlayVisiblePageRevealSequence);
        }
    }

    private void Start()
    {
        StartCoroutine(CStartReveal());
    }

    private void OnDisable()
    {
        if (book != null)
        {
            book.OnPageSlotsUpdated -= RefreshAllCovers;
            book.OnFlip.RemoveListener(PlayVisiblePageRevealSequence);
        }

        if (_revealCoroutine != null)
        {
            StopCoroutine(_revealCoroutine);
            _revealCoroutine = null;
        }
    }

    public void PlayVisiblePageRevealSequence()
    {
        if (_revealCoroutine != null)
            StopCoroutine(_revealCoroutine);

        _revealCoroutine = StartCoroutine(CRevealVisiblePages());
    }

    public void MarkPageAsRevealed(int pageIndex)
    {
        if (pageIndex < 0) return;

        _revealedPages.Add(pageIndex);
        RefreshAllCovers();
    }

    public void ClearVisitedHistory()
    {
        _revealedPages.Clear();
        _revealingPages.Clear();
        RefreshAllCovers();
    }

    private IEnumerator CStartReveal()
    {
        yield return null;

        RefreshAllCovers();

        if (playInitialRevealOnStart)
            PlayVisiblePageRevealSequence();
    }

    private IEnumerator CRevealVisiblePages()
    {
        if (book == null)
            yield break;
    
        IsRevealing = true;
    
        bool previousInteractable = book.interactable;
    
        if (blockBookInputDuringReveal)
            book.interactable = false;
    
        yield return new WaitForSeconds(revealDelayAfterFlip);
    
        List<BookPageSlot> visibleSlots = GetVisibleSlotsInRevealOrder();
    
        for (int i = 0; i < visibleSlots.Count; i++)
        {
            BookPageSlot slot = visibleSlots[i];
            int pageIndex = book.GetPageIndex(slot);
    
            if (!IsValidPageIndex(pageIndex)) continue;
            if (_revealedPages.Contains(pageIndex)) continue;
            if (_revealingPages.Contains(pageIndex)) continue;
    
            CenterHoleFadeUI cover = GetCover(slot);
            if (cover == null) continue;
    
            _revealingPages.Add(pageIndex);
    
            cover.SetDuration(revealDuration);
            cover.SetEndRadius(revealEndRadius);
            cover.ResetToClosed(true);

            book.RestartPageAnimation(slot);
    
            yield return cover.PlayAndWait();
    
            _revealingPages.Remove(pageIndex);
            _revealedPages.Add(pageIndex);
    
            if (revealDelayBetweenPages > 0f)
                yield return new WaitForSeconds(revealDelayBetweenPages);
        }
    
        RefreshAllCovers();

        BookPageSlot lastSlot = revealLeftPageFirst ? BookPageSlot.RightNext : BookPageSlot.LeftNext;
        int lastPageIndex = book.GetPageIndex(lastSlot);
        
        while (!book.IsPageIntroFinished(lastPageIndex))
        {
            yield return null;
        }
    
        if (blockBookInputDuringReveal)
            book.interactable = previousInteractable;
    
        IsRevealing = false;
        _revealCoroutine = null;
    }

    private List<BookPageSlot> GetVisibleSlotsInRevealOrder()
    {
        List<BookPageSlot> slots = new List<BookPageSlot>();

        if (revealLeftPageFirst)
        {
            slots.Add(BookPageSlot.LeftNext);
            slots.Add(BookPageSlot.RightNext);
        }
        else
        {
            slots.Add(BookPageSlot.RightNext);
            slots.Add(BookPageSlot.LeftNext);
        }

        return slots;
    }

    private void RefreshAllCovers()
    {
        if (book == null)
            return;

        SetupCovers();

        RefreshCover(BookPageSlot.Left);
        RefreshCover(BookPageSlot.Right);
        RefreshCover(BookPageSlot.LeftNext);
        RefreshCover(BookPageSlot.RightNext);
    }

    private void RefreshCover(BookPageSlot slot)
    {
        CenterHoleFadeUI cover = GetCover(slot);
        
        if (cover == null) return;

        int pageIndex = book.GetPageIndex(slot);

        if (!IsValidPageIndex(pageIndex))
        {
            cover.OpenImmediate();
            return;
        }

        if (_revealedPages.Contains(pageIndex))
        {
            cover.OpenImmediate();
            return;
        }

        if (_revealingPages.Contains(pageIndex))
            return;

        book.PausePageAnimationAtFirstFrame(slot);

        cover.SetDuration(revealDuration);
        cover.SetEndRadius(revealEndRadius);
        cover.ResetToClosed(true);
    }

    private bool IsValidPageIndex(int pageIndex)
    {
        return book != null && pageIndex >= 0 && pageIndex < book.TotalPageCount;
    }

    private CenterHoleFadeUI GetCover(BookPageSlot slot)
    {
        CenterHoleFadeUI cover;

        if (_covers.TryGetValue(slot, out cover))
            return cover;

        return null;
    }

    private void SetupCovers()
    {
        if (_initialized) return;
        if (book == null) return;

        CreateCover(BookPageSlot.Left);
        CreateCover(BookPageSlot.Right);
        CreateCover(BookPageSlot.LeftNext);
        CreateCover(BookPageSlot.RightNext);

        _initialized = true;
    }

    private void CreateCover(BookPageSlot slot)
    {
        Image pageImage = book.GetPageImage(slot);
        if (pageImage == null) return;

        string coverName = "__RevealCover_" + slot;
        Transform existing = pageImage.transform.Find(coverName);

        CenterHoleFadeUI fade;
        Image coverImage;

        if (existing != null)
        {
            fade = existing.GetComponent<CenterHoleFadeUI>();
            coverImage = existing.GetComponent<Image>();

            if (coverImage == null)
                coverImage = existing.gameObject.AddComponent<Image>();

            if (fade == null)
                fade = existing.gameObject.AddComponent<CenterHoleFadeUI>();
        }
        else
        {
            GameObject coverObject = new GameObject(coverName, typeof(RectTransform), typeof(Image), typeof(CenterHoleFadeUI));
            RectTransform coverRect = coverObject.GetComponent<RectTransform>();

            coverRect.SetParent(pageImage.rectTransform, false);
            coverRect.anchorMin = Vector2.zero;
            coverRect.anchorMax = Vector2.one;
            coverRect.offsetMin = Vector2.zero;
            coverRect.offsetMax = Vector2.zero;
            coverRect.pivot = new Vector2(0.5f, 0.5f);
            coverRect.localScale = Vector3.one;

            coverImage = coverObject.GetComponent<Image>();
            fade = coverObject.GetComponent<CenterHoleFadeUI>();
        }

        coverImage.sprite = GetCoverSprite(slot);
        coverImage.color = coverColor;
        coverImage.raycastTarget = false;

        if (centerHoleFadeMaterial != null)
            coverImage.material = centerHoleFadeMaterial;
        else
        {
            Shader shader = Shader.Find("UI/Center Hole Fade");
            if (shader != null)
                coverImage.material = new Material(shader);
        }

        fade.Initialize(coverImage, revealDuration, revealEndRadius, coverImage.material);
        fade.ResetToClosed(false);

        fade.transform.SetAsLastSibling();

        _covers[slot] = fade;
    }

    private Sprite GetCoverSprite(BookPageSlot slot)
{
    switch (slot)
    {
        case BookPageSlot.Left:
        case BookPageSlot.LeftNext:
            return leftCoverSprite;

        case BookPageSlot.Right:
        case BookPageSlot.RightNext:
            return rightCoverSprite;

        default:
            return rightCoverSprite;
    }
}
}
