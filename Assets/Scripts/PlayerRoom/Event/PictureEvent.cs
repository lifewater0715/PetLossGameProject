using System.Collections;
using UnityEngine;

public class PictureEvent : MonoBehaviour, IPropsEvent
{
    [SerializeField] private string loadSceneName = "MiniGamePicture";

    public void Play()
    {
        StartCoroutine(CEventStart());
    }

    private IEnumerator CEventStart()
    {
        Debug.Log("그림 상호작용");
        SceneLoadManager.Instance.LoadScene(loadSceneName);
        yield return null;
    }
}
