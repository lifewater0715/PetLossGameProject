using UnityEngine;

public class IntroInput : MonoBehaviour
{
    [SerializeField] private AutoFlip autoFlip;
    [SerializeField] private BookFirstVisitRevealController revealController;

    private void Update()
    {
        if (FadeManager.Instance != null && FadeManager.Instance.IsFading) return;
        if (autoFlip == null) return;
        if (autoFlip.IsFlipping) return;
        if (revealController != null && revealController.IsRevealing) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            autoFlip.FlipRightPage();
        }
    }
}