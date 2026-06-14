using System.Collections;
using UnityEngine;

public class TugSystemManager : MonoBehaviour
{
    [SerializeField] private TugGaugeController tugGaugeController;
    [SerializeField] private float minGauge = 40f;
    [SerializeField] private float maxGauge = 60f;
    [SerializeField] private float scoreIncreaseSpeed = 10f;
    [SerializeField] private float clearScore = 100f;
    [SerializeField] private CutSceneManager cutSceneManager;

    private bool gameStart = false;
    public bool GameStart => gameStart;

    private float score;
    private bool isCleared;

    [SerializeField] private string nextSceneName = "PlayerRoom";

    private void Start()
    {
        StartCoroutine(CGameStart());
    }

    private IEnumerator CGameStart()
    {
        yield return new WaitForSeconds(1f);
        gameStart = true;
    }

    private void Update()
    {
        if (isCleared) return;
        if (tugGaugeController == null) return;
        if (!tugGaugeController.FirstPlayerPull) return;

        float gauge = tugGaugeController.Gauge;

        if (gauge < minGauge) return;
        if (gauge > maxGauge) return;

        score += Time.deltaTime * scoreIncreaseSpeed;

        if (score >= clearScore)
        {
            isCleared = true;
            gameStart = false;
            Debug.Log("-------------------Tug minigame clear-------------------");
            StartCoroutine(ShowCutScene());
        }
    }

    private IEnumerator ShowCutScene()
    {
        yield return StartCoroutine(cutSceneManager.StartCutScene());
        SceneLoadManager.Instance.LoadScene(nextSceneName);
    }
}
