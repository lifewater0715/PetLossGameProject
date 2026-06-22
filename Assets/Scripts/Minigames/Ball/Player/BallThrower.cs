using UnityEngine;

public class BallThrower : MonoBehaviour
{
    [SerializeField] private BallThrowController ballThrowController;
    [SerializeField] private BallSystemManager ballSystemManager;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private Rigidbody2D ballRigid;
    [SerializeField] private SFXAudio dogSound;

    [SerializeField] private float spinPower = -720f;

#if UNITY_EDITOR
    [ContextMenu("Auto Assign")]
    private void AutoAssign()
    {
        ballThrowController = GameObject.Find("BallThrowManager").GetComponent<BallThrowController>();
        throwPoint = GameObject.Find("ThrowPoint").GetComponent<Transform>();
        ballRigid = GameObject.Find("Ball").GetComponent<Rigidbody2D>();
    }
#endif

    private void Start()
    {
        ballRigid.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (ballThrowController == null) return;
        
        ballThrowController.OnThrowWithPower += Throw;
    }

    private void OnDisable()
    {
        if (ballThrowController == null) return;
        
        ballThrowController.OnThrowWithPower -= Throw;
    }

    private void Throw(float power)
    {
        ballThrowController.SetCanThrow(false);
        if (ballSystemManager.Turn != 1) dogSound.PlaySound();

        ballRigid.gameObject.SetActive(true);
        ballRigid.transform.position = throwPoint.position;
        ballRigid.linearVelocity = GetThrowVelocity(power);

        ballRigid.angularVelocity = spinPower;
    }

    private Vector2 GetThrowVelocity(float power)
    {
        Vector2 throwDirection = ballThrowController.GetThrowDirection.normalized;
        
        return throwDirection * power;
    }
}
