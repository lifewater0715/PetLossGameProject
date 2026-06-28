using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxUIBtn))]
public class BoxPanelAnim : MonoBehaviour
{
    [SerializeField] private BlackBorder blackBorder;
    [SerializeField] private RectTransform targetPos;
    [SerializeField] private RectTransform originalPos;
    [SerializeField] private float moveDuration = 0.3f;
    [SerializeField] private CutSceneRemind cutSceneRemind;
    [SerializeField] private PlayerRoomSpotlight spotlight;
    [SerializeField] private PlayerRoomTutorialUIGuideText guideText;

    private BoxUIBtn boxUIBtn;
    private RectTransform rectTransform;
    private Coroutine moveCoroutine;

    private bool _active = false;
    private bool _duration = false;
    private bool canAccessBox = false;

    private void Awake()
    {
        boxUIBtn = GetComponent<BoxUIBtn>();
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = originalPos.anchoredPosition;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !_active && !_duration && !cutSceneRemind.NowShowing && canAccessBox)
        {
            BeginPannel();
        }

        if ((Input.GetKeyDown(KeyCode.Tab) 
        || Input.GetKeyDown(KeyCode.Escape)) && _active && !_duration && !cutSceneRemind.NowShowing && canAccessBox)
        {
            QuitPannel();
        }
    }

    public void OnShowBtn()
    {
        BeginPannel();
    }

    public void OnQuitBtn()
    {
        QuitPannel();
    }

    private void BeginPannel()
    {
        PlayMoveToTargetPos();
        
        if (spotlight.Tutorial) 
        {
            guideText.StopGuide();
            spotlight.StopSpotlight();
        }
    }

    private void QuitPannel()
    {
        cutSceneRemind.SetPropsNone();
        PlayMoveToOriginalPos();
    }

    public void PlayMoveToTargetPos()
    {
        boxUIBtn.AllPanelDeActive();
        blackBorder.HalfFadeOut();
        StartMove(targetPos);
        _active = true;
        Time.timeScale = 0f;
    }

    public void PlayMoveToOriginalPos()
    {
        blackBorder.HalfFadeIn();
        StartMove(originalPos);
        _active = false;
        Time.timeScale = 1f;
    }

    private void StartMove(RectTransform destination)
    {
        if (destination == null)
            return;

        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(CMovePanel(destination.anchoredPosition));
    }

    private IEnumerator CMovePanel(Vector2 destination)
    {
        float time = 0f;
        Vector2 startPos = rectTransform.anchoredPosition;
        _duration = true;

        while (time < moveDuration)
        {
            time += Time.unscaledDeltaTime;
            float t = time / moveDuration;
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, destination, t);
            yield return null;
        }

        rectTransform.anchoredPosition = destination;
        _duration = false;
        moveCoroutine = null;
    }

    public void SetCanAccessBox(bool value) { canAccessBox = value; }
}
