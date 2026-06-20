using UnityEngine;

public class BGMStarter : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;

    private void Start()
    {
        if (BGMManager.Instance == null) return;

        BGMManager.Instance.PlaySound(audioClip);
    }
}
