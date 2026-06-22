using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitlePuzzleAlert : MonoBehaviour
{
    [SerializeField] private Image alertImage;
    [SerializeField] private TMP_Text alertMsg;
    [SerializeField] private GameObject borderImage;
    [SerializeField] private GameObject borderSprite;
 
    private void Awake()
    {
        alertImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            ShowSavePicture();
        }
    }
    
    public void ShowSavePicture()
    {
        StartCoroutine(CShowSavePicture());
    }

    private IEnumerator CShowSavePicture()
    {
        float alpha = 0f;

        alertImage.color = new Color(1f, 1f, 1f, alpha);
        alertMsg.color = new Color(0.3f, 0.1f, 0f, alpha);
        alertImage.gameObject.SetActive(true);

        while(alpha < 1f)
        {
            alpha += 0.025f;
            yield return new WaitForSeconds(0.01f);
            alertImage.color = new Color(1f, 1f, 1f, alpha);
            alertMsg.color = new Color(0.3f, 0.1f, 0f, alpha);
        }

        yield return new WaitForSeconds(3f);

        while(alpha > 0f)
        {
            alpha -= 0.025f;
            yield return new WaitForSeconds(0.01f);
            alertImage.color = new Color(1f, 1f, 1f, alpha);
            alertMsg.color = new Color(0.3f, 0.1f, 0f, alpha);
        }

        borderImage.SetActive(false);
        borderSprite.SetActive(false);
    }
}
