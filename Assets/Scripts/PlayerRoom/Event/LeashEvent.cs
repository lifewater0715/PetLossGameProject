using System.Collections;
using UnityEngine;

public class LeashEvent : MonoBehaviour, IPropsEvent
{
    public void Play()
    {
        StartCoroutine(CEventStart());
    }

    private IEnumerator CEventStart()
    {
        Debug.Log("개목줄 상호작용");
        yield return null;
    }
}
