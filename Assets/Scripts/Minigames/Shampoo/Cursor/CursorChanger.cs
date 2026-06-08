using UnityEngine;

public class CursorChanger : MonoBehaviour
{
    [SerializeField] private Sprite Brush;
    [SerializeField] private Sprite Shower;
    [SerializeField] private Sprite Towel;

    public Sprite ChangeCursorImage(CursorType type)
    {
        Sprite returnImage = null;

        switch (type)
        {
            case CursorType.Shampoo:
                returnImage = Brush;
                break;
            case CursorType.Shower:
                returnImage = Shower;
                break;
            case CursorType.Towel:
                returnImage = Towel;
                break;
            default:
                break;
        }

        return returnImage;
    }
}
