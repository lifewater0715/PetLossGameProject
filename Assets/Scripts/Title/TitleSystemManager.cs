using TMPro;
using UnityEngine;

public class TitleSystemManager : MonoBehaviour
{
    [SerializeField] private GameObject confirmBtn;
    [SerializeField] private TMP_FontAsset globalFont;
    
    private void Awake()
    {
        if (FadeManager.Instance == null) return;
        
        FadeManager.Instance.FadeIn();
    }

    private void Start()
    {
        confirmBtn.SetActive(false);
    }

    /// <summary>
    /// Inspector에 지정한 폰트를 현재 및 이후의 모든 TMP 텍스트에 적용합니다.
    /// 버튼의 OnClick에서도 직접 호출할 수 있습니다.
    /// </summary>
    public void ApplyGlobalFont()
    {
        GlobalFontManager.EnsureExists().SetGlobalFont(globalFont);
    }

    /// <summary>
    /// 코드에서 원하는 폰트를 전달해 전역 적용할 때 사용합니다.
    /// </summary>
    public void ApplyGlobalFont(TMP_FontAsset font)
    {
        GlobalFontManager.EnsureExists().SetGlobalFont(font);
    }

    public void EnableHighContrastText()
    {
        GlobalFontManager.EnsureExists().SetHighContrastMode(true);
    }

    public void DisableHighContrastText()
    {
        GlobalFontManager.EnsureExists().SetHighContrastMode(false);
    }

    public void ToggleHighContrastText()
    {
        GlobalFontManager.EnsureExists().ToggleHighContrastMode();
    }
}
