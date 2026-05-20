using UnityEngine;

public class CamFollowTarget : MonoBehaviour
{
    [Header("추적 대상")]
    [SerializeField] private Transform target;

    [Header("추적 속도")]
    [SerializeField] private float smoothTime = 0.15f;

    [Header("카메라 좌표 최대 / 최솟값")]
    [SerializeField] private float xMaxValue = 0;
    [SerializeField] private float xMinValue = 0;
    [SerializeField] private float yMaxValue = 0;
    [SerializeField] private float yMinValue = 0;

    [Header("최대 / 최솟값 적용 여부")]
    [SerializeField] private bool xMaxEnabled = false;
    [SerializeField] private bool xMinEnabled = false;
    [SerializeField] private bool yMaxEnabled = false;
    [SerializeField] private bool yMinEnabled = false;

    private Vector3 _velocity = Vector3.zero;

    private void LateUpdate() 
    {
        CamTargetTrack();
    }

    private void CamTargetTrack()
    {
        if (target == null) return;

        Vector3 targetPos = target.position;

        if (yMinEnabled && yMaxEnabled)
            targetPos.y = Mathf.Clamp(target.position.y, yMinValue, yMaxValue);
        else if (yMinEnabled)
            targetPos.y = Mathf.Clamp(target.position.y, yMinValue, target.position.y);
        else if (yMaxEnabled)
            targetPos.y = Mathf.Clamp(target.position.y, target.position.y, yMaxValue);

        if (xMinEnabled && xMaxEnabled)
            targetPos.x= Mathf.Clamp(target.position.x, xMinValue, xMaxValue);
        else if (xMinEnabled)
            targetPos.x = Mathf.Clamp(target.position.x, xMinValue, target.position.x);
        else if (xMaxEnabled)
            targetPos.x = Mathf.Clamp(target.position.x, target.position.x, xMaxValue);

        targetPos.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _velocity, smoothTime);
    }

    public void SetCamTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
