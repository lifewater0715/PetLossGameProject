using UnityEngine;

public class PropsInfo : MonoBehaviour
{
    [SerializeField] private int requiredTurn;

    private IPropsEvent propsEvent;

    private void Awake()
    {
        propsEvent = GetComponent<IPropsEvent>();
    }

    public bool CanInteract(int currentTurn)
    {
        return requiredTurn == currentTurn;
    }

    public bool TryPlayEvent()
    {
        if (propsEvent == null) return false;

        propsEvent.Play();
        return true;
    }
}
