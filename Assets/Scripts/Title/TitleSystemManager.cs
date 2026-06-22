using UnityEngine;

public class TitleSystemManager : MonoBehaviour
{
    [SerializeField] private GameObject confirmBtn;
    
    private void Awake()
    {
        if (FadeManager.Instance == null) return;
        
        FadeManager.Instance.FadeIn();
    }

    private void Start()
    {
        confirmBtn.SetActive(false);
    }
}
