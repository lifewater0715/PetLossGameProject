using UnityEngine;

public class UserImgManager : MonoBehaviour
{
    private Sprite Userimg;
    private Vector3 userimgcuttingpos;
    private GameObject Mask;

    void Awake()
    {
        Mask.transform.position = userimgcuttingpos;
    }
}
