using UnityEngine;

public class BallThrower : MonoBehaviour
{
    [SerializeField] private BallInput ballInput;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private Rigidbody2D ballRigid;
    [SerializeField] private DogTargetBall dogTargetBall;

    private void Start()
    {
        ballRigid.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        ballInput.OnThrow += Throw;
    }

    private void OnDisable()
    {
        ballInput.OnThrow -= Throw;
    }

    private void Throw(float power)
    {
        ballRigid.gameObject.SetActive(true);
        
        dogTargetBall.StartTracking();

        ballRigid.transform.position = throwPoint.position;

        ballRigid.linearVelocity = GetThrowVelocity(power);
    }

    private Vector2 GetThrowVelocity(float power)
    {
        Vector2 throwDirection = new Vector2(1f, 1f).normalized;
        
        return throwDirection * power;
    }
}
