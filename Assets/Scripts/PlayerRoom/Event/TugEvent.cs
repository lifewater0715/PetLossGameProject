using System.Collections;
using UnityEngine;

public class TugEvent : MonoBehaviour, IPropsEvent
{
    [SerializeField] private string loadSceneName = "MiniGameTug";

    public void Play()
    {
        StartCoroutine(CEventStart());
    }

    private IEnumerator CEventStart()
    {
        Debug.Log("터그 상호작용");
        SceneLoadManager.Instance.LoadScene(loadSceneName);
        yield return null;
    }
}
