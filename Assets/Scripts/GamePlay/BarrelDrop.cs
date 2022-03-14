using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelDrop : MonoBehaviour
{
    Transform target;
    int count;
    [SerializeField] float force, spawnTime, jumpTime, jumpForce;
    float removeScale; 
    bool fly, jump;
    float jumpTm;

    private void OnEnable()
    {
        jump = false;
        fly = true;
        StartCoroutine(FlyOff());
    }
    void Start()
    {
        
    }
    void Update()
    {
        if (jump)
        {
            jumpTm -= Time.deltaTime;
            if (jumpTm <= 0)
            {
                jumpTm = jumpTime;
                GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }
    public void StartDrop(Transform trg, int cnt, float scale)
    {
        transform.localScale = new Vector3(scale, scale, scale);
        count = cnt;
        target = trg;
        removeScale = transform.localScale.x / count;
        Vector3 _forcePos = trg.position - transform.position;
        GetComponent<Rigidbody>().AddForce(new Vector3(_forcePos.x, 2, _forcePos.z) * force, ForceMode.Impulse);
        GetComponent<Rigidbody>().AddTorque(Vector3.forward * force/3, ForceMode.Impulse);
    }
    IEnumerator StartSpawnBarrel()
    {
        for (int i = 0; i < count; i++)
        {
            SpawnBarrel();
            yield return new WaitForSeconds(spawnTime);
        }       
    }
    void SpawnBarrel()
    {
        transform.localScale -= new Vector3(removeScale, removeScale, removeScale);
        GameObject obj = PoolControll.Instance.Spawn("Box");
        obj.transform.position = transform.position;
        StartCoroutine(obj.GetComponent<Box>().DoMove(0.3f, target.GetComponent<Factoria>().dropScrapPos));
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            if(!fly)
            {
                if (!jump)
                {
                    StartCoroutine(StartSpawnBarrel());
                    jump = true;
                }
                GetComponent<Rigidbody>().isKinematic = true;
            }                    
        }
    }
    IEnumerator FlyOff()
    {
        yield return new WaitForSeconds(0.3f);  
        fly = false;
    }
}
