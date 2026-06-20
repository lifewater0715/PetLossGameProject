using System.Collections;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance { get; private set; }
    
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private float filterFadeDuration = 1f;

    private AudioLowPassFilter lowPassFilter;
    private AudioSource audioSource;
    private Coroutine RunningCoroutine;
    private Coroutine filterCoroutine;

    public enum AudioLevel
    {
        None, Low, Middle, High
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        lowPassFilter = GetComponent<AudioLowPassFilter>();
        audioSource = GetComponent<AudioSource>();
        SetFilterValues(22000f, 1f);
    }

    public void PlaySound(AudioClip audioClip)
    {
        audioSource.clip = audioClip;

        if (RunningCoroutine != null)
            StopCoroutine(RunningCoroutine);
        
        audioSource.volume = 1f;
        audioSource.Play();
    }

    public void StopSound()
    {
        if (RunningCoroutine != null) 
            StopCoroutine(RunningCoroutine);

        if (filterCoroutine != null)
            StopCoroutine(filterCoroutine);

        RunningCoroutine = StartCoroutine(CStopSound());
    }

    private IEnumerator CStopSound()
    {
        float startVolume = audioSource.volume;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();

        audioSource.volume = startVolume;
    }

    public void SetFilterMode(AudioLevel audioLevel)
    {
        float targetCutoffFrequency = 22000f;
        float targetVolume = 1f;

        switch (audioLevel)
        {
            case AudioLevel.None:
                targetCutoffFrequency = 22000f;
                targetVolume = 1f;
                break;
            case AudioLevel.Low:
                targetCutoffFrequency = 10000f;
                targetVolume = 0.7f;
                break;
            case AudioLevel.Middle:
                targetCutoffFrequency = 5000f; 
                targetVolume = 0.7f;
                break;
            case AudioLevel.High:
                targetCutoffFrequency = 600f; 
                targetVolume = 0.7f;
                break;
        }

        if (filterCoroutine != null)
            StopCoroutine(filterCoroutine);

        filterCoroutine = StartCoroutine(CSetFilterMode(targetCutoffFrequency, targetVolume));
    }

    private IEnumerator CSetFilterMode(float targetCutoffFrequency, float targetVolume)
    {
        float startCutoffFrequency = lowPassFilter.cutoffFrequency;
        float startVolume = audioSource.volume;
        float timer = 0f;

        while (timer < filterFadeDuration)
        {
            timer += Time.deltaTime;
            float ratio = timer / filterFadeDuration;

            lowPassFilter.cutoffFrequency = Mathf.Lerp(startCutoffFrequency, targetCutoffFrequency, ratio);
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, ratio);
            yield return null;
        }

        SetFilterValues(targetCutoffFrequency, targetVolume);
        filterCoroutine = null;
    }

    private void SetFilterValues(float cutoffFrequency, float volume)
    {
        lowPassFilter.cutoffFrequency = cutoffFrequency;
        audioSource.volume = volume;
    }
}
