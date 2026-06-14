using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IntroSystemManager : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "PlayerRoom";
    [SerializeField] private Book book;
    [SerializeField] private int lastPage = 7;
    [SerializeField] private Image NextSceneBtn;

    private bool _lastPageVisit = false;

    private void Awake()
    {
        NextSceneBtn.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (book.currentPage == lastPage && !_lastPageVisit)
            MoveNextSceneReady();
    }

    private void MoveNextSceneReady()
    {
        _lastPageVisit = true;
        StartCoroutine(ShowNextSceneBtn());
    }

    public void MoveNextScene()
    {
        SceneLoadManager.Instance.LoadScene(nextSceneName);
    }

    private IEnumerator ShowNextSceneBtn()
    {
        NextSceneBtn.color = new Color(1f, 1f, 1f, 0f);
        NextSceneBtn.gameObject.SetActive(true);
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += 0.05f;
            yield return new WaitForSecondsRealtime(0.01f);
            NextSceneBtn.color = new Color(1f, 1f, 1f, alpha);
        }
    }
}
