using UnityEngine;

public class BookPageColorShiftTrigger : MonoBehaviour
{
    [SerializeField] private Book book;
    [SerializeField] private BookPageColorShiftFade colorShiftFade;

    [Header("Trigger")]
    [SerializeField] private int targetPageIndex = 3;
    [SerializeField] private BookPageSlot targetSlot = BookPageSlot.RightNext;

    private bool played;

    private void Awake()
    {
        if (book == null)
            book = GetComponent<Book>();

        if (colorShiftFade == null)
            colorShiftFade = GetComponent<BookPageColorShiftFade>();
    }

    private void OnEnable()
    {
        if (book != null)
        {
            book.OnFlip.AddListener(CheckAndPlay);
            book.OnFlip.AddListener(colorShiftFade.RefreshOverlay);
        }
    }

    private void OnDisable()
    {
        if (book != null)
        {
            book.OnFlip.RemoveListener(CheckAndPlay);
            book.OnFlip.RemoveListener(colorShiftFade.RefreshOverlay);
        }
    }
    private void Start()
    {
        CheckAndPlay();
    }

    private void CheckAndPlay()
    {
        if (played) return;
        if (book == null || colorShiftFade == null) return;
    
        int leftIndex = book.GetPageIndex(BookPageSlot.LeftNext);
        int rightIndex = book.GetPageIndex(BookPageSlot.RightNext);
    
        Debug.Log($"LeftNext: {leftIndex}, RightNext: {rightIndex}");
    
        int currentIndex = book.GetPageIndex(targetSlot);
    
        if (currentIndex != targetPageIndex) return;
    
        played = true;
        colorShiftFade.PlayOnce();
    }
}