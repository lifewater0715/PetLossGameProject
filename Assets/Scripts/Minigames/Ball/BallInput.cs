using UnityEngine;

public class BallInput : MonoBehaviour
{
    [SerializeField] private BallThrowController ballThrowController;

    private bool throwingReady = false;

#if UNITY_EDITOR
    [ContextMenu("Auto Assign")]
    private void AutoAssign()
    {
        ballThrowController = GameObject.Find("BallThrowManager").GetComponent<BallThrowController>();
    }
#endif

    private void Update()
    {
        if (FadeManager.Instance.IsFading) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ballThrowController.StartBallThrow();
            throwingReady = true;
        }

        if (Input.GetKey(KeyCode.Space) && throwingReady)
        {
            ballThrowController.HoldBallThrow();
        }

        if (Input.GetKeyUp(KeyCode.Space) && throwingReady)
        {
            ballThrowController.FinishBallThrow();
        }
    }

    public void ReadyToThrowing()
    {
        throwingReady = false;
    }
}
