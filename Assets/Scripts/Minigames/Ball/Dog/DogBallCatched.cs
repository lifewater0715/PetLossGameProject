using UnityEngine;

public class DogBallCatched : MonoBehaviour
{
    [SerializeField] private LayerMask ballLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((ballLayer.value & (1 << collision.gameObject.layer)) == 0) return;
        Debug.Log("공 잡음");
        collision.gameObject.SetActive(false);
    }
}
