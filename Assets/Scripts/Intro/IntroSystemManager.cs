using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IntroSystemManager : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "PlayerRoom";
    [SerializeField] private Book book;
    [SerializeField] private int lastPage = 7;
    [SerializeField] private Image nextSceneBtn;
    [SerializeField] private GameObject rightHotSpot;
    [SerializeField] private IntroUIGuideText introUIGuideText;
    [SerializeField] private float showBtnDelay = 5f;

    private bool _lastPageVisit = false;
    private float _guideTimer;
    private bool _guideAlertNow = false;
    private float _guideTimerMax = 5f;

    private void Awake()
    {
        nextSceneBtn.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_guideTimer <= _guideTimerMax && !_guideAlertNow && book.currentPage != book.TotalPageCount - 1)
            _guideTimer += Time.deltaTime;
            
        if (_guideTimer > _guideTimerMax && !_guideAlertNow)
        {
            _guideAlertNow = true;
            introUIGuideText.StartGuide();
        }

        if (book.currentPage == lastPage && !_lastPageVisit)
            MoveNextSceneReady();

        if (book.currentPage == lastPage && rightHotSpot.activeSelf) 
            rightHotSpot.SetActive(false);
        else if (book.currentPage != lastPage && !rightHotSpot.activeSelf) 
            rightHotSpot.SetActive(true);
    }

    private void OnEnable()
    {
        book.OnFlip.AddListener(OnPageFlipped);
    }

    private void OnDisable()
    {
        book.OnFlip.RemoveListener(OnPageFlipped);
    }

    private void OnPageFlipped()
    {
        _guideTimer = 0f;
        _guideTimerMax = 13f;
        _guideAlertNow = false;
        introUIGuideText.StopGuide();
    }

    private void MoveNextSceneReady()
    {
        _lastPageVisit = true;
        StartCoroutine(ShowNextSceneBtn());
    }

    public void MoveNextScene()
    {
        BGMManager.Instance.StopSound();
        SceneLoadManager.Instance.LoadScene(nextSceneName);
    }

    private IEnumerator ShowNextSceneBtn()
    {
        yield return new WaitForSeconds(showBtnDelay);
        nextSceneBtn.color = new Color(1f, 1f, 1f, 0f);
        nextSceneBtn.gameObject.SetActive(true);
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += 0.05f;
            yield return new WaitForSecondsRealtime(0.01f);
            nextSceneBtn.color = new Color(1f, 1f, 1f, alpha);
        }
    }
}
