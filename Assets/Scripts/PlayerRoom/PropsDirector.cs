using UnityEngine;

public class PropsDirector : MonoBehaviour
{
    public bool TryPlayEvent(PropsInfo propsInfo)
    {
        if (propsInfo == null) return false;

        if (!propsInfo.CanInteract(PropsTurn.Turn))
        {
            Debug.Log("상호 작용 불가능");
            return false;
        }

        if (!propsInfo.TryPlayEvent())
        {
            Debug.Log("이벤트 없음");
            return false;
        }

        PropsTurn.NextTurn();
        return true;
    }
}
