using System;
using UnityEngine;

public class CursorBtn : MonoBehaviour
{
    [SerializeField] private CursorChanger cursorChanger;
    [SerializeField] private CursorChase cursorChase;
    public event Action<CursorType> OnChangeTool;

    public void OnClickShampoo()
    {
        OnChangeTool?.Invoke(CursorType.Shampoo);
        //SetImage(CursorType.Shampoo);
    }

    public void OnClickShower()
    {
        OnChangeTool?.Invoke(CursorType.Shower);
        //SetImage(CursorType.Shower);
    }

    public void OnClickTowel()
    {
        OnChangeTool?.Invoke(CursorType.Towel);
        //SetImage(CursorType.Towel);
    }
}
