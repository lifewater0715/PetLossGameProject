using System.Collections;
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
    private Camera _cam;
    private Coroutine _sizeCoroutine;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
    }

    private void LateUpdate() 
    {
        CamTargetTrack();
    }

    private void CamTargetTrack()
    {
        if (target == null) return;

        Vector3 targetPos = target.position;
        float cameraHalfHeight = 0f;
        float cameraHalfWidth = 0f;

        if (_cam != null && _cam.orthographic)
        {
            cameraHalfHeight = _cam.orthographicSize;
            cameraHalfWidth = cameraHalfHeight * _cam.aspect;
        }

        if (yMinEnabled && yMaxEnabled)
        {
            float minY = yMinValue + cameraHalfHeight;
            float maxY = yMaxValue - cameraHalfHeight;
            targetPos.y = minY <= maxY ? Mathf.Clamp(target.position.y, minY, maxY) : (yMinValue + yMaxValue) * 0.5f;
        }
        else if (yMinEnabled)
            targetPos.y = Mathf.Clamp(target.position.y, yMinValue + cameraHalfHeight, target.position.y);
        else if (yMaxEnabled)
            targetPos.y = Mathf.Clamp(target.position.y, target.position.y, yMaxValue - cameraHalfHeight);

        if (xMinEnabled && xMaxEnabled)
        {
            float minX = xMinValue + cameraHalfWidth;
            float maxX = xMaxValue - cameraHalfWidth;
            targetPos.x = minX <= maxX ? Mathf.Clamp(target.position.x, minX, maxX) : (xMinValue + xMaxValue) * 0.5f;
        }
        else if (xMinEnabled)
            targetPos.x = Mathf.Clamp(target.position.x, xMinValue + cameraHalfWidth, target.position.x);
        else if (xMaxEnabled)
            targetPos.x = Mathf.Clamp(target.position.x, target.position.x, xMaxValue - cameraHalfWidth);

        targetPos.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _velocity, smoothTime);
    }

    public void SetCamTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetCamSmoothTime(float value)
    {
        smoothTime = value;
    }

    public void OnCamSmoothTarget(Transform targetObj, float smoothTime)
    {
        StartCoroutine(COnCamSmoothTarget(targetObj, smoothTime));
    }

    private IEnumerator COnCamSmoothTarget(Transform targetObj, float smoothTime)
    {
        float smoothTimeDelay = 0.01f;

        SetCamSmoothTime(smoothTime);
        SetCamTarget(targetObj);

        while (smoothTime > 0)
        {
            smoothTime -= smoothTimeDelay;
            yield return new WaitForSeconds(smoothTimeDelay);
            SetCamSmoothTime(smoothTime);
        }
    }

    public void OnCamSmoothSize(float targetSize)
    {
        if (_sizeCoroutine != null)
            StopCoroutine(_sizeCoroutine);

        _sizeCoroutine = StartCoroutine(COnCamSmoothSize(targetSize));
    }

    private IEnumerator COnCamSmoothSize(float targetSize)
    {
        float smoothTimeDelay = 0.05f;
        float currentSize = _cam.orthographicSize;

        if (currentSize < targetSize)
        {
            while (currentSize <= targetSize)
            {
                currentSize += smoothTimeDelay;
                yield return new WaitForSeconds(0.02f);
                _cam.orthographicSize = currentSize;
            }
        }
        else if (currentSize > targetSize)
        {
            while (currentSize >= targetSize)
            {
                currentSize -= smoothTimeDelay;
                yield return new WaitForSeconds(0.02f);
                _cam.orthographicSize = currentSize;
            }
        }
        else
        {
            _sizeCoroutine = null;
            yield break;
        }

        _cam.orthographicSize = targetSize;
        _sizeCoroutine = null;
    }
}
