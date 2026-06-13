using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CutSceneFade))]
public class CutSceneManager : MonoBehaviour
{
    private CutSceneFade cutSceneFade;

    private void Awake()
    {
        cutSceneFade = GetComponent<CutSceneFade>();
    }

    public IEnumerator StartCutScene()
    {
        FadeManager.Instance.HalfFadeOut();
        cutSceneFade.FadeIn();

        yield return new WaitForSecondsRealtime(5f);
        FadeManager.Instance.HalfFadeIn();
        cutSceneFade.FadeOut();

        yield return new WaitForSecondsRealtime(2f);
    }
}
