using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ScnChackManager : MonoBehaviour
{
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
    }
    // Update is called once per frame
    void Update()
    {

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
    }

    private void OnClick(bool clickside)
    {
        //Debug.Log("OnClicked Side : " + clickside);
        if (SanChackNum >= SanChackList.Count - 1)
        {
            SanchackEnd();
            return;
        }

        if (clickside == SanChackList[SanChackNum].IsRight)
        {
            GoodWay();
            emgRander.sprite = emgGood;
            StartCoroutine(OpenImg(emgRander));
        }
        else
        {
            BadWay();
            emgRander.sprite = emgFuck;
            StartCoroutine(OpenImg(emgRander));
        }

        SanChackNum++;
    }

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

        FadeManager.Instance.FadeOut();

        yield return new WaitForSeconds(1.2f);

        RelodeImg();
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
    public void SanchackEnd()
    {

    }
}