using System.Collections;
using UnityEngine;

public class PictureEvent : MonoBehaviour, IPropsEvent
{
    public void Play()
    {
        StartCoroutine(CEventStart());
    }

    private IEnumerator CEventStart()
    {
        Debug.Log("그림 상호작용");
        yield return null;
    }
}
