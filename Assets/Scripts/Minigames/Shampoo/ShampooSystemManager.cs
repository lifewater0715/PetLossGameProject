using System.Collections;
using UnityEngine;

public class ShampooSystemManager : MonoBehaviour
{
    [SerializeField] private CursorEvent cursorEvent;
    [SerializeField] private CursorBtn cursorBtn;
    [SerializeField] private ShampooDogAnimation shampooDogAnimation;
    [SerializeField] private ShampooEffect shampooEffect;
    [SerializeField] private ShampooUIGlowHighlightController UIGlow;

    [SerializeField] private float chargeShampooSpeed = 70f;
    [SerializeField] private float chargeShowerSpeed = 15f;
    [SerializeField] private float chargeTowelSpeed = 65f;
    [SerializeField] private float rubChargeMultiplier = 0.05f;
    [SerializeField] private float maxRubMoveDistance = 0.3f;

    [SerializeField] private string nextSceneName = "PlayerRoom";
    [SerializeField] private CutSceneManager cutSceneManager;
    [SerializeField] private ShampooUIGuideText shampooUIGuideText;

    [SerializeField] private GameObject cursorImage;

    private CursorType _cursorType = CursorType.None;
    private bool _wipingAnim = false;
    private float _charged = 0f;

    private int _turn = 1;

    public float Charged => _charged;
    public float NormalizedCharged => Mathf.Clamp01(_charged / 100f);
    public int Turn => _turn;

    private void Start()
    {
        UIGlow.SetShampooUIHighlight(true);
        BGMManager.Instance.SetFilterMode(BGMManager.AudioLevel.None);
    }

    private void OnEnable()
    {
        cursorEvent.OnRubbedDistance += TryInteract;
        cursorBtn.OnChangeTool += ToolType;
        cursorBtn.OnChangeTool += TextInteract;
        cursorEvent.OnRubbedStop += StopInteract;
        cursorEvent.OnHeldOnDog += TryShowerInteract;
        cursorEvent.OnHeldOnDogStop += StopShowerInteract;
    }

    private void OnDisable()
    {
        cursorEvent.OnRubbedDistance -= TryInteract;
        cursorBtn.OnChangeTool -= ToolType;
        cursorBtn.OnChangeTool -= TextInteract;
        cursorEvent.OnRubbedStop -= StopInteract;
        cursorEvent.OnHeldOnDog -= TryShowerInteract;
        cursorEvent.OnHeldOnDogStop -= StopShowerInteract;
    }

    private void ToolType(CursorType type)
    {
        _cursorType = type;
        ResetChargedForNextTool(type);
    }

    private void ResetChargedForNextTool(CursorType type)
    {
        if (_charged < 100f) return;

        if (type == CursorType.Shower && _turn == 2)
        {
            _charged = 0f;
        }

        if (type == CursorType.Towel && _turn == 3)
        {
            _charged = 0f;
        }
    }

    private void TryInteract(float moveDistance)
    {
        if (_cursorType == CursorType.None) return;

        if (_cursorType == CursorType.Shampoo && _turn == 1)
        {
            shampooUIGuideText.StopGuide();
            ShampooInteract(moveDistance);
            UIGlow.SetShampooUIHighlight(false);
        }
        else if (_cursorType == CursorType.Towel && _turn == 3)
        {
            shampooUIGuideText.StopGuide();
            TowelInteract(moveDistance);
            UIGlow.SetTowelUIHighlight(false);
        }
    }

    private void TryShowerInteract()
    {
        if (_cursorType != CursorType.Shower || _turn != 2) return;

        shampooUIGuideText.StopGuide();
        ShowerInteract();
        UIGlow.SetShowerUIHighlight(false);
    }

    private void StopShowerInteract()
    {
        if (_cursorType != CursorType.Shower) return;

        shampooDogAnimation.StopShowerAnimation();
    }

    private void StopInteract()
    {
        if (_cursorType == CursorType.None) return;

        switch (_cursorType)
        {
            case CursorType.Shampoo:
                shampooDogAnimation.StopSoapAnimation();
                break;
            case CursorType.Shower:
                shampooDogAnimation.StopShowerAnimation();
                break;
            case CursorType.Towel:
                if (!_wipingAnim) shampooDogAnimation.StopShowerAnimation();
                else shampooDogAnimation.StopTowelAnimation();
                break;
            default:
                return;
        }
    }

    private void ShampooInteract(float moveDistance)
    {
        if (_turn != 1) return;

        shampooDogAnimation.StartSoapAnimation();
        _charged += GetRubChargeAmount(moveDistance, chargeShampooSpeed);

        if (_charged < 20f) return;
        
        shampooEffect.OnBubble();

        if (_charged < 100f) return;

        _charged = 100f;
        _turn++;
        Debug.Log("샴푸 이벤트 완료!");
        shampooUIGuideText.EndToolShampoo();
        UIGlow.SetShowerUIHighlight(true);
    }

    private void ShowerInteract()
    {
        if (_turn != 2) return;

        shampooDogAnimation.StartShowerAnimation();
        _charged += Time.deltaTime * chargeShowerSpeed;

        shampooEffect.ChangeWaterState(_charged);

        if (_charged < 20f) return;

        shampooEffect.OnShower();

        if (_charged < 100f) return;

        _charged = 100f;
        _turn++;
        Debug.Log("샤워기 이벤트 완료!");
        shampooUIGuideText.EndToolShower();
        UIGlow.SetTowelUIHighlight(true);
    }
    
    private void TowelInteract(float moveDistance)
    {
        if (_turn != 3) return;

        if (!_wipingAnim) shampooDogAnimation.StartShowerAnimation();
        else shampooDogAnimation.StartTowelAnimation();

        _charged += GetRubChargeAmount(moveDistance, chargeTowelSpeed);

        if (_charged < 50f) return;

        if (!_wipingAnim) 
        {
            _wipingAnim = true;
            shampooDogAnimation.ChangeToTowelAnimation();
            //shampooEffect.StopFloorWaterAnimation();
        }

        if (_charged < 100f) return;

        _charged = 100f;
        _turn++;
        Debug.Log("타월 이벤트 완료!");
        StartCoroutine(ShowCutScene());
    }

    private float GetRubChargeAmount(float moveDistance, float chargeSpeed)
    {
        float clampedMoveDistance = Mathf.Min(moveDistance, maxRubMoveDistance);
        return clampedMoveDistance * chargeSpeed * rubChargeMultiplier;
    }

    private IEnumerator ShowCutScene()
    {
        yield return StartCoroutine(cutSceneManager.StartCutScene());
        Cursor.visible = true;
        cursorImage.SetActive(false);
        SceneLoadManager.Instance.LoadScene(nextSceneName);
    }

    private void TextInteract(CursorType type)
    {
        if (type == CursorType.Shampoo && _turn == 1)
        {
            shampooUIGuideText.ShampooBtnClickText();
        }

        if (type == CursorType.Shower && _turn == 2)
        {
            shampooUIGuideText.ShowerBtnClickText();
        }

        if (type == CursorType.Towel && _turn == 3)
        {
            shampooUIGuideText.TowelBtnClickText();
        }
    }
}
