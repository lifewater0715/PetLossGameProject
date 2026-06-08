using UnityEngine;

public class TugSystemManager : MonoBehaviour
{
    [SerializeField] private TugGaugeController tugGaugeController;
    [SerializeField] private float minGauge = 40f;
    [SerializeField] private float maxGauge = 60f;
    [SerializeField] private float scoreIncreaseSpeed = 10f;
    [SerializeField] private float clearScore = 100f;

    private float score;
    private bool isCleared;

    [SerializeField] private string nextSceneName = "PlayerRoom";

    private void Update()
    {
        if (isCleared) return;
        if (tugGaugeController == null) return;

        float gauge = tugGaugeController.Gauge;

        if (gauge < minGauge) return;
        if (gauge > maxGauge) return;

        score += Time.deltaTime * scoreIncreaseSpeed;

        if (score >= clearScore)
        {
            isCleared = true;
            Debug.Log("-------------------Tug minigame clear-------------------");
            SceneLoadManager.Instance.LoadScene(nextSceneName);
        }
    }
}
