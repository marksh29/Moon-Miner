using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Host : MonoBehaviour
{
    [SerializeField] float toTargetTime;
    [SerializeField] Transform target;
    public bool home;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void StartMove(float time, Transform trgt)
    {
        GetComponent<SphereCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;

        StartCoroutine(DoMove(time, trgt));
    }
    public void StartHomeMove(float time, Transform trgt)
    {
        GetComponent<SphereCollider>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        StartCoroutine(DoMove(time, trgt));
    }

    public IEnumerator DoMove(float time, Transform trgt)
    {
        target = trgt;

        

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
    private void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "HostDrop")
        {
            coll.gameObject.GetComponent<Build>().AddHost();
            Destroy(gameObject);
        }
    }
}
