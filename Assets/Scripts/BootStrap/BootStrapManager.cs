using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BootStrapManager : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "TitleScreen";
    [SerializeField] private Image targetImage;
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private Sprite[] loadingAnim;

    private Coroutine LoadingAnim;

    private void Start()
    {
        StartCoroutine(StartBootStrap());
        //StartCoroutine(StartLoadingText());
        // LoadingAnim = StartCoroutine(StartLoadingAnim());
    }

    private IEnumerator StartBootStrap()
    {
        yield return new WaitForSeconds(0.5f);

        bool success = BootStrap();

        if (!success) 
        {
            Debug.Log("에디터 전용: BootStrap 검사에 실패했습니다. 해당 함수 확인 바람.");
            Application.Quit();
            yield break;
        }

        Debug.Log("부팅 성공");

        if (LoadingAnim != null)
        {
            StopCoroutine(LoadingAnim);
            LoadingAnim = null;
        }

        DeActiveLoadingObj();
        yield return new WaitForSeconds(0.2f);
        //페이드 아웃 연출은 어색해서 비동기 로드로 대체.
        SceneManager.LoadScene(nextSceneName);
    }

    private bool BootStrap()
    {
        if (FadeManager.Instance == null) 
        {
            Debug.Log("FadeManager 누락 감지.");
            return false;
        }

        if (SceneLoadManager.Instance == null) 
        {
            Debug.Log("SceneLoadManager 누락 감지.");
            return false;
        }

        ResolutionLock resolutionLock = FindFirstObjectByType<ResolutionLock>();
        if (resolutionLock == null) 
        {
            Debug.Log("ResolutionLock 누락 감지.");
            return false;
        }

        //성공적으로 마치셨어요!
        return true;
    }

// -------------------연출용------------------- //
    private IEnumerator StartLoadingAnim()
    {
        int index = 0;

        while (true)
        {
            targetImage.sprite = loadingAnim[index];
            yield return new WaitForSeconds(0.1f);

            index++;
            if (index > loadingAnim.Length - 1) index = 0;
        }
    }

    private IEnumerator StartLoadingText()
    {
        while (true)
        {
            loadingText.text = "로딩중..";
            yield return new WaitForSeconds(0.2f);
            loadingText.text = "로딩중...";
            yield return new WaitForSeconds(0.2f);
            loadingText.text = "로딩중....";
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void DeActiveLoadingObj()
    {
        targetImage.gameObject.SetActive(false);
        loadingText.gameObject.SetActive(false);
    }
}
