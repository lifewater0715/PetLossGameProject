using UnityEngine;

public class ResolutionLock : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetFHDScreen();
    }

    private void FixedUpdate()
    {
        if (Screen.width != 1920 || Screen.height != 1080)
        {
            SetFHDScreen();
        }
    }

    private void SetFHDScreen()
    {
        Screen.SetResolution(1920, 1080, true);
    }
}
