using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [Header("PuzzleData")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private List<GameObject> Puzzle = new List<GameObject>();
    [SerializeField] private GameObject nextpuzzle;
    [SerializeField] private GameObject Finpuzzle;


    private RaycastHit2D hit;
    private GameObject grabbedTarget;
    private PuzzleData puzzleData;
    private bool isPuzzleFinished;
    //private float puzzleCount;

    void Start()
    {
        //Debug.Log("PuzzleManager Start");
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
        StartCoroutine(PuzzleFinC());
    }

    private IEnumerator PuzzleFinC()
    {
        FadeManager.Instance.FadeOut();

        yield return new WaitForSeconds(1.2f);
        
        FadeManager.Instance.FadeIn();
        Finpuzzle.SetActive(true);
        yield return new WaitForSeconds(1.2f);

        FadeManager.Instance.FadeOut();
        yield return new WaitForSeconds(1.2f);

        if (nextpuzzle == null)
        {
             puzzleEnd();
        }
        else nextpuzzle.SetActive(true);
        FadeManager.Instance.FadeIn();
        gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        
    }

    private void puzzleEnd()
    {
        
    }
}
