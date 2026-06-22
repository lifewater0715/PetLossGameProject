using System.Collections.Generic;
using System.IO;
using SimpleFileBrowser;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ImgLoader : MonoBehaviour
{
    private const string TitleSceneName = "TitleScreen";
    private const string BorderSpriteName = "BorderSprite";
    private const string BorderImageName = "BorderImage";
    private const string ConfirmBtnName = "UserImgCutBtn";

    [SerializeField] private SpriteRenderer UserspriteRenderer;
    [SerializeField] private SpriteRenderer UserspriteRendereBg;
    [SerializeField] private List<PuzzlePieceSlot> puzzlePieceSlots = new List<PuzzlePieceSlot>();
    [SerializeField] private bool applyMaskPiecesOnConfirm = true;

    private GameObject UserImg;
    private GameObject UserImgBG;
    private Texture2D loadedTexture;
    private Sprite loadedSprite;
    private Vector3 savedUserImgPosition;
    private Vector3 savedUserImgBgPosition;
    private Vector3 savedUserImgScale;
    private Vector3 savedUserImgBgScale;

    [SerializeField] private bool ImgCutteMod = false;
    [SerializeField] private bool isEditPaused = false;
    [SerializeField] private bool isImageConfirmed = false;

    //여기서 부턴 내 커스텀 (by byealha)
    [SerializeField] private GameObject imageCutter;
    [SerializeField] private GameObject borderSprite;
    [SerializeField] private GameObject borderImage;
    [SerializeField] private GameObject confirmBtn;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        UserImg = UserspriteRenderer.transform.gameObject;
        UserImgBG = UserspriteRendereBg.transform.gameObject;
        AssignTitleScreenBorders();
    }

    void Update()
    {
        if (ImgCutteMod == false || isImageConfirmed == true)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeImageEdit();
        }

        if (isEditPaused == false)
        {
            ImgMover(UserImg);
            ImgMover(UserImgBG);

            float Scrollpow = Input.mouseScrollDelta.y;
            ImgZoomer(UserImg, Scrollpow);
            ImgZoomer(UserImgBG, Scrollpow);

            if (Input.GetMouseButtonDown(0))
            {
                PauseImageEdit();
            }
        }
    }

    public void PauseImageEdit()
    {
        if (ImgCutteMod == false)
        {
            return;
        }

        isEditPaused = true;
        Debug.Log("Image edit paused. Press Confirm to save or ESC to continue editing.");
        confirmBtn.SetActive(true);
    }

    public void ResumeImageEdit()
    {
        if (ImgCutteMod == false || isImageConfirmed == true)
        {
            return;
        }

        isEditPaused = false;
        UserspriteRendereBg.enabled = true;
        SetPuzzlePiecesVisible(false);
        Debug.Log("Image edit resumed.");
        confirmBtn.SetActive(false);
    }

    [ContextMenu("GG")]
    public void ConfirmImageTransform()
    {
        if (ImgCutteMod == false)
        {
            return;
        }

        if (isEditPaused == false)
        {
            Debug.Log("Pause image editing first, then confirm the current transform.");
            return;
        }

        SaveCurrentImageTransform();
        isImageConfirmed = true;
        ImgCutteMod = false;
        UserspriteRendereBg.enabled = false;

        if (applyMaskPiecesOnConfirm == true)
        {
            ApplyImageToPuzzlePieces();
            SavePuzzlePieceStates();
        }

        Debug.Log("Image transform confirmed and saved.");
    }

    public void SaveCurrentImageTransform()
    {
        savedUserImgPosition = UserImg.transform.position;
        savedUserImgBgPosition = UserImgBG.transform.position;
        savedUserImgScale = UserImg.transform.lossyScale;
        savedUserImgBgScale = UserImgBG.transform.lossyScale;

        Debug.Log($"Saved image position: {savedUserImgPosition}, scale: {savedUserImgScale}");
    }

    public void ApplyImageToPuzzlePieces()
    {
        if (loadedSprite == null)
        {
            Debug.LogWarning("No loaded image sprite to apply.");
            return;
        }

        for (int i = 0; i < puzzlePieceSlots.Count; i++)
        {
            PuzzlePieceSlot slot = puzzlePieceSlots[i];
            if (slot == null)
            {
                continue;
            }

            slot.ConfigurePiece(loadedSprite, savedUserImgPosition, savedUserImgScale);
        }

        SetPuzzlePiecesVisible(true);
    }

    public void SavePuzzlePieceStates()
    {
        for (int i = 0; i < puzzlePieceSlots.Count; i++)
        {
            PuzzlePieceSlot slot = puzzlePieceSlots[i];
            if (slot == null)
            {
                continue;
            }

            slot.SaveCurrentState();
        }
    }

    public void ApplySavedPuzzlePieceStates()
    {
        for (int i = 0; i < puzzlePieceSlots.Count; i++)
        {
            PuzzlePieceSlot slot = puzzlePieceSlots[i];
            if (slot == null)
            {
                continue;
            }

            slot.ApplySavedState();
        }
    }

    public void ClearPuzzlePieceStates()
    {
        for (int i = 0; i < puzzlePieceSlots.Count; i++)
        {
            PuzzlePieceSlot slot = puzzlePieceSlots[i];
            if (slot == null)
            {
                continue;
            }

            slot.ClearPiece();
        }
    }

    public Vector3 GetSavedUserImagePosition()
    {
        return savedUserImgPosition;
    }

    public Vector3 GetSavedUserImageScale()
    {
        return savedUserImgScale;
    }

    public List<PuzzlePieceSlot> GetPuzzlePieceSlots()
    {
        return puzzlePieceSlots;
    }

    public void ImgZoomer(GameObject ZoomTrg, float ScrPow)
    {
        if (ScrPow > 0)
        {
            ZoomTrg.transform.localScale += new Vector3(0.2f, 0.2f, 0f);
        }

        if (ScrPow < 0)
        {
            ZoomTrg.transform.localScale -= new Vector3(0.2f, 0.2f, 0f);
        }
    }

    public void ImgMover(GameObject moveTrg)
    {
        moveTrg.transform.position = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
    }

    [ContextMenu("Open File Browser")]
    public void OnenUI()
    {
        FileBrowser.SetFilters(false, ".png", ".jpg", ".jpeg");
        FileBrowser.ShowLoadDialog(onSuccess, onCancel, 0, false, null, null);
    }

    public void onSuccess(string[] paths)
    {
        ImgCutteMod = true;
        isEditPaused = false;
        isImageConfirmed = false;
        UserspriteRendereBg.enabled = true;
        ClearPuzzlePieceStates();
        Debug.Log("Selected: " + string.Join(", ", paths));

        LoadImage(paths[0]);
    }

    public void onCancel()
    {
        AssignTitleScreenBorders();

        imageCutter.SetActive(false);

        if (borderSprite != null)
        {
            borderSprite.SetActive(false);
        }

        if (borderImage != null)
        {
            borderImage.SetActive(false);
        }

        Debug.Log("File selection cancelled.");
    }

    public void LoadImage(string path)
    {
        byte[] bytes = File.ReadAllBytes(path);
        loadedTexture = new Texture2D(2, 2);
        loadedTexture.LoadImage(bytes);
        loadedSprite = Sprite.Create(loadedTexture, new Rect(0, 0, loadedTexture.width, loadedTexture.height), new Vector2(0.5f, 0.5f));
        UserspriteRenderer.sprite = loadedSprite;
        UserspriteRendereBg.sprite = loadedSprite;
    }

    private void SetPuzzlePiecesVisible(bool isVisible)
    {
        for (int i = 0; i < puzzlePieceSlots.Count; i++)
        {
            PuzzlePieceSlot slot = puzzlePieceSlots[i];
            if (slot == null)
            {
                continue;
            }

            slot.SetVisible(isVisible);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == TitleSceneName)
        {
            AssignTitleScreenBorders();
        }
    }

    private void AssignTitleScreenBorders()
    {
        if (SceneManager.GetActiveScene().name != TitleSceneName)
        {
            return;
        }

        if (borderSprite == null)
        {
            borderSprite = GameObject.Find(BorderSpriteName);
        }

        if (borderImage == null)
        {
            borderImage = GameObject.Find(BorderImageName);
        }

        if (confirmBtn == null)
        {
            confirmBtn = GameObject.Find(ConfirmBtnName);
        }
    }
}
