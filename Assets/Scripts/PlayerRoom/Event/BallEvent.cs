using System.Collections;
using UnityEngine;

public class BallEvent : MonoBehaviour, IPropsEvent
{
    public void Play()
    {
        StartCoroutine(CEventStart());
    }

    private IEnumerator CEventStart()
    {
        Debug.Log("장난감 공 상호작용");
        yield return null;
    }
}
