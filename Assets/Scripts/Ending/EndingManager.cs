using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{
    [SerializeField] private Image whiteBorder;
    [SerializeField] private EndingDogFade endingDogFade;
    [SerializeField] private Image titleLogo;
    [SerializeField] private TMP_Text thankMsg;

    private bool _finish = false;

    private void Start()
    {
        BGMManager.Instance.SetFilterMode(BGMManager.AudioLevel.Middle);
        DefaultSetting();
        StartCoroutine(EndingScript());
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && _finish)
        {
            _finish = false;
            PropsTurn.Reset();
            BGMManager.Instance.StopSound();
            BGMManager.Instance.SetFilterMode(BGMManager.AudioLevel.None);
            SceneLoadManager.Instance.LoadScene("TitleScreen");
        }
    }

    private void DefaultSetting()
    {
        whiteBorder.gameObject.SetActive(false);
        titleLogo.gameObject.SetActive(false);
        thankMsg.gameObject.SetActive(true);
        thankMsg.color = new Color(1f, 0.5f, 0f, 0f);
    }

    private IEnumerator EndingScript()
    {
        yield return new WaitForSeconds(2f);
        endingDogFade.ShowDog();

        yield return new WaitForSeconds(5f);
        yield return StartCoroutine(ShowSmoothImage(0.93f, whiteBorder));

        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(ShowSmoothImage(1f, titleLogo));

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ShowSmoothText(1f, 0.5f, 0f, thankMsg));

        yield return new WaitForSeconds(2f);
        _finish = true;
        
    }

    private IEnumerator ShowSmoothImage(float saturation, Image target)
    {
        float alpha = 0;
        target.color = new Color(saturation, saturation, saturation, 0f);
        target.gameObject.SetActive(true);

        while (alpha < 1f)
        {
            alpha += 0.01f;
            yield return new WaitForSeconds(0.01f);
            target.color = new Color(saturation, saturation, saturation, alpha);
        }
    }

    private IEnumerator ShowSmoothText(float r, float g, float b, TMP_Text target)
    {
        float alpha = 0;
        target.color = new Color(r, g, b, 0f);
        target.gameObject.SetActive(true);

        while (alpha < 1f)
        {
            alpha += 0.01f;
            yield return new WaitForSeconds(0.01f);
            target.color = new Color(r, g, b, alpha);
        }
    }
}
