using UnityEngine;

public class SFXAudio : MonoBehaviour
{
    private AudioSource audioSource;
    private bool _isPaused;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        if (_isPaused)
        {
            audioSource.UnPause();
            _isPaused = false;
            return;
        }

        if (audioSource.isPlaying) return;

        audioSource.Play();
    }

    public void StopSound()
    {
        audioSource.Stop();
        _isPaused = false;
    }

    public void PauseSound()
    {
        if (!audioSource.isPlaying) return;

        audioSource.Pause();
        _isPaused = true;
    }

    public void UnPauseSound()
    {
        audioSource.UnPause();
        _isPaused = false;
    }
}
