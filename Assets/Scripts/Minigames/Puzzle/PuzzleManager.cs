using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "PlayerRoom";

    [Header("PuzzleData")]
    [SerializeField] private Camera mainCamera;

    [SerializeField] private bool useUserIMG;
    [SerializeField] private PuzzleUI PuzzleUI;

    [SerializeField] private List<GameObject> Puzzle = new List<GameObject>();
    [SerializeField] private List<GameObject> PuzzleEnd = new List<GameObject>();

    [SerializeField] private GameObject nextpuzzle;
    [SerializeField] private GameObject nextpuzzle2;
    [SerializeField] private GameObject Finpuzzle;

    [SerializeField] private PictureSystemManager systemManager;
    [SerializeField] private PictureUIGuideText uiGuideText;

    private RaycastHit2D hit;
    private GameObject grabbedTarget;
    private PuzzleData puzzleData;
    private bool isPuzzleFinished;
    //private float puzzleCount;

    void Start()
    {
        PuzzleUI.StartGet();

        //Debug.Log("PuzzleManager Start");
        if (useUserIMG) //나도 이러면 안되는거 아는ㄷ 시발 존나 좃같음아아ㅏㅏㅏㅏㅏㅏㅏㅏㅏㅏㅏㅏㅏㅏㅏ
        {
            PuzzleUI.StartGet();
            PuzzleUI.OnUserPuzzle();
            PuzzleUI.TextureAP();
            PuzzleUI.OnImgCutter();

            Finpuzzle.GetComponent<SpriteRenderer>().sprite = PuzzleUI.TextruAP.GetComponent<SpriteRenderer>().sprite;

            PuzzleUI.User_Puzzle_01.transform.parent = Puzzle[0].transform;
            PuzzleUI.User_Puzzle_02.transform.parent = Puzzle[1].transform;
            PuzzleUI.User_Puzzle_03.transform.parent = Puzzle[2].transform;
            PuzzleUI.User_Puzzle_04.transform.parent = Puzzle[3].transform;
            PuzzleUI.User_Puzzle_05.transform.parent = Puzzle[4].transform;

            PuzzleUI.User_Puzzle_01.transform.localPosition = Vector3.zero;
            PuzzleUI.User_Puzzle_02.transform.localPosition = Vector3.zero;
            PuzzleUI.User_Puzzle_03.transform.localPosition = Vector3.zero;
            PuzzleUI.User_Puzzle_04.transform.localPosition = Vector3.zero;
            PuzzleUI.User_Puzzle_05.transform.localPosition = Vector3.zero;

            var x = PuzzleUI.User_Puzzle_01.transform.GetChild(0).transform.localPosition.x;
            var y = PuzzleUI.User_Puzzle_01.transform.GetChild(0).transform.localPosition.y;
            PuzzleUI.User_Puzzle_01.transform.GetChild(0).transform.localPosition = new Vector3(x, y, 0f);

            x = PuzzleUI.User_Puzzle_02.transform.GetChild(0).transform.localPosition.x;
            y = PuzzleUI.User_Puzzle_02.transform.GetChild(0).transform.localPosition.y;
            PuzzleUI.User_Puzzle_02.transform.GetChild(0).transform.localPosition = new Vector3(x, y, 0f);

            x = PuzzleUI.User_Puzzle_03.transform.GetChild(0).transform.localPosition.x;
            y = PuzzleUI.User_Puzzle_03.transform.GetChild(0).transform.localPosition.y;
            PuzzleUI.User_Puzzle_03.transform.GetChild(0).transform.localPosition = new Vector3(x, y, 0f);

            x = PuzzleUI.User_Puzzle_04.transform.GetChild(0).transform.localPosition.x;
            y = PuzzleUI.User_Puzzle_04.transform.GetChild(0).transform.localPosition.y;
            PuzzleUI.User_Puzzle_04.transform.GetChild(0).transform.localPosition = new Vector3(x, y, 0f);

            x = PuzzleUI.User_Puzzle_05.transform.GetChild(0).transform.localPosition.x;
            y = PuzzleUI.User_Puzzle_05.transform.GetChild(0).transform.localPosition.y;
            PuzzleUI.User_Puzzle_05.transform.GetChild(0).transform.localPosition = new Vector3(x, y, 0f);

        }
    }

    void Update()
    {
        PuzzleFintest();

        if (Input.GetMouseButtonDown(0))
        {
            hit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Input.mousePosition));

            if (hit.collider != null && hit.collider.tag == "Puzzle")
            {
                //Debug.Log("Hit: " + hit.collider.gameObject.name);
                uiGuideText.StopGuide();
                grabbedTarget = hit.collider.gameObject;
                puzzleData = hit.collider.GetComponent<PuzzleData>();
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (grabbedTarget != null && puzzleData.onpuzzlesuccess != true)
            {
                PuzzleMove();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (grabbedTarget != null && puzzleData.onpuzzlesuccess != true)
            {
                //Debug.Log("Hit: " + grabbedTarget.gameObject.name);
                puzzleData.ReleasePiece();
                grabbedTarget = null;
                puzzleData = null;
            }
        }
    }

    private void PuzzleMove()
    {
        Vector3 CamPos = mainCamera.ScreenPointToRay(Input.mousePosition).origin;
        grabbedTarget.transform.position = new Vector3(CamPos.x, CamPos.y, 0);
    }

    private void PuzzleFintest()
    {
        if (isPuzzleFinished)
        {
            return;
        }

        for (int i = 0; i < 5; i++)
        {
            if (!Puzzle[i].GetComponent<PuzzleData>().onpuzzlesuccess)
            {
                return;
            }
        }
        PuzzleFin();
        return;
    }

    private void PuzzleFin()
    {
        if (isPuzzleFinished)
        {
            return;
        }

        isPuzzleFinished = true;
        Debug.Log("성공");
        PuzzleUI.OffImgCutter();
        StartCoroutine(PuzzleFinC());
    }

    private IEnumerator PuzzleFinC()
    {
        FadeManager.Instance.FadeOut();

        yield return new WaitForSeconds(1.2f);

        FadeManager.Instance.FadeIn();
        Finpuzzle.SetActive(true);
        

        if (nextpuzzle == null && nextpuzzle2 == null)
        {
            yield return new WaitForSeconds(4f);
            puzzleEnd();
        }
        else
        {
            yield return new WaitForSeconds(3f);
            FadeManager.Instance.FadeOut();
            yield return new WaitForSeconds(1.2f);
            if (PuzzleUI.TextruAP.GetComponent<SpriteRenderer>().sprite != null && nextpuzzle2 != null)
            {
                nextpuzzle2.SetActive(true);
            }
            else
            {
                nextpuzzle.SetActive(true);
            }
            FadeManager.Instance.FadeIn();
            gameObject.SetActive(false);
        }
    }

    private void puzzleEnd()
    {
        SceneLoadManager.Instance.LoadScene(nextSceneName);
    }
}
