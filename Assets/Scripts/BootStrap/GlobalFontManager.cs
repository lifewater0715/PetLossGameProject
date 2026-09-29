using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 게임 전체에서 사용하는 TMP 폰트를 관리합니다.
/// BootStrap 씬에서 생성된 뒤 씬이 바뀌어도 유지됩니다.
/// </summary>
public sealed class GlobalFontManager : MonoBehaviour
{
    private static readonly Vector2 TextAreaPadding = new Vector2(80f, 30f);
    private const float MaxTextAreaWidth = 1600f;
    private readonly HashSet<TMP_Text> pendingTextUpdates = new HashSet<TMP_Text>();
    private readonly Dictionary<TMP_Text, TMP_FontAsset> originalFonts =
        new Dictionary<TMP_Text, TMP_FontAsset>();

    public static GlobalFontManager Instance { get; private set; }

    public TMP_FontAsset CurrentFont { get; private set; }
    public bool IsHighContrastEnabled { get; private set; }

    public event Action<bool> HighContrastModeChanged;

    public static GlobalFontManager EnsureExists()
    {
        if (Instance != null)
            return Instance;

        GlobalFontManager existingManager = FindFirstObjectByType<GlobalFontManager>();
        if (existingManager != null)
            return existingManager;

        GameObject managerObject = new GameObject(nameof(GlobalFontManager));
        return managerObject.AddComponent<GlobalFontManager>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
    }

    private void OnDestroy()
    {
        if (Instance != this)
            return;

        SceneManager.sceneLoaded -= OnSceneLoaded;
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
        Instance = null;
    }

    /// <summary>
    /// 현재 씬과 이후에 로드되거나 생성되는 모든 TMP 텍스트의 폰트를 변경합니다.
    /// </summary>
    public void SetGlobalFont(TMP_FontAsset font)
    {
        if (font == null)
        {
            Debug.LogWarning("변경할 TMP 폰트가 지정되지 않았습니다.");
            return;
        }

        CurrentFont = font;
        ApplyFontToLoadedTexts();
    }

    /// <summary>
    /// 전역 폰트 적용 전에 각 텍스트가 사용하던 원본 폰트로 되돌립니다.
    /// 이후 로드되는 씬에는 전역 폰트를 더 이상 강제로 적용하지 않습니다.
    /// </summary>
    public void RestoreOriginalFonts()
    {
        CurrentFont = null;
        pendingTextUpdates.Clear();

        foreach (KeyValuePair<TMP_Text, TMP_FontAsset> entry in originalFonts)
        {
            if (entry.Key != null && entry.Value != null)
                entry.Key.font = entry.Value;
        }

        originalFonts.Clear();
    }

    /// <summary>
    /// HighContrastText가 붙은 텍스트의 배경 박스를 한 번에 켜거나 끕니다.
    /// </summary>
    public void SetHighContrastMode(bool enabled)
    {
        if (IsHighContrastEnabled == enabled)
            return;

        IsHighContrastEnabled = enabled;
        HighContrastModeChanged?.Invoke(enabled);
    }

    public void ToggleHighContrastMode()
    {
        SetHighContrastMode(!IsHighContrastEnabled);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyFontToLoadedTexts();
    }

    private void OnTextChanged(UnityEngine.Object changedObject)
    {
        if (CurrentFont == null || changedObject is not TMP_Text text)
            return;

        // TMP 이벤트는 Canvas 그래픽 재빌드 도중에도 발생하므로 실제 변경은 지연합니다.
        pendingTextUpdates.Add(text);
    }

    private void LateUpdate()
    {
        if (pendingTextUpdates.Count == 0 ||
            CanvasUpdateRegistry.IsRebuildingLayout() ||
            CanvasUpdateRegistry.IsRebuildingGraphics())
            return;

        TMP_Text[] texts = new TMP_Text[pendingTextUpdates.Count];
        pendingTextUpdates.CopyTo(texts);
        pendingTextUpdates.Clear();

        foreach (TMP_Text text in texts)
        {
            if (text == null)
                continue;

            ApplyFont(text);
            ExpandTextAreaToFit(text);
        }
    }

    private void ApplyFontToLoadedTexts()
    {
        if (CurrentFont == null)
            return;

        TMP_Text[] texts = FindObjectsByType<TMP_Text>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None);

        foreach (TMP_Text text in texts)
        {
            ApplyFont(text);
            ExpandTextAreaToFit(text);
        }
    }

    private void ApplyFont(TMP_Text text)
    {
        if (text == null || CurrentFont == null)
            return;

        if (!originalFonts.ContainsKey(text))
            originalFonts.Add(text, text.font);

        if (text.font != CurrentFont)
            text.font = CurrentFont;
    }

    /// <summary>
    /// 폰트나 문장이 바뀌어도 글자가 RectTransform 밖으로 벗어나지 않도록
    /// 현재 크기보다 부족한 방향만 확장합니다.
    /// </summary>
    private void ExpandTextAreaToFit(TMP_Text text)
    {
        if (text == null || string.IsNullOrEmpty(text.text))
            return;

        RectTransform rectTransform = text.rectTransform;
        float currentWidth = rectTransform.rect.width;
        float currentHeight = rectTransform.rect.height;

        // 먼저 한 줄 기준 너비를 구하되 화면을 지나치게 넘지 않도록 제한합니다.
        Vector2 singleLineSize = text.GetPreferredValues(
            text.text,
            Mathf.Infinity,
            Mathf.Infinity);
        float desiredWidth = Mathf.Min(
            Mathf.Max(currentWidth, singleLineSize.x + TextAreaPadding.x),
            MaxTextAreaWidth);

        // 제한된 너비에서 줄바꿈까지 반영해 필요한 높이를 다시 계산합니다.
        float contentWidth = Mathf.Max(1f, desiredWidth - TextAreaPadding.x);
        Vector2 wrappedSize = text.GetPreferredValues(
            text.text,
            contentWidth,
            Mathf.Infinity);
        float desiredHeight = Mathf.Max(
            currentHeight,
            wrappedSize.y + TextAreaPadding.y);

        if (desiredWidth > currentWidth + 0.5f)
        {
            rectTransform.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal,
                desiredWidth);
        }

        if (desiredHeight > currentHeight + 0.5f)
        {
            rectTransform.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Vertical,
                desiredHeight);
        }
    }
}
