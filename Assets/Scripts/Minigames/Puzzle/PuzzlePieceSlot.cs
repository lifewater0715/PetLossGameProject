using UnityEngine;

public class PuzzlePieceSlot : MonoBehaviour
{
    [System.Serializable]
    private class MaskIsolationSettings
    {
        public bool useCustomRange = true;
        public string sortingLayerName = "PuzzleMask";
        public int sortingOrder = 0;
        public int sortingRangePadding = 1;
    }

    [System.Serializable]
    public class SavedPieceState
    {
        public string pieceName;
        public Sprite sprite;
        public Vector3 imagePosition;
        public Vector3 imageScale;
        public bool isSaved;
    }

    [SerializeField] private string pieceName = "PuzzlePiece";
    [SerializeField] private SpriteMask pieceMask;
    [SerializeField] private SpriteRenderer maskedImageRenderer;
    [SerializeField] private bool hideUntilConfigured = true;
    [SerializeField] private MaskIsolationSettings maskIsolation = new MaskIsolationSettings();

    private readonly SavedPieceState savedState = new SavedPieceState();

    public string PieceName => pieceName;
    public SavedPieceState State => savedState;

    void Awake()
    {
        if (hideUntilConfigured == true)
        {
            SetVisible(false);
        }

        ApplyMaskSpriteToRenderer();
    }

    public void ConfigurePiece(Sprite sourceSprite, Vector3 imagePosition, Vector3 imageLossyScale)
    {
        if (maskedImageRenderer == null)
        {
            Debug.LogWarning($"{name} is missing a masked image renderer.");
            return;
        }

        ApplyMaskSpriteToRenderer();
        maskedImageRenderer.sprite = sourceSprite;
        maskedImageRenderer.color = Color.white;
        ApplyWorldTransform(imagePosition, imageLossyScale);
        SetVisible(sourceSprite != null);
    }

    public void SaveCurrentState()
    {
        if (maskedImageRenderer == null)
        {
            return;
        }

        savedState.pieceName = pieceName;
        savedState.sprite = maskedImageRenderer.sprite;
        savedState.imagePosition = maskedImageRenderer.transform.position;
        savedState.imageScale = maskedImageRenderer.transform.lossyScale;
        savedState.isSaved = savedState.sprite != null;
    }

    public void ApplySavedState()
    {
        if (maskedImageRenderer == null || savedState.isSaved == false)
        {
            return;
        }

        ApplyMaskSpriteToRenderer();
        maskedImageRenderer.sprite = savedState.sprite;
        maskedImageRenderer.color = Color.white;
        ApplyWorldTransform(savedState.imagePosition, savedState.imageScale);
        SetVisible(true);
    }

    public void ClearPiece()
    {
        if (maskedImageRenderer != null)
        {
            maskedImageRenderer.sprite = null;
            maskedImageRenderer.color = Color.white;
        }

        savedState.sprite = null;
        savedState.isSaved = false;
        SetVisible(false);
    }

    public void SetVisible(bool isVisible)
    {
        if (maskedImageRenderer != null)
        {
            maskedImageRenderer.enabled = isVisible;
        }
    }

    private void ApplyMaskSpriteToRenderer()
    {
        if (pieceMask == null || maskedImageRenderer == null)
        {
            return;
        }

        if (maskIsolation.useCustomRange == true)
        {
            int sortingLayerId = SortingLayer.NameToID(maskIsolation.sortingLayerName);
            if (sortingLayerId == 0 && maskIsolation.sortingLayerName != "Default")
            {
                Debug.LogWarning($"{name} could not find Sorting Layer '{maskIsolation.sortingLayerName}'. Create it in Tags and Layers.");
                sortingLayerId = SortingLayer.NameToID("Default");
            }

            pieceMask.isCustomRangeActive = true;
            pieceMask.frontSortingLayerID = sortingLayerId;
            pieceMask.backSortingLayerID = sortingLayerId;
            pieceMask.frontSortingOrder = maskIsolation.sortingOrder + Mathf.Max(1, maskIsolation.sortingRangePadding);
            pieceMask.backSortingOrder = maskIsolation.sortingOrder - Mathf.Max(1, maskIsolation.sortingRangePadding);

            maskedImageRenderer.sortingLayerID = sortingLayerId;
            maskedImageRenderer.sortingOrder = maskIsolation.sortingOrder;
        }

        maskedImageRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
    }

    private void ApplyWorldTransform(Vector3 worldPosition, Vector3 worldScale)
    {
        Transform targetTransform = maskedImageRenderer.transform;
        targetTransform.position = worldPosition;

        Transform parentTransform = targetTransform.parent;
        if (parentTransform == null)
        {
            targetTransform.localScale = worldScale;
            return;
        }

        Vector3 parentScale = parentTransform.lossyScale;
        targetTransform.localScale = new Vector3(
            SafeDivide(worldScale.x, parentScale.x),
            SafeDivide(worldScale.y, parentScale.y),
            SafeDivide(worldScale.z, parentScale.z));
    }

    private float SafeDivide(float value, float divisor)
    {
        if (Mathf.Approximately(divisor, 0f))
        {
            return value;
        }

        return value / divisor;
    }
}
