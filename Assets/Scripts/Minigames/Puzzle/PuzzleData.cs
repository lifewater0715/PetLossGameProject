using System.Collections;
using UnityEngine;


public class PuzzleData : MonoBehaviour
{
    [SerializeField] private GameObject startpos;
    [SerializeField] private GameObject endpos;
    [SerializeField] private float retruntime = 0.2f;
    [SerializeField] private bool onclickend;
    [SerializeField] private bool onpuzzlefail = false;


    void Start()
    {
    }

    void FixedUpdate()
    {
        if (onclickend == true && onpuzzlefail == true)
        {
            Debug.Log("퍼즐 실패");

            StartCoroutine(RetrunStartPos(retruntime, gameObject.transform.position, startpos.transform.position));
            onclickend = false;
        }
    }

    public void ClickSet(bool Clickdata)
    {
        onclickend = Clickdata;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject == endpos && onclickend == true)
        {
            Debug.Log("클릭 놓음" + other.gameObject.name);
            StartCoroutine(RetrunStartPos(retruntime, gameObject.transform.position, endpos.transform.position));
        }
    }

    //퍼즐 무버 쌰갈!
    private IEnumerator RetrunStartPos(float retruntime, Vector3 startpos, Vector3 targetpos)
    {
        float timestack = 0f;

        while (timestack < retruntime)
        {
            timestack += Time.deltaTime;
            transform.position = Vector3.Lerp(startpos, targetpos, Mathf.Clamp01(timestack / retruntime));

            yield return null;
        }

        transform.position = targetpos;
    }
}
