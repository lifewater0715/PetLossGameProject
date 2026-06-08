using UnityEngine;

public class IntroInput : MonoBehaviour
{
    [SerializeField] private IntroSystemManager introSystemManager;

    private void Update()
    {
        if (FadeManager.Instance != null && FadeManager.Instance.IsFading) return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            introSystemManager.MoveNextScene();
        }
    }
}
