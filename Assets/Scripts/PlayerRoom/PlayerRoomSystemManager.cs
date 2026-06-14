using UnityEngine;

public class PlayerRoomSystemManager : MonoBehaviour
{
    [SerializeField] private PropsGlowHighlightController glow;

    private void Start()
    {
        SetGlowAnim();
    }

    private void SetGlowAnim()
    {
        switch (PropsTurn.Turn)
        {
            case 1:
                glow.SetPictureHighlight(true);
                break;
            case 2:
                glow.SetBallHighlight(true);
                break;
            case 3:
                glow.SetTugHighlight(true);
                break;
            case 4:
                glow.SetLeashHighlight(true);
                break;
            case 5:
                glow.SetShampooHighlight(true);
                break;
        }
    }
}
