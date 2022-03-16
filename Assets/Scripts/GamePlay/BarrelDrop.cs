using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelDrop : MonoBehaviour
{
    public bool scrapLand;
    [SerializeField] Transform target;
    [SerializeField] int count;
    [SerializeField] float force, spawnTime, jumpTime, jumpForce;
    float removeScale;
    [SerializeField] bool fly, jump;
    float jumpTm;

    private void OnEnable()
    {
        jump = false;
        fly = true;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
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
                jump = false;
                jumpTm = jumpTime;
                GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }
    public void StartDrop(Transform trg, int cnt, float scale, bool land)
    {
        scrapLand = land;
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
        gameObject.SetActive(false);
    }
    void SpawnBarrel()
    {
        transform.localScale -= new Vector3(removeScale, removeScale, removeScale);
        if(!scrapLand)
        {
            GameObject obj = PoolControll.Instance.Spawn("Box");
            obj.transform.position = transform.position;
            StartCoroutine(obj.GetComponent<Box>().DoMove(0.3f, target.GetComponent<Factoria>().dropScrapPos));
        }
        else
        {
            if (target.gameObject.GetComponent<Build>() != null)
            {
                GameObject obj = PoolControll.Instance.Spawn("Box");
                obj.transform.position = transform.position;
                StartCoroutine(obj.GetComponent<Box>().DoMove(0.3f, target.GetComponent<Factoria>().dropScrapPos));
            }
            else
            {
                if (target.gameObject.GetComponent<BuildLand>().buildCount > 0)
                {
                    GameObject obj = PoolControll.Instance.Spawn("Box");
                    obj.transform.position = transform.position;
                    target.gameObject.GetComponent<BuildLand>().BuildCount();
                    obj.GetComponent<Box>().scrapLand = scrapLand;
                    StartCoroutine(obj.GetComponent<Box>().DoMove(0.3f, target.gameObject.GetComponent<BuildLand>().NextBlock()));
                }
                else
                {
                    count = 0;
                    gameObject.SetActive(false);
                }
            }
        }           
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            if(!fly)
            {
                StartCoroutine(StartSpawnBarrel());
                fly = true;
            }
            if(!jump)
            {
                jump = true;                
            }
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }
    IEnumerator FlyOff()
    {
        yield return new WaitForSeconds(1f);  
        fly = false;
    }
}
