using System.Collections;
using UnityEngine;

public class CutSceneRemind : MonoBehaviour
{
    [SerializeField] private MultiCutSceneManager pictureCutScene;
    [SerializeField] private CutSceneManager ballCutScene;
    [SerializeField] private CutSceneManager tugCutScene;
    [SerializeField] private CutSceneManager leashCutScene;
    [SerializeField] private CutSceneManager shampooCutScene;
    [SerializeField] private PlayerRoomUIGuideText uiGuideText;

    private bool nowShowing = false;
    public bool NowShowing => nowShowing;

    public enum PropsType
    {
        None, Picture, Ball, Tug, Leash, Shampoo
    };

    [SerializeField] private PropsType propsType;

    public void SetPropsNone()
    {
        propsType = PropsType.None;
    }

    public void SetPropsPicture()
    {
        propsType = PropsType.Picture;
    }

    public void SetPropsBall()
    {
        propsType = PropsType.Ball;
    }

    public void SetPropsTug()
    {
        propsType = PropsType.Tug;
    }

    public void SetPropsLeash()
    {
        propsType = PropsType.Leash;
    }

    public void SetPropsShampoo()
    {
        propsType = PropsType.Shampoo;
    }

    public void OnButtonClick()
    {
        StartCoroutine(COnButtonClick());
    }

    private IEnumerator COnButtonClick()
    {
        switch (propsType)
        {
            case PropsType.Picture:
                nowShowing = true;
                yield return StartCoroutine(pictureCutScene.StartCutScene());
                nowShowing = false;
                break;

            case PropsType.Ball:
                nowShowing = true;
                yield return StartCoroutine(ballCutScene.StartCutScene());
                nowShowing = false;
                break;

            case PropsType.Tug:
                nowShowing = true;
                yield return StartCoroutine(tugCutScene.StartCutScene());
                nowShowing = false;
                break;

            case PropsType.Leash:
                nowShowing = true;
                yield return StartCoroutine(leashCutScene.StartCutScene());
                nowShowing = false;
                break;

            case PropsType.Shampoo:
                nowShowing = true;
                yield return StartCoroutine(shampooCutScene.StartCutScene());
                nowShowing = false;
                break;

            case PropsType.None:
                uiGuideText.StartGuide();
                yield return null;
                break;
        }
    }
}
