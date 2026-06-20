using System.Collections;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance { get; private set; }
    
    [SerializeField] private float fadeDuration = 1.5f;

    private AudioLowPassFilter lowPassFilter;
    private AudioSource audioSource;
    private Coroutine RunningCoroutine;

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
        SetFilterMode(AudioLevel.None);
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
        switch (audioLevel)
        {
            case AudioLevel.None:
                lowPassFilter.cutoffFrequency = 22000f;
                audioSource.volume = 1f;
                break;
            case AudioLevel.Low:
                lowPassFilter.cutoffFrequency = 10000f;
                audioSource.volume = 0.7f;
                break;
            case AudioLevel.Middle:
                lowPassFilter.cutoffFrequency = 5000f; 
                audioSource.volume = 0.7f;
                break;
            case AudioLevel.High:
                lowPassFilter.cutoffFrequency = 600f; 
                audioSource.volume = 0.7f;
                break;
        }
    }
}
