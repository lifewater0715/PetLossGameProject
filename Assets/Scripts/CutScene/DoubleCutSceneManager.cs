using System.Collections;
using UnityEngine;

public class MultiCutSceneManager : MonoBehaviour
{
    [SerializeField] private CutSceneFade[] cutSceneFade;

    public IEnumerator StartCutScene()
    {
        FadeManager.Instance.HalfFadeOut();

        for (int i = 0; i < cutSceneFade.Length; i++)
        {
            cutSceneFade[i].FadeIn();
        }
        

        yield return new WaitForSecondsRealtime(5f);
        FadeManager.Instance.HalfFadeIn();

        for (int i = 0; i < cutSceneFade.Length; i++)
        {
            cutSceneFade[i].FadeOut();
        }

        yield return new WaitForSecondsRealtime(2f);
    }
}
