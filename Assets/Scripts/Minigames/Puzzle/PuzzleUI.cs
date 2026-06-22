using UnityEngine;

public class PuzzleUI : MonoBehaviour
{
    [SerializeField] private GameObject LoaderObj;
    [SerializeField] private GameObject ImgCutterObj;

    public GameObject User_Puzzle_01;
    public GameObject User_Puzzle_02;
    public GameObject User_Puzzle_03;
    public GameObject User_Puzzle_04;
    public GameObject User_Puzzle_05;

    [SerializeField]private ImgLoader imgLoader;
    [SerializeField]public GameObject TextruAP;
    [SerializeField] private GameObject borderSprite;
    [SerializeField] private GameObject borderImage;

    void Start()
    {
        StartGet();

        if (borderImage == null | borderSprite == null) return;

        borderSprite.SetActive(false);
        borderImage.SetActive(false);
    }

    void Update()
    {

    }

    public void TextureAP()
    {
        TextruAP.SetActive(true);
        TextruAP.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0.2f);
    }

    [ContextMenu("씨발 가져오ㅣㅣㅣ")]
    public void StartGet()
    {
        LoaderObj = GameObject.FindWithTag("ImgLoders");
        ImgCutterObj = LoaderObj.transform.Find("ImagCutter").gameObject;
        ImgCutterObj.SetActive(false);

        LoaderObj = GameObject.FindWithTag("ImgLoders");
        imgLoader = GameObject.Find("Imgloader").GetComponent<ImgLoader>();

        TextruAP = LoaderObj.transform.Find("UserImg").gameObject.transform.Find("Textrues").gameObject;

        User_Puzzle_01 = GameObject.Find("User_Puzzle_01");
        User_Puzzle_02 = GameObject.Find("User_Puzzle_02");
        User_Puzzle_03 = GameObject.Find("User_Puzzle_03");
        User_Puzzle_04 = GameObject.Find("User_Puzzle_04");
        User_Puzzle_05 = GameObject.Find("User_Puzzle_05");
    }

    public void LoadImg()
    {
        imgLoader.OnenUI();

        if (borderImage == null | borderSprite == null) return;
        
        borderSprite.SetActive(true);
        borderImage.SetActive(true);
    }

    public void CUTImg()
    {
        imgLoader.ConfirmImageTransform();
    }

    public void OnImgCutter()
    {
        ImgCutterObj.SetActive(true);
    }

    public void OffImgCutter()
    {
        ImgCutterObj.SetActive(false);
    }

    public void OnUserPuzzle()
    {
        User_Puzzle_01.GetComponent<SpriteMask>().enabled = true;
        User_Puzzle_02.GetComponent<SpriteMask>().enabled = true;
        User_Puzzle_03.GetComponent<SpriteMask>().enabled = true;
        User_Puzzle_04.GetComponent<SpriteMask>().enabled = true;
        User_Puzzle_05.GetComponent<SpriteMask>().enabled = true;
    }

    public void OffUserPuzzle()
    {
        User_Puzzle_01.GetComponent<SpriteMask>().enabled = false;
        User_Puzzle_02.GetComponent<SpriteMask>().enabled = false;
        User_Puzzle_03.GetComponent<SpriteMask>().enabled = false;
        User_Puzzle_04.GetComponent<SpriteMask>().enabled = false;
        User_Puzzle_05.GetComponent<SpriteMask>().enabled = false;
    }
}
