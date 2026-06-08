using UnityEngine;

public class IntroSystemManager : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "PlayerRoom";

    public void NextPage()
    {
        
    }

    public void MoveNextScene()
    {
        SceneLoadManager.Instance.LoadScene(nextSceneName);
    }
}
