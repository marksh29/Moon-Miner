using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float toTargetTime;
    [SerializeField] Transform target;
    public Vector3 upPos;
   
    void Start()
    {
        rb.isKinematic = false;
    }

    public IEnumerator DoMove(float time, Transform trgt)
    {
        target = trgt;
       
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;

        transform.parent = target;
        transform.rotation = target.rotation;

        Vector3 startPosition = transform.localPosition;
        Vector3 targetPosition = new Vector3(transform.localPosition.x, transform.localPosition.y < 0 ? target.position.y + 3 : transform.localPosition.y + 1f, transform.localPosition.z);

        float startTime = Time.realtimeSinceStartup;
        float fraction = 0f;
        while (fraction < 1f)
        {
            fraction = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / time);
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, fraction);
            yield return null;
        }
        StartCoroutine(MoveToTarget());
    }
    public IEnumerator MoveToTarget()
    {
        transform.parent = target;
        Vector3 startPosition = transform.localPosition;
        float startTime = Time.realtimeSinceStartup;
        float fraction = 0f;
        while (fraction < 1f)
        {
            fraction = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / toTargetTime);
            transform.localPosition = Vector3.Lerp(startPosition, new Vector3(0, 0, 0), fraction);               
            yield return null;
        }
        //transform.position = target.position;
        //transform.rotation = target.rotation;
    }

    //public IEnumerator DoMove(float time, float yy, Transform trgt)
    //{
    //    target = trgt;
    //    transform.parent = target;
    //    GetComponent<BoxCollider>().enabled = false;
    //    GetComponent<Rigidbody>().isKinematic = true;

    //    Vector3 startPosition = transform.localPosition;
    //    Vector3 targetPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + yy, transform.localPosition.z);

    //    float startTime = Time.realtimeSinceStartup;
    //    float fraction = 0f;
    //    while (fraction < 1f)
    //    {
    //        fraction = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / time);
    //        transform.localPosition = Vector3.Lerp(startPosition, targetPosition, fraction);
    //        yield return null;
    //    }
    //    StartCoroutine(MoveToTarget());
    //}
    //public IEnumerator MoveToTarget()
    //{
    //    transform.parent = target;
    //    Vector3 startPosition = transform.localPosition;
    //    float startTime = Time.realtimeSinceStartup;
    //    float fraction = 0f;
    //    while (fraction < 1f)
    //    {
    //        fraction = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / toTargetTime);
    //        transform.localPosition = Vector3.Lerp(startPosition, new Vector3(0, 0, 0), fraction);
    //        yield return null;
    //    }
    //    //transform.position = target.position;
    //    //transform.rotation = target.rotation;
    //}
}
