using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelDrop : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] int count;
    [SerializeField] float force, removeScale, spawnTime;
    bool fly;

    private void OnEnable()
    {
        fly = true;
        StartCoroutine(FlyOff());
    }
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void StartDrop(Transform trg, int cnt, float scale)
    {
        transform.localScale = new Vector3(scale, scale, scale);
        count = cnt;
        target = trg;
        removeScale = transform.localScale.x / count;
        GetComponent<Rigidbody>().AddForce(new Vector3(-1.05f, 2.5f, 0) * force, ForceMode.Impulse);
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
        if(collision.gameObject.tag == "Ground" && !fly)
        {
            StartCoroutine(StartSpawnBarrel());
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }
    IEnumerator FlyOff()
    {
        yield return new WaitForSeconds(0.3f);
        fly = false;
    }
}
