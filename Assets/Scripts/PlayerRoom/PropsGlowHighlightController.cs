using UnityEngine;

public class PropsGlowHighlightController : MonoBehaviour
{
    [SerializeField] private SpriteGlowHighlight picture;
    [SerializeField] private SpriteGlowHighlight ball;
    [SerializeField] private SpriteGlowHighlight tug;
    [SerializeField] private SpriteGlowHighlight leash;
    [SerializeField] private SpriteGlowHighlight shampoo;

    public void SetPictureHighlight(bool active)
    {
        picture.SetHighlight(active);
    }

    public void SetBallHighlight(bool active)
    {
        ball.SetHighlight(active);
    }

    public void SetTugHighlight(bool active)
    {
        tug.SetHighlight(active);
    }

    public void SetLeashHighlight(bool active)
    {
        leash.SetHighlight(active);
    }

    public void SetShampooHighlight(bool active)
    {
        shampoo.SetHighlight(active);
    }
}