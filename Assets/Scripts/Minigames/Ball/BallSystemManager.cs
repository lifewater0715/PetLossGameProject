using System.Collections;
using UnityEngine;

public class BallSystemManager : MonoBehaviour
{
    [SerializeField] private Transform beginningDogTransform;
    [SerializeField] private Transform dogObj;
    [SerializeField] private BallInput ballInput;

    [SerializeField] private BallThrowController ballThrowController;

    [SerializeField] private BallPlayerAnimation ballPlayerAnimation;
    [SerializeField] private BallDogAnimation ballDogAnimation;

    [SerializeField] private CutSceneManager cutSceneManager;

    [SerializeField] private string nextSceneName = "PlayerRoom";

    private int turn = 0;
    public int Turn => turn;

    private void Start()
    {
        BGMManager.Instance.SetFilterMode(BGMManager.AudioLevel.None);
    }

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

        StartCoroutine(ShowCutScene());
    }

    private IEnumerator ShowCutScene()
    {
        ballInput.SetCanThrowing(false);
        yield return StartCoroutine(cutSceneManager.StartCutScene());
        SceneLoadManager.Instance.LoadScene(nextSceneName);
    }
}
