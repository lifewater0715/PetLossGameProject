using UnityEngine;

public class TitleBtnManager : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "IntroScene";

    public void GameStartBtn()
    {
        SceneLoadManager.Instance.LoadScene(nextSceneName);
    }

    public void GameQuitBtn()
    {
        Application.Quit();
    }

}
