using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float toTargetTime;
    [SerializeField] Transform target;
    public Vector3 upPos;

    private void OnEnable()
    {
        int id = Random.Range(0, transform.childCount);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == id ? true : false);
        }
    }
    void Start()
    {
        
    }
    public IEnumerator DoMove(float time, Transform trgt)
    {
        target = trgt;
        transform.parent = target;
        transform.rotation = target.rotation;

        Vector3 startPosition = transform.localPosition;
        Vector3 targetPosition = new Vector3(transform.localPosition.x, transform.localPosition.y < 0 ? 5 : 1, transform.localPosition.z);

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
        Vector3 startPosition = transform.localPosition;
        float startTime = Time.realtimeSinceStartup;
        float fraction = 0f;
        while (fraction < 1f)
        {
            fraction = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / toTargetTime);
            transform.localPosition = Vector3.Lerp(startPosition, new Vector3(0, 0, 0), fraction);               
            yield return null;
        }
        transform.localPosition = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "AddScrap")
        {
            gameObject.SetActive(false);
            other.gameObject.transform.parent.parent.GetComponent<Factoria>().AddScrap();
        }
    }
}
