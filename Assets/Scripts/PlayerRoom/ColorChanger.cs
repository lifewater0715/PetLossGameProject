using System.Collections;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    [Header("어두운 색")]
    [SerializeField] private SpriteRenderer floorBefore;
    [SerializeField] private SpriteRenderer wallsBefore;
    [SerializeField] private SpriteRenderer pictureAreaBefore;
    [SerializeField] private SpriteRenderer ballAreaBefore;
    [SerializeField] private SpriteRenderer tugAreaBefore;
    [SerializeField] private SpriteRenderer leashAreaBefore;
    [SerializeField] private SpriteRenderer shampooAreaBefore;

    [Header("밝은 색")]
    [SerializeField] private SpriteRenderer floorAfter;
    [SerializeField] private SpriteRenderer wallsAfter;
    [SerializeField] private SpriteRenderer pictureAreaAfter;
    [SerializeField] private SpriteRenderer ballAreaAfter;
    [SerializeField] private SpriteRenderer tugAreaAfter;
    [SerializeField] private SpriteRenderer leashAreaAfter;
    [SerializeField] private SpriteRenderer shampooAreaAfter;

    [Header("소품")]
    [SerializeField] private GameObject pictureProp;
    [SerializeField] private GameObject ballProp;
    [SerializeField] private GameObject tugProp;
    [SerializeField] private GameObject leashProp;
    [SerializeField] private GameObject shampooProp;

    private void Awake()
    {
        floorAfter.gameObject.SetActive(false);
        wallsAfter.gameObject.SetActive(false);
        pictureAreaAfter.gameObject.SetActive(false);
        ballAreaAfter.gameObject.SetActive(false);
        tugAreaAfter.gameObject.SetActive(false);
        leashAreaAfter.gameObject.SetActive(false);
        shampooAreaAfter.gameObject.SetActive(false);

        TurnCheck();
    }

    private void TurnCheck()
    {
        switch (PropsTurn.Turn)
        {
            case 2:
                PictureColorChange();
                break;
            case 3:
                BallColorChange();
                break;
            case 4:
                StartCoroutine(TugColorChange());
                break;
            case 5:
                StartCoroutine(LeashColorChange());
                break;
            case 6:
                ShampooColorChange();
                break;
        }
    }

    private void PictureColorChange()
    {
        pictureProp.SetActive(false);

        StartCoroutine(CObjectHide(pictureAreaBefore));
        StartCoroutine(CObjectShow(pictureAreaAfter));
    }

    private void BallColorChange()
    {
        PictureDone();

        ballProp.SetActive(false);
        StartCoroutine(CObjectHide(ballAreaBefore));
        StartCoroutine(CObjectShow(ballAreaAfter));
    }

    private IEnumerator TugColorChange()
    {
        PictureDone();
        BallDone();

        tugProp.SetActive(false);
        StartCoroutine(CObjectHide(floorBefore));
        StartCoroutine(CObjectShow(floorAfter));

        yield return new WaitForSeconds(0.2f);

        StartCoroutine(CObjectHide(tugAreaBefore));
        StartCoroutine(CObjectShow(tugAreaAfter));
    }

    private IEnumerator LeashColorChange()
    {
        PictureDone();
        BallDone();
        TugDone();

        FloorDone();

        leashProp.SetActive(false);
        StartCoroutine(CObjectHide(wallsBefore));
        StartCoroutine(CObjectShow(wallsAfter));

        yield return new WaitForSeconds(0.2f);

        StartCoroutine(CObjectHide(leashAreaBefore));
        StartCoroutine(CObjectShow(leashAreaAfter));
    }

    private void ShampooColorChange()
    {
        PictureDone();
        BallDone();
        TugDone();
        LeashDone();

        FloorDone();
        WallsDone();

        shampooProp.SetActive(false);
        StartCoroutine(CObjectHide(shampooAreaBefore));
        StartCoroutine(CObjectShow(shampooAreaAfter));
    }

    private IEnumerator CObjectHide(SpriteRenderer sprite)
    {
        sprite.color = new Color(1f, 1f, 1f, 1f);
        sprite.gameObject.SetActive(true);
        float alpha = 1f;
        while (alpha > 0f)
        {
            alpha -= 0.01f;
            yield return new WaitForSecondsRealtime(0.01f);
            sprite.color = new Color(1f, 1f, 1f, alpha);
        }
        sprite.gameObject.SetActive(false);
    }

    private IEnumerator CObjectShow(SpriteRenderer sprite)
    {
        sprite.color = new Color(1f, 1f, 1f, 0f);
        sprite.gameObject.SetActive(true);
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += 0.01f;
            yield return new WaitForSecondsRealtime(0.01f);
            sprite.color = new Color(1f, 1f, 1f, alpha);
        }
    }

    private void FloorDone()
    {
        floorBefore.gameObject.SetActive(false);
        floorAfter.gameObject.SetActive(true);
    }

    private void WallsDone()
    {
        wallsBefore.gameObject.SetActive(false);
        wallsAfter.gameObject.SetActive(true);
    }

    private void PictureDone()
    {
        pictureProp.SetActive(false);
        pictureAreaBefore.gameObject.SetActive(false);
        pictureAreaAfter.gameObject.SetActive(true);
    }

    private void BallDone()
    {
        ballProp.SetActive(false);
        ballAreaBefore.gameObject.SetActive(false);
        ballAreaAfter.gameObject.SetActive(true);
    }

    private void TugDone()
    {
        tugProp.SetActive(false);
        tugAreaBefore.gameObject.SetActive(false);
        tugAreaAfter.gameObject.SetActive(true);
    }

    private void LeashDone()
    {
        leashProp.SetActive(false);
        leashAreaBefore.gameObject.SetActive(false);
        leashAreaAfter.gameObject.SetActive(true);
    }

    private void ShampooDone()
    {
        shampooProp.SetActive(false);
    }
}
