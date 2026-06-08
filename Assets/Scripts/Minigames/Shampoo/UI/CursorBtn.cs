using System;
using UnityEngine;

public class CursorBtn : MonoBehaviour
{
    [SerializeField] private CursorChanger cursorChanger;
    [SerializeField] private CursorChase cursorChase;
    [SerializeField] private CursorSpriteAnimation cursorSpriteAnim;

    public event Action<CursorType> OnChangeTool;

    public void OnClickShampoo()
    {
        cursorSpriteAnim.Stop();
        OnChangeTool?.Invoke(CursorType.Shampoo);
        //SetImage(CursorType.Shampoo);
    }

    public void OnClickShower()
    {
        OnChangeTool?.Invoke(CursorType.Shower);
        cursorSpriteAnim.Play();
        //SetImage(CursorType.Shower);
    }

    public void OnClickTowel()
    {
        cursorSpriteAnim.Stop();
        OnChangeTool?.Invoke(CursorType.Towel);
        //SetImage(CursorType.Towel);
    }
}
