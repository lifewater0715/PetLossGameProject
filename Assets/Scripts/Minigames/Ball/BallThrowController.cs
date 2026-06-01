using System;
using UnityEngine;

public class BallThrowController : MonoBehaviour
{
    [Header("장난감 공 충전")]
    [SerializeField] private float minPower = 5f;
    [SerializeField] private float maxPower = 15f;
    [SerializeField] private float chargeSpeed = 8f;

    [SerializeField] private Vector2 throwDirection = new Vector2(1f, 1f);
    public Vector2 GetThrowDirection => throwDirection;

    private float _currentPower;
    private int _chargeDirection = 1;

    private bool canThrow = true;

    public event Action OnChargeStarted;
    public event Action<float> OnPowerChanged;
    public event Action OnThrow;
    public event Action<float> OnThrowWithPower;

    public void StartBallThrow()
    {
        if (!canThrow) return;

        _currentPower = minPower;
        _chargeDirection = 1;
        OnPowerChanged?.Invoke(_currentPower);
        OnChargeStarted?.Invoke();
    }

    public void HoldBallThrow()
    {
        if (!canThrow) return;

        ChargePower();
        OnPowerChanged?.Invoke(_currentPower);
    }

    public void FinishBallThrow()
    {
        if (!canThrow) return;

        OnThrowWithPower?.Invoke(_currentPower);
        OnThrow?.Invoke();
    }

    public void SetCanThrow(bool value)
    {
        canThrow = value;
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
