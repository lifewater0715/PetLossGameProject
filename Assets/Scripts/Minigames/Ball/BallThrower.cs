using UnityEngine;

public class BallThrower : MonoBehaviour
{
    [SerializeField] private BallInput ballInput;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private Rigidbody2D ballRigid;
    [SerializeField] private Camera mainCam;

    private void OnEnable()
    {
        ballInput.onThrow += Throw;
    }

    private void OnDisable()
    {
        ballInput.onThrow -= Throw;
    }

    private void Throw(float power)
    {
        ballRigid.transform.position = transform.position;

        ballRigid.linearVelocity = GetThrowVelocity(power);
    }

    private Vector2 GetThrowVelocity(float power)
    {
        Vector2 throwDirection = new Vector2(1f, 1f).normalized;
        
        return throwDirection * power;
    }
}
