using UnityEngine;

public class ShampooUIGlowHighlightController : MonoBehaviour
{
    [SerializeField] private UIGlowHighlight ShampooUI;
    [SerializeField] private UIGlowHighlight ShowerUI;
    [SerializeField] private UIGlowHighlight TowelUI;

    public void SetShampooUIHighlight(bool active)
    {
        ShampooUI.SetHighlight(active);
    }

    public void SetShowerUIHighlight(bool active)
    {
        ShowerUI.SetHighlight(active);
    }

    public void SetTowelUIHighlight(bool active)
    {
        TowelUI.SetHighlight(active);
    }
}