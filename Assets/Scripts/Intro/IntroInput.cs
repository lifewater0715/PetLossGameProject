using UnityEngine;

public class IntroInput : MonoBehaviour
{
    [SerializeField] private IntroSystemManager introSystemManager;
    [SerializeField] private AutoFlip autoFlip;

    private void Update()
    {
        if (FadeManager.Instance != null && FadeManager.Instance.IsFading) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            autoFlip.FlipRightPage();
        }
    }
}
