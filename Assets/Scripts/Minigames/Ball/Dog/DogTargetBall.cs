using UnityEngine;

public class DogTargetBall : MonoBehaviour
{
    [SerializeField] private Transform ballObj;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float catchDistance = 0.1f;
    [SerializeField] private BallThrowController ballThrowController;

    private bool _isTracking = false;

#if UNITY_EDITOR
    [ContextMenu("Auto Assign")]
    private void AutoAssign()
    {
        ballObj = GameObject.Find("Ball").GetComponent<Transform>();
        ballThrowController = GameObject.Find("BallThrowManager").GetComponent<BallThrowController>();
    }
#endif

    private void Update()
    {
        if (!_isTracking) return;

        float distanceX = Mathf.Abs(ballObj.position.x - transform.position.x);

        if (distanceX <= catchDistance)
        {
            StopTracking();
            return;
        }

        Vector3 currentPos = transform.position;

        float nextX = Mathf.MoveTowards(
            currentPos.x, 
            ballObj.position.x, 
            moveSpeed * Time.deltaTime);

        transform.position = new Vector2(nextX, currentPos.y);
    }

    private void OnEnable()
    {
        if (ballThrowController == null) return;
        
        ballThrowController.OnThrow += StartTracking;
    }

    private void OnDisable()
    {
        if (ballThrowController == null) return;
        
        ballThrowController.OnThrow -= StartTracking;
    }

    public void StartTracking()
    {
        _isTracking = true;
    }

    public void StopTracking()
    {
        _isTracking = false;
    }
}
