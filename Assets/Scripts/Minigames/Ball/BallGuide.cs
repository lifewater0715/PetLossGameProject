using System.Collections.Generic;
using UnityEngine;

public class BallGuide : MonoBehaviour
{
    [SerializeField] private BallThrowController ballThrowController;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private Transform landingMarker;

    [Header("가이드라인 생성 빈도")]
    [SerializeField, Min(0.01f)] private float timeStep = 0.05f;
    [SerializeField, Min(0.1f)] private float maxSimulationTime = 5f;

    [Header("바닥 레이어 설정")]
    [SerializeField] private LayerMask groundLayer;

    private readonly List<Vector3> points = new();

#if UNITY_EDITOR
    [ContextMenu("Auto Assign")]
    private void AutoAssign()
    {
        ballThrowController = GameObject.Find("BallThrowManager").GetComponent<BallThrowController>();
        throwPoint = GameObject.Find("ThrowPoint").GetComponent<Transform>();
        lineRenderer = GameObject.Find("LineGuide").GetComponent<LineRenderer>();
        landingMarker = GameObject.Find("Marker").GetComponent<Transform>();

        groundLayer = LayerMask.GetMask("Ground");
    }
#endif

    private void Awake()
    {
        if (lineRenderer != null)
            lineRenderer.enabled = false;
        
        if (landingMarker != null)
            landingMarker.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (ballThrowController == null) return;

        ballThrowController.OnChargeStarted += ShowGuide;
        ballThrowController.OnThrow += HideGuide;
        ballThrowController.OnPowerChanged += DrawGuide;
    }

    private void OnDisable()
    {
        if (ballThrowController == null) return;
        
        ballThrowController.OnChargeStarted -= ShowGuide;
        ballThrowController.OnThrow -= HideGuide;
        ballThrowController.OnPowerChanged -= DrawGuide;
    }

    private void ShowGuide()
    {
        lineRenderer.enabled = true;
        landingMarker.gameObject.SetActive(true);
    }

    private void HideGuide()
    {
        lineRenderer.enabled = false;
        landingMarker.gameObject.SetActive(false);
    }

    private void DrawGuide(float power)
    {
        points.Clear();

        Vector2 startPos = throwPoint.position;
        Vector2 velocity = ballThrowController.GetThrowDirection.normalized * power;

        points.Add(startPos);

        Vector2 previousPoint = startPos;
        Vector2 lastPoint = startPos;

        bool hitGround = false;

        for (float time = timeStep; time <= maxSimulationTime; time += timeStep)
        {
            Vector2 nextPoint = startPos + velocity * time + 0.5f * Physics2D.gravity * time * time;

            RaycastHit2D hit = Physics2D.Linecast(previousPoint, nextPoint, groundLayer);

            if (hit.collider != null)
            {
                points.Add(hit.point);
                lastPoint = hit.point;
                hitGround = true;
                break;
            }
            
            points.Add(nextPoint);
            previousPoint = nextPoint;
            lastPoint = nextPoint;
        }

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());

        UpdateLandingMarker(lastPoint, hitGround);
    }

    private void UpdateLandingMarker(Vector2 landingPoint, bool hitGround)
    {
        if (landingMarker == null) return;

        landingMarker.gameObject.SetActive(hitGround);
        landingMarker.position = landingPoint;
    }
}
