using UnityEngine;

public class IntroInput : MonoBehaviour
{
    [SerializeField] private Book book;
    [SerializeField] private AutoFlip autoFlip;
    [SerializeField] private BookFirstVisitRevealController revealController;
    [SerializeField] private int lastPage = 4;

    private void Update()
    {
        if (FadeManager.Instance != null && FadeManager.Instance.IsFading) return;
        if (autoFlip == null) return;
        if (autoFlip.IsFlipping) return;
        if (revealController != null && revealController.IsRevealing) return;

        int currentPage = book.GetPageIndex(BookPageSlot.RightNext);

        if (!book.IsPageIntroFinished(currentPage))
            return;

        if (Input.GetKeyDown(KeyCode.Space) && currentPage != lastPage)
        {
            Debug.Log("Space 허용");
        
            autoFlip.FlipRightPage();
        }
    }
}