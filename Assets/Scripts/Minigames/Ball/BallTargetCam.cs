using UnityEngine;

public class BallTargetCam : MonoBehaviour
{
    [SerializeField] private CamFollowTarget camFollowTarget;
    [SerializeField] private BallInput ballInput;

    [Header("카메라 대상 오브젝트")]
    [SerializeField] private Transform ballObj;
    [SerializeField] private Transform markingObj;

    private void OnEnable()
    {
        ballInput.OnChargeStarted += OnMarkingTarget;
    }

    private void OnDisable()
    {
        ballInput.OnChargeStarted -= OnMarkingTarget;
    }

    private void OnBallTarget()
    {
        camFollowTarget.SetCamTarget(ballObj);
    }

    private void OnMarkingTarget()
    {
        camFollowTarget.SetCamTarget(markingObj);
    }
}
