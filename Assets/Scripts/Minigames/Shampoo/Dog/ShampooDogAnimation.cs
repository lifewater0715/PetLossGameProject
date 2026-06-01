using UnityEngine;

public class ShampooDogAnimation : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void StartSoapAnimation()
    {
        anim.SetTrigger("isSoaping");
    }

    public void StartShowerAnimation()
    {
        anim.SetTrigger("isWashing");
    }
}
