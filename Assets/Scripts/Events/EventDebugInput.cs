using UnityEngine;

public class EventDebugInput : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            FadeManager.instance.FadeIn();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            FadeManager.instance.FadeOut();
        }
    }
}
