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
        anim.SetBool("isSoaping", true);
    }

    public void StopSoapAnimation()
    {
        anim.SetBool("isSoaping", false);
    }

    public void StartShowerAnimation()
    {
        anim.SetBool("isWashing", true);
    }

    public void StopShowerAnimation()
    {
        anim.SetBool("isWashing", false);
    }

    public void StartTowelAnimation()
    {
        anim.SetBool("isWiping", true);
    }

    public void StopTowelAnimation()
    {
        anim.SetBool("isWiping", false);
    }

    public void ChangeToTowelAnimation()
    {
        AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);
        float normalizedTime = currentState.normalizedTime % 1f;

        anim.SetBool("isWashing", false);
        anim.SetBool("isWiping", true);

        anim.Play("Towel_Progress", 0, normalizedTime);
    }
}
