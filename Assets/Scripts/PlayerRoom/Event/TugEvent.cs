using System.Collections;
using UnityEngine;

public class TugEvent : MonoBehaviour, IPropsEvent
{
    public void Play()
    {
        StartCoroutine(CEventStart());
    }

    private IEnumerator CEventStart()
    {
        Debug.Log("터그 상호작용");
        yield return null;
    }
}
