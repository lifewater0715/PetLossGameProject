using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ScnChackManager : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "PlayerRoom";
    [SerializeField] private CutSceneManager cutSceneManager;

    [Header("산책정보")]
    [SerializeField] private List<SanChackData> SanChackList = new List<SanChackData>();
    [SerializeField] private int SanChackNum = 0;
    [Header("길")]
    [SerializeField] private SpriteRenderer lRander;
    [SerializeField] private SpriteRenderer rRander;
    [Header("댕댕이 이미지")]
    [SerializeField] private SpriteRenderer dogRander;
    [SerializeField] private Animator dogAni;
    [Header("클릭방지용 켄버스")]
    [SerializeField] private GameObject Cover;
    [Header("이모지관련")]
    [SerializeField] private SpriteRenderer emgRander;
    [SerializeField] private Sprite emgGood;
    [SerializeField] private Sprite emgFuck;
    [Header("시간 딜레이")]
    [SerializeField] private float FuckWayTime;

    //폴리싱 작업을 위한 추가 (by Byealha)
    [Header("폴리싱 및 버그 완화 작업")]
    [SerializeField] private GameObject dontClickArea;
    [SerializeField] private LeashUIGuideText uiGuideText;
    private Coroutine cutSceneCoroutine;

    [System.Serializable]
    public struct SanChackData
    {
        public Sprite Limg;
        public Sprite Rimg;
        public bool IsRight;
    }

    void Awake()
    {
        RelodeImg();
        BGMManager.Instance.SetFilterMode(BGMManager.AudioLevel.None);
    }
    // Update is called once per frame
    void Update()
    {

    }

    private void OnDisable()
    {
        StopCoroutine(cutSceneCoroutine);
        cutSceneCoroutine = null;
    }

    private void RelodeImg()
    {
        lRander.sprite = SanChackList[SanChackNum].Limg;
        rRander.sprite = SanChackList[SanChackNum].Rimg;

        if (!SanChackList[SanChackNum].IsRight)
        {
            dogRander.flipX = true;
        }
        else if (SanChackList[SanChackNum].IsRight)
        {
            dogRander.flipX = false;
        }

        FadeManager.Instance.FadeIn();
        dontClickArea.SetActive(false);
    }

    private void OnClick(bool clickside)
    {
        if (FadeManager.Instance != null && FadeManager.Instance.IsFading) return;
        //Debug.Log("OnClicked Side : " + clickside);

        if (cutSceneCoroutine != null) return;

        if (SanChackNum >= SanChackList.Count - 1)
        {
            if (clickside == SanChackList[SanChackNum].IsRight)
            {
                cutSceneCoroutine = StartCoroutine(ShowCutScene());
                return;
            }
            else
            {
                BadChoice();
                return;
            }
        }

        if (clickside == SanChackList[SanChackNum].IsRight)
        {
            GoodChoice();
        }
        else
        {
            BadChoice();
        }
    }

    private void GoodChoice()
    {
        GoodWay();
        emgRander.sprite = emgGood;
        dontClickArea.SetActive(true);
        uiGuideText.StopGuide();
        StartCoroutine(SmoothShowImotion(emgRander));
        SanChackNum++;
    }

    private void BadChoice()
    {
        BadWay();
        emgRander.sprite = emgFuck;
        dontClickArea.SetActive(true);
        uiGuideText.StopGuide();
        StartCoroutine(SmoothShowImotion(emgRander));
    }

    //말풍선 폴리싱 (by Byealha)
    private IEnumerator SmoothShowImotion(SpriteRenderer target)
    {
        target.color = new Color(1f, 1f, 1f, 0f);
        float alpha = 0f;
        target.gameObject.SetActive(true);

        while (alpha < 1f)
        {
            alpha += 0.05f;
            yield return new WaitForSeconds(0.01f);
            target.color = new Color(1f, 1f, 1f, alpha);
        }

        yield return new WaitForSeconds(1f);

        while (alpha > 0f)
        {
            alpha -= 0.05f;
            yield return new WaitForSeconds(0.01f);
            target.color = new Color(1f, 1f, 1f, alpha);
        }
        target.gameObject.SetActive(false);
    }

    //기존 것
    private IEnumerator OpenImg(SpriteRenderer emg)
    {
        emg.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        emg.gameObject.SetActive(false);

    }

    private IEnumerator NotMyWay()
    {
        dogAni.SetBool("IsNotWay", true);

        yield return new WaitForSeconds(1.5f);

        dogAni.SetBool("IsNotWay", false);

        dontClickArea.SetActive(false);

        /*FadeManager.Instance.FadeOut();

        yield return new WaitForSeconds(1.2f);

        RelodeImg();*/
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.5f);

        FadeManager.Instance.FadeOut();

        yield return new WaitForSeconds(1.2f);

        RelodeImg();
    }

    private void GoodWay()
    {
        StartCoroutine(Timer());

        Debug.Log("마루쫑긋");
    }

    private void BadWay()
    {
        StartCoroutine(NotMyWay());

        Debug.Log("마루씨발");
    }

    public void LeftClick()
    {
        OnClick(false);
    }

    public void RightClick()
    {
        OnClick(true);
    }

    //스테이지 클리어
    private IEnumerator ShowCutScene()
    {
        yield return StartCoroutine(cutSceneManager.StartCutScene());
        SceneLoadManager.Instance.LoadScene(nextSceneName);
    }
}