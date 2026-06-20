using System.Collections;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance { get; private set; }
    
    [SerializeField] private float fadeDuration = 1.5f;

    private AudioSource audioSource;
    private Coroutine RunningCoroutine;

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

        audioSource = GetComponent<AudioSource>();
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
}
