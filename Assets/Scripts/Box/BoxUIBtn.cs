using UnityEngine;

public class BoxUIBtn : MonoBehaviour
{
    [SerializeField] private GameObject picturePanel;
    [SerializeField] private GameObject ballPanel;
    [SerializeField] private GameObject tugPanel;
    [SerializeField] private GameObject leashPanel;
    [SerializeField] private GameObject shampooPanel;

    public void PicturePanelActive()
    {
        picturePanel.SetActive(true);
        ballPanel.SetActive(false);
        tugPanel.SetActive(false);
        leashPanel.SetActive(false);
        shampooPanel.SetActive(false);
    }

    public void BallPanelActive()
    {
        picturePanel.SetActive(false);
        ballPanel.SetActive(true);
        tugPanel.SetActive(false);
        leashPanel.SetActive(false);
        shampooPanel.SetActive(false);
    }

    public void TugPanelActive()
    {
        picturePanel.SetActive(false);
        ballPanel.SetActive(false);
        tugPanel.SetActive(true);
        leashPanel.SetActive(false);
        shampooPanel.SetActive(false);
    }

    public void LeashPanelActive()
    {
        picturePanel.SetActive(false);
        ballPanel.SetActive(false);
        tugPanel.SetActive(false);
        leashPanel.SetActive(true);
        shampooPanel.SetActive(false);
    }

    public void ShampooPanelActive()
    {
        picturePanel.SetActive(false);
        ballPanel.SetActive(false);
        tugPanel.SetActive(false);
        leashPanel.SetActive(false);
        shampooPanel.SetActive(true);
    }

    public void AllPanelDeActive()
    {
        picturePanel.SetActive(false);
        ballPanel.SetActive(false);
        tugPanel.SetActive(false);
        leashPanel.SetActive(false);
        shampooPanel.SetActive(false);
    }
}
