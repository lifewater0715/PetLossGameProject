using System.Collections;
using UnityEngine;

public class ShampooEvent : MonoBehaviour, IPropsEvent
{
    [SerializeField] private string loadSceneName = "MiniGameShampoo";

    public void Play()
    {
        StartCoroutine(CEventStart());
    }

    private IEnumerator CEventStart()
    {
        Debug.Log("샴푸 상호작용");
        SceneLoadManager.Instance.LoadScene(loadSceneName);
        yield return null;
    }
}
