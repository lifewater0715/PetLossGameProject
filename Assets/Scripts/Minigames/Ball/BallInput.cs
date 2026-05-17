using System;
using UnityEngine;

public class BallInput : MonoBehaviour
{
    [SerializeField] private float minPower = 0f;
    [SerializeField] private float maxPower = 15f;
    [SerializeField] private float chargeSpeed = 8f;

    private float _currentPower;
    private int _chargeDirection = 1;

    public event Action onChargeStarted;
    public event Action<float> onPowerChanged;
    public event Action<float> onThrow;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _currentPower = minPower;
            _chargeDirection = 1;
            onPowerChanged?.Invoke(_currentPower);
            onChargeStarted?.Invoke();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            ChargePower();
            onPowerChanged?.Invoke(_currentPower);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            onThrow?.Invoke(_currentPower);
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
