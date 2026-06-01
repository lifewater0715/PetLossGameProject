using System;
using UnityEngine;

public class CursorEvent : MonoBehaviour
{
    [SerializeField] private Collider2D dogCollider;
    [SerializeField] private float minMoveDistance = 0.15f;

    public event Action OnRubbed;

    private Camera _cam;
    private Vector2 _prevMouseWorldPos;

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

        if (!Input.GetMouseButton(0)) return;

        Vector2 currentMouseWorldPos = GetMouseWorldPos();

        if (!IsMouseOnDog(currentMouseWorldPos))
        {
            _prevMouseWorldPos = currentMouseWorldPos;
            return;
        }

        float moveDistance = Vector2.Distance(_prevMouseWorldPos, currentMouseWorldPos);

        if (moveDistance >= minMoveDistance)
        {
            OnRubbed?.Invoke();

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