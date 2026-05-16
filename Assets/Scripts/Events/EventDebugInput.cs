using UnityEngine;

public class EventDebugInput : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            FadeManager.Instance.FadeIn();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            FadeManager.Instance.FadeOut();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            FadeManager.Instance.HalfFadeIn();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            FadeManager.Instance.HalfFadeOut();
        }
    }
}
