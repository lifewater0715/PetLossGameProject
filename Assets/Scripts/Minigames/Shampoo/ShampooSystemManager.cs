using UnityEngine;

public class ShampooSystemManager : MonoBehaviour
{
    [SerializeField] private CursorEvent cursorEvent;
    [SerializeField] private CursorBtn cursorBtn;

    private CursorType _cursorType = CursorType.None;

    private void OnEnable()
    {
        cursorEvent.OnRubbed += TryInteract;
        cursorBtn.OnChangeTool += ToolType;
    }

    private void OnDisable()
    {
        cursorEvent.OnRubbed -= TryInteract;
        cursorBtn.OnChangeTool -= ToolType;
    }

    private void ToolType(CursorType type)
    {
        _cursorType = type;
    }

    private void TryInteract()
    {
        if (_cursorType == CursorType.None) return;
        
        Debug.Log("강아지 쓰담쓰담 중");
    }
}
