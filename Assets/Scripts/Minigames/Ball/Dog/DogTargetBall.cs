using UnityEngine;

public class DogTargetBall : MonoBehaviour
{
    [SerializeField] private Transform ballObj;
    [SerializeField] private bool isTracking = false;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float catchDistance = 0.1f;

    private void Update()
    {
        if (!isTracking) return;

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

    public void StartTracking()
    {
        isTracking = true;
    }

    public void StopTracking()
    {
        isTracking = false;
    }
}
