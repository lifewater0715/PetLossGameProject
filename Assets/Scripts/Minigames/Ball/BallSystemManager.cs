using UnityEngine;

public class BallSystemManager : MonoBehaviour
{
    [SerializeField] private Transform beginningDogTransform;
    [SerializeField] private Transform dogObj;

    [SerializeField] private BallThrowController ballThrowController;

    [SerializeField] private BallPlayerAnimation ballPlayerAnimation;
    [SerializeField] private BallDogAnimation ballDogAnimation;

    [Header("턴")]
    [SerializeField] private int turn = 0;
    [SerializeField] private string nextSceneName = "PlayerRoom";

    public void DogPositionReset()
    {
        turn++;
        dogObj.position = beginningDogTransform.position;
        ballThrowController.SetCanThrow(true);

        ballDogAnimation.StopCatchIdleAnim();
        ballPlayerAnimation.StopShootAnim();
    }

    public void TurnCheck()
    {
        if (turn < 3 ) return;
        Debug.Log("턴 종료");

        SceneLoadManager.Instance.LoadScene(nextSceneName);
    }
}
