using UnityEngine;

public class BallPlayerAnimation : MonoBehaviour
{
    [SerializeField] private BallThrowController ballThrowController;
    private Animator anim;

#if UNITY_EDITOR
    [ContextMenu("Auto Assign")]
    private void AutoAssign()
    {
        ballThrowController = GameObject.Find("BallThrowManager").GetComponent<BallThrowController>();
    }
#endif

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (ballThrowController == null) return;
        
        ballThrowController.OnThrow += StartShootAnim;
    }

    private void OnDisable()
    {
        if (ballThrowController == null) return;
        
        ballThrowController.OnThrow -= StartShootAnim;
    }

    public void StartShootAnim()
    {
        anim.SetBool("isThrow", true);
    }

    public void StopShootAnim()
    {
        anim.SetBool("isThrow", false);
    }
}
