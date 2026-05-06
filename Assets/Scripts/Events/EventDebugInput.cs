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
    }
}
