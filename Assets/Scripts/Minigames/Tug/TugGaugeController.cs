using UnityEngine;

public class TugGaugeController : MonoBehaviour
{
    [SerializeField] private float dogPower = 6f;
    [SerializeField] private float chargeSpeed = 4f;
    [SerializeField] private TugSystemManager tugSystemManager;

    private float _gauge = 50f;
    private bool _dogPull = true;

    public float Gauge => _gauge;
    public float NormalizedGauge => _gauge / 100f;
    public bool IsDogPulling => _dogPull;
    public bool IsPlayerPulling => !_dogPull;

    private void Update()
    {
        if (!tugSystemManager.GameStart) return;

        OnDogPull();
    }

    private void OnDogPull()
    {
        if (!_dogPull) return;

        if (_gauge == 0 ) return;

        _gauge += Time.deltaTime * dogPower;
        Debug.Log(_gauge);
        if (_gauge > 100f) _gauge = 90f;
    }

    public void OnChargedGauge()
    {
        if (_gauge == 100f) return;

        _gauge -= Time.deltaTime * chargeSpeed;
        Debug.Log(_gauge);
        if (_gauge < 0f) _gauge = 10f;
    }

    public void SetDogPull(bool value)
    {
        _dogPull = value;
    }
}
