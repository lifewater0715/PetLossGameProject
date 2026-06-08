using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [Header("PuzzleData")]
    [SerializeField] Camera mainCamera;
    private RaycastHit2D hit;
    private GameObject grabbedTarget;
    private PuzzleData puzzleData;
    
    void Start()
    {
        //Debug.Log("PuzzleManager Start");
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            hit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Input.mousePosition));

            if (hit.collider != null)
            {
                //Debug.Log("Hit: " + hit.collider.gameObject.name);
                grabbedTarget = hit.collider.gameObject;
                puzzleData = hit.collider.GetComponent<PuzzleData>();
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (grabbedTarget != null)
            {
                PuzzleMove();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (grabbedTarget != null)
            {
                //Debug.Log("Hit: " + grabbedTarget.gameObject.name);
                puzzleData.ClickSet(true);
                grabbedTarget = null;
            }
        }
    }

    private void PuzzleMove()
    {
        Vector3 CamPos = mainCamera.ScreenPointToRay(Input.mousePosition).origin;
        grabbedTarget.transform.position = new Vector3(CamPos.x, CamPos.y, 0);
    }
}
