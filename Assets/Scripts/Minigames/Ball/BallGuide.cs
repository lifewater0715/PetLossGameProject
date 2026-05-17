using UnityEngine;

public class BallGuide : MonoBehaviour
{
    [SerializeField] private BallInput ballInput;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private int pointCount = 30;
    [SerializeField] private float timeStep = 0.05f;

    private Vector2 _throwDirection = new Vector2(1f, 1f);

    private void Awake()
    {
        lineRenderer.positionCount = pointCount;
        lineRenderer.enabled = false;
    }

    private void OnEnable()
    {
        ballInput.onChargeStarted += ShowGuide;
        ballInput.onThrow += HideGuide;
        ballInput.onPowerChanged += DrawGuide;
    }

    private void OnDisable()
    {
        ballInput.onChargeStarted -= ShowGuide;
        ballInput.onThrow -= HideGuide;
        ballInput.onPowerChanged -= DrawGuide;
    }

    private void ShowGuide()
    {
        lineRenderer.enabled = true;
    }

    private void HideGuide(float power)
    {
        lineRenderer.enabled = false;
    }

    private void DrawGuide(float power)
    {
        Vector2 startPos = throwPoint.position;
        Vector2 velocity = _throwDirection.normalized * power;

        for (int i = 0; i < pointCount; i++)
        {
            float time = i * timeStep;

            Vector2 point = startPos + velocity * time + 0.5f * Physics2D.gravity * time * time;

            lineRenderer.SetPosition(i, point);
        }
    }
}
