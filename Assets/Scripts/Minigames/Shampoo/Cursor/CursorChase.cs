using UnityEngine;
using UnityEngine.UI;

public class CursorChase : MonoBehaviour
{
    [SerializeField] private CursorChanger cursorChanger;
    [SerializeField] private GameObject cursor;
    [SerializeField] private CursorBtn cursorBtn;

    private RectTransform _cursorPosition;
    private Image _cursorImage;

    private void Awake()
    {
        _cursorPosition = cursor.GetComponent<RectTransform>();
        _cursorImage = cursor.GetComponent<Image>();
        
        cursor.SetActive(false);
    }

    private void OnEnable()
    {
        cursorBtn.OnChangeTool += ChangeCursor;
    }

    private void OnDisable()
    {
        cursorBtn.OnChangeTool -= ChangeCursor;
    }

    private void Update()
    {
        _cursorPosition.position = Input.mousePosition;
    }

    public void ChangeCursor(CursorType type)
    {
        cursor.SetActive(true);
        Cursor.visible = false;
        _cursorImage.sprite = cursorChanger.ChangeCursorImage(type);
    }
}
