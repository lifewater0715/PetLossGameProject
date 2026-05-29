using System.Collections;
using UnityEngine;

public class LeashEvent : MonoBehaviour, IPropsEvent
{
    [SerializeField] private string loadSceneName = "MiniGameLeash";

    public void Play()
    {
        StartCoroutine(CEventStart());
    }

    private IEnumerator CEventStart()
    {
        Debug.Log("개 목줄 상호작용");
        //SceneLoadManager.Instance.LoadScene(loadSceneName);
        yield return null;
    }
}
