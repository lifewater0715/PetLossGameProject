using UnityEngine;

public class BallTargetCam : MonoBehaviour
{
    [SerializeField] private CamFollowTarget camFollowTarget;
    [SerializeField] private BallInput ballInput;

    [Header("카메라 대상 오브젝트")]
    [SerializeField] private Transform DogObj;
    [SerializeField] private Transform markingObj;

    private void OnEnable()
    {
        ballInput.OnChargeStarted += OnMarkingTarget;
        ballInput.OnThrow += OnDogTarget;
    }

    private void OnDisable()
    {
        ballInput.OnChargeStarted -= OnMarkingTarget;
        ballInput.OnThrow -= OnDogTarget;
    }

    private void OnDogTarget(float _)
    {
        camFollowTarget.SetCamSmoothTime(0.2f);
        camFollowTarget.SetCamTarget(DogObj);
    }

    private void OnMarkingTarget()
    {
        camFollowTarget.SetCamSmoothTime(0f);
        camFollowTarget.SetCamTarget(markingObj);
    }
}
