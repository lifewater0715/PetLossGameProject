using UnityEngine;

public class TugInput : MonoBehaviour
{
    [SerializeField] private TugGaugeController tugGaugeController;
    [SerializeField] private TugSystemManager tugSystemManager;

    private void Update()
    {
        if (FadeManager.Instance != null && FadeManager.Instance.IsFading) return;
        
        if (!tugSystemManager.GameStart) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            tugGaugeController.SetDogPull(false);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            tugGaugeController.OnChargedGauge();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            tugGaugeController.SetDogPull(true);
        }
    }
}
