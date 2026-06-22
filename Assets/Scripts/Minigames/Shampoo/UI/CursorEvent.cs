using System;
using UnityEngine;

public class CursorEvent : MonoBehaviour
{
    [SerializeField] private Collider2D dogCollider;
    [SerializeField] private float minMoveDistance = 0.15f;

    public event Action OnRubbed;
    public event Action<float> OnRubbedDistance;
    public event Action OnRubbedStop;
    public event Action OnHeldOnDog;
    public event Action OnHeldOnDogStop;

    private Camera _cam;
    private Vector2 _prevMouseWorldPos;
    private bool _wasHeldOnDog;

    private void Awake()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _prevMouseWorldPos = GetMouseWorldPos();
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnRubbedStop?.Invoke();

            if (_wasHeldOnDog)
            {
                OnHeldOnDogStop?.Invoke();
                _wasHeldOnDog = false;
            }
        }

        if (!Input.GetMouseButton(0)) return;

        Vector2 currentMouseWorldPos = GetMouseWorldPos();
        bool isMouseOnDog = IsMouseOnDog(currentMouseWorldPos);

        if (isMouseOnDog)
        {
            OnHeldOnDog?.Invoke();
            _wasHeldOnDog = true;
        }
        else if (_wasHeldOnDog)
        {
            OnHeldOnDogStop?.Invoke();
            _wasHeldOnDog = false;
        }

        if (!isMouseOnDog)
        {
            _prevMouseWorldPos = currentMouseWorldPos;
            return;
        }

        float moveDistance = Vector2.Distance(_prevMouseWorldPos, currentMouseWorldPos);

        if (moveDistance >= minMoveDistance)
        {
            OnRubbed?.Invoke();
            OnRubbedDistance?.Invoke(moveDistance);

            _prevMouseWorldPos = currentMouseWorldPos;
        }
    }

    private Vector2 GetMouseWorldPos()
    {
        return _cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private bool IsMouseOnDog(Vector2 pos)
    {
        return dogCollider.OverlapPoint(pos);
    }
}
