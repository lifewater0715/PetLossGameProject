using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 이 컴포넌트를 붙인 TMP 텍스트에만 고대비 배경을 표시합니다.
/// 배경은 런타임에 텍스트 바로 뒤에 자동으로 생성됩니다.
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(TextMeshProUGUI))]
public sealed class HighContrastText : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Color backgroundColor = new Color(0f, 0f, 0f, 0.85f);
    [SerializeField] private Vector2 padding = new Vector2(30f, 15f);

    private TMP_Text targetText;
    private GlobalFontManager fontManager;
    private Color originalTextColor;
    private bool ownsBackground;
    private bool highContrastEnabled;
    private bool isRefreshing;
    private bool refreshRequested;

    public bool IsHighContrastEnabled => highContrastEnabled;

    private void Awake()
    {
        targetText = GetComponent<TMP_Text>();
        originalTextColor = targetText.color;
    }

    private void OnEnable()
    {
        if (!Application.isPlaying)
            return;

        fontManager = GlobalFontManager.EnsureExists();
        fontManager.HighContrastModeChanged += SetHighContrast;
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);

        EnsureBackground();
        SetHighContrast(fontManager.IsHighContrastEnabled);
    }

    private void OnDisable()
    {
        if (!Application.isPlaying)
            return;

        if (fontManager != null)
            fontManager.HighContrastModeChanged -= SetHighContrast;

        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);

        if (background != null)
            background.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (ownsBackground && background != null)
            Destroy(background.gameObject);
    }

    private void OnRectTransformDimensionsChange()
    {
        if (Application.isPlaying && highContrastEnabled && !isRefreshing)
            RequestRefresh();
    }

    private void OnTextChanged(Object changedObject)
    {
        if (changedObject == targetText && highContrastEnabled && !isRefreshing)
            RequestRefresh();
    }

    private void LateUpdate()
    {
        if (!refreshRequested)
            return;

        // Canvas 재빌드 중에는 Graphic의 크기나 활성 상태를 변경할 수 없습니다.
        if (CanvasUpdateRegistry.IsRebuildingLayout() ||
            CanvasUpdateRegistry.IsRebuildingGraphics())
            return;

        refreshRequested = false;
        RefreshBackground();
    }

    private void SetHighContrast(bool enabled)
    {
        highContrastEnabled = enabled;
        ApplyTextColor(enabled);
        EnsureBackground();
        RequestRefresh();
    }

    private void ApplyTextColor(bool useHighContrastColor)
    {
        if (targetText == null)
            return;

        // 페이드 진행 중의 알파값은 유지하고 RGB 값만 변경합니다.
        float currentAlpha = targetText.color.a;
        Color nextColor = useHighContrastColor ? Color.white : originalTextColor;
        nextColor.a = currentAlpha;
        targetText.color = nextColor;
    }

    private void EnsureBackground()
    {
        if (background != null || targetText == null)
            return;

        GameObject backgroundObject = new GameObject(
            $"{gameObject.name}_HighContrastBackground",
            typeof(RectTransform));
        backgroundObject.SetActive(false);
        backgroundObject.AddComponent<CanvasRenderer>();
        background = backgroundObject.AddComponent<Image>();

        RectTransform textRect = targetText.rectTransform;
        RectTransform backgroundRect = backgroundObject.GetComponent<RectTransform>();
        backgroundRect.SetParent(textRect.parent, false);
        CopyRectTransform(textRect, backgroundRect);
        backgroundRect.SetSiblingIndex(textRect.GetSiblingIndex());

        background.raycastTarget = false;
        ownsBackground = true;
    }

    private void RequestRefresh()
    {
        refreshRequested = true;
    }

    private void RefreshBackground()
    {
        if (isRefreshing || background == null || targetText == null)
            return;

        isRefreshing = true;

        try
        {
            bool shouldShow = highContrastEnabled && !string.IsNullOrWhiteSpace(targetText.text);
            background.gameObject.SetActive(shouldShow);

            if (!shouldShow)
                return;

            background.color = backgroundColor;
            targetText.ForceMeshUpdate();

            Bounds textBounds = targetText.textBounds;
            RectTransform textRect = targetText.rectTransform;
            RectTransform backgroundRect = background.rectTransform;

            CopyRectTransform(textRect, backgroundRect);

            Vector2 localOffset = (Vector2)textBounds.center - textRect.rect.center;
            backgroundRect.anchoredPosition = textRect.anchoredPosition + localOffset;
            backgroundRect.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal,
                textBounds.size.x + padding.x * 2f);
            backgroundRect.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Vertical,
                textBounds.size.y + padding.y * 2f);
        }
        finally
        {
            isRefreshing = false;
        }
    }

    private static void CopyRectTransform(RectTransform source, RectTransform destination)
    {
        destination.anchorMin = source.anchorMin;
        destination.anchorMax = source.anchorMax;
        destination.pivot = source.pivot;
        destination.anchoredPosition3D = source.anchoredPosition3D;
        destination.sizeDelta = source.sizeDelta;
        destination.localRotation = source.localRotation;
        destination.localScale = source.localScale;
    }
}
