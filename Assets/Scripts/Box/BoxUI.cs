using UnityEngine;

public class BoxUI : MonoBehaviour
{
    [SerializeField] private GameObject[] props;

    private void Start()
    {
        for (int i = 0; i < props.Length; i++)
        {
            props[i].SetActive(false);
        }

        AddPropsBtn();
    }

    private void AddPropsBtn()
    {
        if (PropsTurn.Turn == 1) return;

        for (int i = 1; i < PropsTurn.Turn; i++)
        {
            props[i - 1].SetActive(true);
        }
    }
}
