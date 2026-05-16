using UnityEngine;

public class PropsInteractor : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private PropsDirector propsDirector;

#if UNITY_EDITOR
    [ContextMenu("Auto Assign")]
    private void AutoAssign()
    {
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        propsDirector = GameObject.Find("PropsDirector").GetComponent<PropsDirector>();
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
        if (mainCam == null) return;
        
        if (propsDirector == null) return;

        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector2 point = mousePos;

        Collider2D hit = Physics2D.OverlapPoint(point);
        if (hit == null) return;

        PropsInfo props = hit.GetComponent<PropsInfo>();
        if (props == null) return;

        propsDirector.TryPlayEvent(props);
    }
}
