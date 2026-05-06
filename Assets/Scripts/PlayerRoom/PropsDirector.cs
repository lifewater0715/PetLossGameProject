using UnityEngine;

public class PropsDirector : MonoBehaviour
{

    public void PlayEvent(PropsInfo propsInfo)
    {

        if (propsInfo.GetRequiredTurn != PropsTurn.Turn)
        {
            Debug.Log("상호 작용 불가능");
            return;
        }

        if (propsInfo.GetPropsEvent == null)
        {
            Debug.Log("이벤트 없음");
            return;
        }
        
        propsInfo.GetPropsEvent.Play();
        PropsTurn.NextTurn();
    }
}
