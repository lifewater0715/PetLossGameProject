using UnityEngine;

public class BallDogAnimation : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private BallThrowController ballThrowController;

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
        
        ballThrowController.OnThrow += StartRunAnim;
    }

    private void OnDisable()
    {
        if (ballThrowController == null) return;
        
        ballThrowController.OnThrow -= StartRunAnim;
    }

    public void StartRunAnim()
    {
        anim.SetBool("isRun", true);
    }

    public void StopRunAnim()
    {
        anim.SetBool("isRun", false);
    }

    public void StartCatchIdleAnim()
    {
        anim.SetBool("isCatch", true);
    }

    public void StopCatchIdleAnim()
    {
        anim.SetBool("isCatch", false);
    }
}
