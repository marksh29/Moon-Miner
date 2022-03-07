using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostSpawner : MonoBehaviour
{
    [SerializeField] int minHost, maxHost, limitHost;
    [SerializeField] GameObject hostPrefab;
    [SerializeField] float xx, zz;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SpawnHosts();
    }
    public void SpawnHosts()
    {
        int count = Random.Range(minHost, maxHost);
        for (int i =0; i < count; i++)
        {
            if(transform.childCount - 1 < limitHost)
            {
                GameObject obj = Instantiate(hostPrefab, transform.position, transform.rotation, transform) as GameObject;
                obj.transform.localPosition = new Vector3(Random.Range(-xx, xx ), transform.position.y + hostPrefab.transform.localScale.y , Random.Range(-zz, zz));
            }            
        }
    }
}
