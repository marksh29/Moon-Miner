using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField] int _addMoney;
    [SerializeField] Transform _target;
    [SerializeField] float _dropForce;
    private void OnEnable()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        GetComponent<BoxCollider>().isTrigger = false;
        GetComponent<Rigidbody>().useGravity = true;
        //move = false;
       
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-2f, 2f), 3.5f, Random.Range(-2f, -0.1f)) * _dropForce, ForceMode.Impulse);
        StartCoroutine(DoMove(0.5f));
    }   
    void Update()
    {       
    }
    private IEnumerator DoMove(float time)
    {
        yield return new WaitForSeconds(1);
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<BoxCollider>().isTrigger = true;

        Vector3 startPosition = transform.position;
        float startTime = Time.realtimeSinceStartup;
        float fraction = 0f;
        while (fraction < 1f)
        {
            fraction = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / time);
            transform.position = Vector3.Lerp(startPosition, _target.position, fraction);
            yield return null;
        }
        //Controll.Instance.ChangeMoney(_addMoney);
        Destroy(gameObject);
    }
}
