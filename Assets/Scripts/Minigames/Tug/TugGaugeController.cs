using UnityEngine;

public class TugGaugeController : MonoBehaviour
{
    [SerializeField] private float dogPower = 1f;
    [SerializeField] private float chargeSpeed = 1f;

    private float _gauge = 50f;
    private bool _dogPull = true;

    private void Update()
    {
        OnDogPull();
    }

    private void OnDogPull()
    {
        if (!_dogPull) return;

        if (_gauge == 0 ) return;

        _gauge -= Time.deltaTime * dogPower;
        Debug.Log(_gauge);
        if (_gauge < 0) _gauge = 0f;
    }

    public void OnChargedGauge()
    {
        if (_gauge == 100f) return;

        _gauge += Time.deltaTime * chargeSpeed;
        Debug.Log(_gauge);
        if (_gauge > 100f) _gauge = 100f;
    }

    public void SetDogPull(bool value)
    {
        _dogPull = value;
    }
}
