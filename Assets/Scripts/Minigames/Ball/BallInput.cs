using System;
using UnityEngine;

public class BallInput : MonoBehaviour
{
    [Header("장난감 공 충전")]
    [SerializeField] private float minPower = 5f;
    [SerializeField] private float maxPower = 15f;
    [SerializeField] private float chargeSpeed = 8f;

    private float _currentPower;
    private int _chargeDirection = 1;

    public event Action OnChargeStarted;
    public event Action<float> OnPowerChanged;
    public event Action<float> OnThrow;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _currentPower = minPower;
            _chargeDirection = 1;
            OnPowerChanged?.Invoke(_currentPower);
            OnChargeStarted?.Invoke();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            ChargePower();
            OnPowerChanged?.Invoke(_currentPower);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            OnThrow?.Invoke(_currentPower);
        }
    }

    private void ChargePower()
    {
        _currentPower += chargeSpeed * _chargeDirection * Time.deltaTime;

        if (_currentPower >= maxPower)
        {
            _currentPower = maxPower;
            _chargeDirection = -1;
        }
        else if (_currentPower <= minPower)
        {
            _currentPower = minPower;
            _chargeDirection = 1;
        }
    }
}
