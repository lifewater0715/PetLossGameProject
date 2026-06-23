using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CutSceneFade))]
[RequireComponent(typeof(CutSceneTextFade))]
public class CutSceneManager : MonoBehaviour
{
    private CutSceneFade cutSceneFade;
    private CutSceneTextFade cutSceneTextFade;

    private void Awake()
    {
        cutSceneFade = GetComponent<CutSceneFade>();
        cutSceneTextFade = GetComponent<CutSceneTextFade>();
    }

    public IEnumerator StartCutScene()
    {
        FadeManager.Instance.HalfFadeOut();
        cutSceneFade.FadeIn();
        cutSceneTextFade.FadeIn();

        yield return new WaitForSecondsRealtime(5f);
        FadeManager.Instance.HalfFadeIn();
        cutSceneFade.FadeOut();
        cutSceneTextFade.FadeOut();

        yield return new WaitForSecondsRealtime(2f);
    }
}
