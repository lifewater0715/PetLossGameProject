using UnityEngine;

public class PlayerRoomBGMStarter : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;

    private void Start()
    {
        if (BGMManager.Instance == null) return;

        if (PropsTurn.Turn == 1)
            BGMManager.Instance.PlaySound(audioClip);
    }
}
