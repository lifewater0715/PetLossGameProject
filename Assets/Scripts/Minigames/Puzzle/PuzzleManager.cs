using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    PuzzleData puzzleData;
    [SerializeField] private GameObject puzzleStartPoint;
    [SerializeField] private GameObject puzzleTargetPoint;
    [SerializeField] private int puzzleNumber;
    [SerializeField] private float puzzleMoveSpeed;

    [SerializeField] Camera mainCamera;
    private RaycastHit2D hit;
    private GameObject grabbedTarget;
    
    void Start()
    {
        Debug.Log("PuzzleManager Start");
        
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            hit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Input.mousePosition));
            Debug.Log("FireLay");

            if (hit.collider != null)
            {
                Debug.Log("Hit: " + hit.collider.gameObject.name);
                grabbedTarget = hit.collider.gameObject;

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
                Debug.Log("Hit: " + grabbedTarget.gameObject.name);
                grabbedTarget = null;
            }
        }
    }

    private void PuzzleMove()
    {
        grabbedTarget.transform.position = mainCamera.ScreenPointToRay(Input.mousePosition).origin;
    }
}
