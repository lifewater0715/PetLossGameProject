using UnityEngine;

public class PropsInteractor : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private PropsValidator propsValidator;

#if UNITY_EDITOR
    [ContextMenu("Auto Assign")]
    private void AutoAssign()
    {
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        propsValidator = GetComponent<PropsValidator>();
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

        if (!propsValidator.GetTargetLayer(point)) return;

        Debug.Log("입력 감지 성공!");
    }
}
