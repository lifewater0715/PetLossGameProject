using System.Collections;
using UnityEngine;

public class EndingDogFade : MonoBehaviour
{
    [SerializeField] private SpriteRenderer dogSprite;

    private void Awake()
    {
        dogSprite.gameObject.SetActive(false);
    }

    public void ShowDog()
    {
        StartCoroutine(CShowDog());
    }
    
    private IEnumerator CShowDog()
    {
        float alpha = 0;
        dogSprite.color = new Color(1f, 1f, 1f, 0f);
        dogSprite.gameObject.SetActive(true);

        while (alpha < 1f)
        {
            alpha += 0.01f;
            yield return new WaitForSeconds(0.01f);
            dogSprite.color = new Color(1f, 1f, 1f, alpha);
        }
    }
}
