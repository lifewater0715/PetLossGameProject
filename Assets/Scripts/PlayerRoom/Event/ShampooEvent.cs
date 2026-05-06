using System.Collections;
using UnityEngine;

public class ShampooEvent : MonoBehaviour, IPropsEvent
{
    public void Play()
    {
        StartCoroutine(CEventStart());
    }

    private IEnumerator CEventStart()
    {
        Debug.Log("샴푸 상호작용");
        yield return null;
    }
}
