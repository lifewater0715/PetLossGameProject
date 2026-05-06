using UnityEngine;

public class PropsInfo : MonoBehaviour
{
    [SerializeField] private int requiredTurn;
    [SerializeField] private PropsDirector propsDirector;

    private IPropsEvent propsEvent;

    public int GetRequiredTurn => requiredTurn;
    public IPropsEvent GetPropsEvent => propsEvent;

#if UNITY_EDITOR
    [ContextMenu("Auto Assign")]
    private void AutoAssign()
    {
        propsDirector = GameObject.Find("PropsDirector").GetComponent<PropsDirector>();
    }
#endif

    private void Awake()
    {
        propsEvent = GetComponent<IPropsEvent>();
    }

    public void Interaction()
    {
        propsDirector.PlayEvent(this);
    }
}
