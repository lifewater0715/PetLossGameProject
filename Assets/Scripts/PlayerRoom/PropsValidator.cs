using UnityEngine;

public class PropsValidator : MonoBehaviour
{
    [SerializeField] private LayerMask propsLayer;

#if UNITY_EDITOR
    [ContextMenu("Auto Assign")]
    private void AutoAssign()
    {
        propsLayer = LayerMask.GetMask("Props");
    }
#endif

    public bool GetTargetLayer(Vector2 position)
    {
        Collider2D hit = Physics2D.OverlapPoint(position, propsLayer);
        return hit != null;
    }
}
