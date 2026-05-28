using UnityEngine;

public class TugInput : MonoBehaviour
{
    [SerializeField] private TugGaugeController tugGaugeController;

    private void Update()
    {
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
