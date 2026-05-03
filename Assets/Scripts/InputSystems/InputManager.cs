using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public event Action onLeftClick;
    public event Action onKeyInputF;
    
    private Vector2 _mouseVec;

    public Vector2 mouseVec => _mouseVec;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if(context.performed) onLeftClick?.Invoke();
    }

    public void OnPointerPosition(InputAction.CallbackContext context)
    {
        _mouseVec = context.ReadValue<Vector2>();
    }

    public void OnKeyInputF(InputAction.CallbackContext context)
    {
        if(context.performed) onKeyInputF?.Invoke();
    }
}