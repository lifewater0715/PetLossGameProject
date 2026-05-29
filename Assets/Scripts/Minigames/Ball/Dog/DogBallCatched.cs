using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BallDogAnimation))]
public class DogBallCatched : MonoBehaviour
{
    [SerializeField] private LayerMask ballLayer;
    [SerializeField] private CamFollowTarget camFollowTarget;
    [SerializeField] private BallTargetCam ballTargetCam;
    [SerializeField] private BallInput ballInput;

    [SerializeField] private BallSystemManager ballSystemManager;

    private BallDogAnimation ballDogAnimation;

#if UNITY_EDITOR
    [ContextMenu("Auto Assign")]
    private void AutoAssign()
    {
        ballLayer = LayerMask.GetMask("Ball");
    }
#endif

    private void Awake()
    {
        ballDogAnimation = GetComponent<BallDogAnimation>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((ballLayer.value & (1 << collision.gameObject.layer)) == 0) return;

        OnCatchedBall(collision);
    }

    private void OnCatchedBall(Collider2D collision)
    {
        Debug.Log("공 잡음");
        collision.gameObject.SetActive(false);

        camFollowTarget.SetCamSmoothTime(0.1f);
        ballDogAnimation.StopRunAnim();
        ballDogAnimation.StartCatchIdleAnim();
        
        StartCoroutine(CBallCamReset());
    }

    private IEnumerator CBallCamReset()
    {
        yield return new WaitForSeconds(2f);

        camFollowTarget.SetCamSmoothTime(0.2f);
        ballTargetCam.OnCenterTarget();

        ballSystemManager.DogPositionReset();
        ballSystemManager.TurnCheck();

        ballInput.ReadyToThrowing();
    }
}
