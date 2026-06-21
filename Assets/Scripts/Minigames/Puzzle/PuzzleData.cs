using System.Collections;
using UnityEngine;


public class PuzzleData : MonoBehaviour
{
    [SerializeField] private GameObject startpos;
    [SerializeField] private GameObject endpos;
    [SerializeField] private float retruntime = 0.2f;
    public bool onpuzzlesuccess = false;

    private bool isOnEndPos;
    private Coroutine moveCoroutine;

    void Start()
    {
    }

    public void ReleasePiece()
    {
        if (onpuzzlesuccess)
        {
            return;
        }

        Vector3 targetPosition = isOnEndPos ? endpos.transform.position : startpos.transform.position;

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = StartCoroutine(RetrunStartPos(retruntime, transform.position, targetPosition));

        if (isOnEndPos)
        {
            success();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == endpos)
        {
            isOnEndPos = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == endpos)
        {
            isOnEndPos = false;
        }
    }

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
        moveCoroutine = null;
    }

    private void success()
    {
        onpuzzlesuccess = true;
    }
}
