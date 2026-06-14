using UnityEngine;

public class TitleSystemManager : MonoBehaviour
{
    private void Awake()
    {
        if (FadeManager.Instance == null) return;
        
        FadeManager.Instance.FadeIn();
    }
}
