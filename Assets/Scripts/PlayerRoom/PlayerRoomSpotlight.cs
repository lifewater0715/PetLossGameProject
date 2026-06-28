using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRoomSpotlight : MonoBehaviour
{
    [SerializeField] private Image overlayImage;
    [SerializeField] private GameObject gaurdImage;
    [SerializeField] private BoxPanelAnim boxPanelAnim;
    [SerializeField] private PlayerRoomTutorialUIGuideText guideText;

    [SerializeField] private float radius = 1f;
    [SerializeField] private float feather = 0.05f;

    private Material _material;

    private static readonly int RadiusId = Shader.PropertyToID("_Radius");
    private static readonly int FeatherId = Shader.PropertyToID("_Feather");

    private Coroutine _spotlightRoutine;

    private bool tutorial = false;
    public bool Tutorial => tutorial;

    private void Awake()
    {
        _material = Instantiate(overlayImage.material);
        overlayImage.material = _material;
        overlayImage.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        _material.SetFloat(RadiusId, radius);
        _material.SetFloat(FeatherId, feather);
    }

    public void PlaySpotlight()
    {
        if (_spotlightRoutine != null)
            StopCoroutine(_spotlightRoutine);

        overlayImage.gameObject.SetActive(true);
        tutorial = true;
        _spotlightRoutine = StartCoroutine(SpotlightRoutine(1f, 0.15f, 1f));
    }

    public void StopSpotlight()
    {
        tutorial = false;
        overlayImage.gameObject.SetActive(false);
    }

    private IEnumerator SpotlightRoutine(float startRadius, float endRadius, float duration)
    {
        yield return new WaitForSeconds(2f);

        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            float t = time / duration;
            t = Mathf.SmoothStep(0f, 1f, t);

            radius = Mathf.Lerp(startRadius, endRadius, t);

            yield return null;
        }

        radius = endRadius;
        gaurdImage.SetActive(false);
        boxPanelAnim.SetCanAccessBox(true);
        guideText.StartGuide();
    }
}
