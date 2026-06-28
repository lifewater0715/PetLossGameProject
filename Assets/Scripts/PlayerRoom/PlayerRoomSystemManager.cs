using System.Collections;
using UnityEngine;

public class PlayerRoomSystemManager : MonoBehaviour
{
    [SerializeField] private PropsGlowHighlightController glow;
    [SerializeField] private PlayerRoomSpotlight spotlight;
    [SerializeField] private GameObject gaurdImage;
    [SerializeField] private string nextSceneName = "Ending";
    [SerializeField] private BoxPanelAnim boxPanelAnim;

    private void Start()
    {
        SetGlowAnim();
    }

    private void SetGlowAnim()
    {
        switch (PropsTurn.Turn)
        {
            case 1:
                glow.SetPictureHighlight(true);
                break;
            case 2:
                glow.SetBallHighlight(true);
                BGMManager.Instance.SetFilterMode(BGMManager.AudioLevel.VeryHigh);
                spotlight.PlaySpotlight();
                break;
            case 3:
                glow.SetTugHighlight(true);
                BGMManager.Instance.SetFilterMode(BGMManager.AudioLevel.High);
                break;
            case 4:
                glow.SetLeashHighlight(true);
                BGMManager.Instance.SetFilterMode(BGMManager.AudioLevel.Middle);
                break;
            case 5:
                glow.SetShampooHighlight(true);
                BGMManager.Instance.SetFilterMode(BGMManager.AudioLevel.Low);
                break;
            case 6:
                StartCoroutine(MoveEndingScene());
                break;
        }

        if (PropsTurn.Turn > 2) 
        {
            boxPanelAnim.SetCanAccessBox(true);
            gaurdImage.SetActive(false);
        }
    }

    private IEnumerator MoveEndingScene()
    {
        yield return new WaitForSeconds(4f);
        SceneLoadManager.Instance.LoadScene(nextSceneName);
        //BGMManager.Instance.StopSound();
    }
}
