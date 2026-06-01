using UnityEngine;

public class ShampooSystemManager : MonoBehaviour
{
    [SerializeField] private CursorEvent cursorEvent;
    [SerializeField] private CursorBtn cursorBtn;
    [SerializeField] private ShampooDogAnimation shampooDogAnimation;

    [SerializeField] private float chargeShampooSpeed = 100f;
    [SerializeField] private float chargeShowerSpeed = 100f;
    [SerializeField] private float chargeTowelSpeed = 100f;

    private CursorType _cursorType = CursorType.None;
    private float _charged = 0f;

    private int _turn = 1;

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

        switch (_cursorType)
        {
            case CursorType.Shampoo:
                shampooDogAnimation.StartSoapAnimation();
                ShampooInteract();
                break;
            case CursorType.Shower:
                shampooDogAnimation.StartShowerAnimation();
                ShowerInteract();
                break;
            case CursorType.Towel:
                TowelInteract();
                break;
            default:
                return;
        }
    }

    private void ShampooInteract()
    {
        if (_turn != 1) return;

        _charged += Time.deltaTime * chargeShampooSpeed;

        if (_charged < 100f) return;

        _charged = 0f;
        _turn++;
        Debug.Log("샴푸 이벤트 완료!");
    }

    private void ShowerInteract()
    {
        if (_turn != 2) return;
        
        _charged += Time.deltaTime * chargeShowerSpeed;

        if (_charged < 100f) return;

        _charged = 0f;
        _turn++;
        Debug.Log("샤워기 이벤트 완료!");
    }
    
    private void TowelInteract()
    {
        if (_turn != 3) return;

        _charged += Time.deltaTime * chargeTowelSpeed;

        if (_charged < 100f) return;

        _charged = 0f;
        _turn++;
        Debug.Log("타월 이벤트 완료!");
    }

}
