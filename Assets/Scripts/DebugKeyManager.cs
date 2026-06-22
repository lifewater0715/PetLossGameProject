using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugKeyManager : MonoBehaviour
{
    [SerializeField] private string sceneName;

    private void Awake()
    {  
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.R))
        {
            if (!SceneCheck()) 
            {
                Debug.Log("씬 없어요~");
                return;
            }

            SceneManager.LoadScene(sceneName);
        }
    }

    private bool SceneCheck()
    {
        int count = SceneManager.sceneCountInBuildSettings;

        for (int i = 0; i < count; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);

            if (name == sceneName)
                return true;
        }

        return false;
    }
}