using System.Collections;
using UnityEngine;

public class BallTargetCam : MonoBehaviour
{
    [SerializeField] private CamFollowTarget camFollowTarget;
    [SerializeField] private BallThrowController ballThrowController;

    [Header("카메라 대상 오브젝트")]
    [SerializeField] private Transform dogObj;
    [SerializeField] private Transform markerObj;
    [SerializeField] private Transform centerObj;

#if UNITY_EDITOR
    [ContextMenu("Auto Assign")]
    private void AutoAssign()
    {
        camFollowTarget = GameObject.Find("Main Camera").GetComponent<CamFollowTarget>();
        ballThrowController = GameObject.Find("BallThrowManager").GetComponent<BallThrowController>();

        dogObj = GameObject.Find("Dog").GetComponent<Transform>();
        markerObj = GameObject.Find("Marker").GetComponent<Transform>();
        centerObj = GameObject.Find("CamCenter").GetComponent<Transform>();
    }
#endif

    private void OnEnable()
    {
        if (ballThrowController == null) return;

        ballThrowController.OnChargeStarted += OnMarkingTarget;
        ballThrowController.OnThrow += OnDogTarget;
    }

    private void OnDisable()
    {
        if (ballThrowController == null) return;
        
        ballThrowController.OnChargeStarted -= OnMarkingTarget;
        ballThrowController.OnThrow -= OnDogTarget;
    }

    private void OnDogTarget()
    {
        camFollowTarget.SetCamSmoothTime(0.2f);
        camFollowTarget.SetCamTarget(dogObj);
        camFollowTarget.OnCamSmoothSize(5f);
    }

    private void OnMarkingTarget()
    {
        camFollowTarget.OnCamSmoothTarget(centerObj, 0.2f);
        camFollowTarget.OnCamSmoothSize(7f);
    }

    public void OnCenterTarget()
    {
        camFollowTarget.SetCamSmoothTime(0.2f);
        camFollowTarget.SetCamTarget(centerObj);
    }
}
