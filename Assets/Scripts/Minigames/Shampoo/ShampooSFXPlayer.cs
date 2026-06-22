using UnityEngine;

public class ShampooSFXPlayer : MonoBehaviour
{
    [SerializeField] private CursorEvent cursorEvent;
    [SerializeField] private CursorBtn cursorBtn;
    [SerializeField] private ShampooSystemManager shampooSystemManager;

    [SerializeField] private SFXAudio shampooSound;
    [SerializeField] private SFXAudio showerSound;
    [SerializeField] private SFXAudio towelSound;

    private CursorType _cursorType = CursorType.None;
    private SFXAudio _playingSound;

    private void OnEnable()
    {
        cursorBtn.OnChangeTool += ChangeTool;
        cursorEvent.OnRubbed += PlayRubSound;
        cursorEvent.OnRubbedStop += PauseCurrentSound;
        cursorEvent.OnHeldOnDog += PlayHeldSound;
        cursorEvent.OnHeldOnDogStop += PauseCurrentSound;
    }

    private void OnDisable()
    {
        cursorBtn.OnChangeTool -= ChangeTool;
        cursorEvent.OnRubbed -= PlayRubSound;
        cursorEvent.OnRubbedStop -= PauseCurrentSound;
        cursorEvent.OnHeldOnDog -= PlayHeldSound;
        cursorEvent.OnHeldOnDogStop -= PauseCurrentSound;
    }

    private void ChangeTool(CursorType type)
    {
        _cursorType = type;
        PauseCurrentSound();
    }

    private void PlayRubSound()
    {
        if (_cursorType == CursorType.Shower) return;

        if (_cursorType == CursorType.Shampoo && shampooSystemManager.Turn == 1)
        {
            PlaySound(shampooSound);
            return;
        }

        if (_cursorType == CursorType.Towel && shampooSystemManager.Turn == 3)
        {
            PlaySound(towelSound);
            return;
        }

        StopCurrentSound();
    }

    private void PlayHeldSound()
    {
        if (_cursorType == CursorType.Shower && shampooSystemManager.Turn == 2)
        {
            PlaySound(showerSound);
            return;
        }

        if (_cursorType == CursorType.Shower)
        {
            StopCurrentSound();
        }
    }

    private void PlaySound(SFXAudio sound)
    {
        if (sound == null) return;
        if (_playingSound == sound) return;

        StopCurrentSound();
        sound.PlaySound();
        _playingSound = sound;
    }

    private void PauseCurrentSound()
    {
        if (_playingSound == null) return;

        _playingSound.PauseSound();
        _playingSound = null;
    }

    private void StopCurrentSound()
    {
        if (_playingSound == null) return;

        _playingSound.StopSound();
        _playingSound = null;
    }
}
