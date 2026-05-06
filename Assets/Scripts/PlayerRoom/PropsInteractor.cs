using UnityEngine;

public class PropsInteractor : MonoBehaviour
{
    [SerializeField] private Camera mainCam;

#if UNITY_EDITOR
    [ContextMenu("Auto Assign")]
    private void AutoAssign()
    {
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
#endif

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryInteraction();
        }
    }

    private void TryInteraction()
    {
        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector2 point = mousePos;

        Collider2D hit = Physics2D.OverlapPoint(point);
        if (hit == null) return;

        PropsInfo props = hit.GetComponent<PropsInfo>();
        if (props == null) return;

        props.Interaction();

    }
}
